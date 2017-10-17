namespace CodereviewWorkItemGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.UserDataTypes;
    using ICSSoft.STORMNET.Windows.Forms;

    using IIS.CodeReviewEngine;

    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.Framework.Client;
    using Microsoft.TeamFoundation.Framework.Common;
    using Microsoft.TeamFoundation.VersionControl.Client;
    using Microsoft.TeamFoundation.WorkItemTracking.Client;

    /// <summary>
    /// Класс для генерации <see cref="WorkItem"/> для выполнения Codereview.
    /// </summary>
    public class WorkItemCreator
    {
        /// <summary>
        /// Наименование типа связанного контекста для указания номера Changeset'а у задачи.
        /// </summary>
        private const string WorkItemContextTypeForChangeset = "Набор изменений";

        /// <summary>
        /// Наименование связи дувух задач: запроса и ответа. Значение константы совпадает со стандартным наименованием дочерней связи из TFS-шаблона.
        /// </summary>
        private const string LinkChildWorkItem = "Дочерний элемент";

        /// <summary>
        /// Причина создания <see cref="WorkItem"/>. Значение константы совпадает с DEFAULTREASON из TFS-шаблона.
        /// </summary>
        private const string ReasonWorkItemCreation = "Новый";

        /// <summary>
        /// Максимальное количество Chngeset'ов обрабатываемых за раз. По умочанию стоит не большое, чтобы не загружать процесс.
        /// </summary>
        private readonly int _maxCountChangeSets;

        /// <summary>
        /// Время запуска процесса. Необходимо запомнить время запуска изначально на случай, если процесс будет выполняться на временной границе между двумя днями.
        /// </summary>
        private readonly DateTime _now;

        /// <summary>
        /// Вспомогательный элемент для получения случайных чисел.
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// Создать экземпляр класса для генерации <see cref="WorkItem"/> для выполнения Codereview.
        /// </summary>
        public WorkItemCreator()
        {
            _maxCountChangeSets = int.Parse(ConfigurationManager.AppSettings["MaxCountChangeSets"]);
            _now = DateTime.Now;
            _random = new Random();
        }

        /// <summary>
        /// Запустить процесс генерации задач на проверку кода.
        /// </summary>
        public void Run()
        {
            try
            {
                DataObject[] dataObjects = DataServiceProvider.DataService.LoadObjects(TFSProject.Views.TFSProjectE);

                foreach (TFSProject tfsProject in dataObjects)
                {
                    try
                    {
                        ProcessProject(tfsProject);
                    }
                    catch (Exception exception)
                    {
                        LogService.LogErrorFormat("Ошибка при обработке проекта {0}. {1}", tfsProject.Name, exception.ToString());
                    }
                    finally
                    {
                        // В случае, если обработка конкретного проекта упала, обновим у него время последнего проверенного Changeset'а.
                        DataServiceProvider.DataService.UpdateObject(tfsProject);
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.LogError(ex);
            }
        }

        /// <summary>
        /// Обработать конкретный проект из TFS.
        /// Выполняет генерацию задач на проверку кода для указанного прокта.
        /// </summary>
        /// <param name="tfsProject">Проект из TFS для которого необходимо сгенерировать задачи на проверку кода.</param>
        private void ProcessProject(TFSProject tfsProject)
        {
            // В проекте необходимо указывать последнию дату проверки иначе она будет проставлена в текущий день.
            if (tfsProject.LastCheckDate == null)
                tfsProject.LastCheckDate = new NullableDateTime { Value = _now };

            List<TeamSlice> teamSlices = GetTeamSlices(tfsProject);

            // Если нет команды в текущий период времени, то и CheckIn'ы нет смысла перебирать.
            if (teamSlices.Count == 0)
                return;

            string projectName = tfsProject.GetProjectName();
            string tfsServerUrl = tfsProject.GetProjectCollectionUrl();

            var projectCollection = new TfsTeamProjectCollection(new Uri(tfsServerUrl));
            projectCollection.EnsureAuthenticated();

            // Получаем changeset'ы, начиная с даты последней проверки по настоящий момент.
            IEnumerable<Changeset> changesets = GetChangesets(projectCollection, projectName, tfsProject.LastCheckDate.Value, _now);

            if (projectName.Contains('/'))
            {
                projectName = projectName.Split('/')[0];
            }

            // Сервис для работы с пользователями tfs (будем получать пользователя по его логину полному).
            IIdentityManagementService identityManagementService = projectCollection.GetService<IIdentityManagementService>();

            foreach (Changeset changeset in changesets)
            {
                var creationDate = changeset.CreationDate;
                var teamSlice = teamSlices.FirstOrDefault(n => n.StartDate <= creationDate && n.EndDate >= creationDate);

                if (teamSlice == null)
                {
                    LogService.LogWarn($"Невозможно подобрать команду под указанный диапазон дат. Changeset ${changeset.ChangesetId}");
                    continue;
                }

                var identity = identityManagementService.ReadIdentity(
                    IdentitySearchFactor.AccountName, changeset.Committer, MembershipQuery.None, ReadIdentityOptions.None);
                if (identity == null)
                {
                    throw new InvalidOperationException(
                        $"Не удалось получить сведения по логину {changeset.Committer}.");
                }

                /* Проведение имперсонализации: 
                 * создаём review request от имени того, кто коммитил (это позволит потом этому человеку закрыть review request).
                 * Чтобы всё корректно проходило, у пользователя, от имени которого работает данный сервис,
                 * должно быть разрешение “Make requests on behalf of others”.
                 */
                var impersonatedCollection = new TfsTeamProjectCollection(projectCollection.Uri, identity.Descriptor);
                impersonatedCollection.EnsureAuthenticated();
                WorkItemStore impersonatedWorkitemStore = impersonatedCollection.GetService<WorkItemStore>();
                Project impersonatedProject = impersonatedWorkitemStore.Projects[projectName];
                
                // Программист, которому будет назначен чекин на проверку.
                TFSProgrammer programmer = GetRandomProgrammer(teamSlice, changeset.CommitterDisplayName);
                if (programmer == null)
                {
                    throw new InvalidOperationException("Команда в подобранный диапазон дат вовсе не содержит программистов или содержит только автора набора изменений.");
                }

                var responseWorkItem = new WorkItem(impersonatedProject.WorkItemTypes[TFSCommonCollectionContsntans.ResponseWorkItemType])
                    {
                        Title = CutWorkitemTitle(changeset.Comment)
                    };
                responseWorkItem.Fields["System.AssignedTo"].Value = programmer.Name;
                responseWorkItem.Fields["System.State"].Value = TFSCommonCollectionContsntans.StateWorkItemCreation;
                responseWorkItem.Fields["System.Reason"].Value = ReasonWorkItemCreation;
                responseWorkItem.Fields["System.AreaPath"].Value = tfsProject.WorkItemAreaPath ?? projectName;
                responseWorkItem.Fields["System.IterationPath"].Value = tfsProject.WorkItemIterationPath ?? projectName;

                SaveWorkItem(responseWorkItem, changeset.ChangesetId);

                var requestWorkItem = new WorkItem(impersonatedProject.WorkItemTypes[TFSCommonCollectionContsntans.RequestWorkItemType])
                    {
                        Title = CutWorkitemTitle(changeset.Comment)
                    };
                requestWorkItem.Fields["System.AssignedTo"].Value = changeset.CommitterDisplayName;
                requestWorkItem.Fields["Microsoft.VSTS.CodeReview.ContextType"].Value = WorkItemContextTypeForChangeset;
                requestWorkItem.Fields["Microsoft.VSTS.CodeReview.Context"].Value = changeset.ChangesetId;
                requestWorkItem.Fields["System.AreaPath"].Value = tfsProject.WorkItemAreaPath ?? projectName;
                requestWorkItem.Fields["System.IterationPath"].Value = tfsProject.WorkItemIterationPath ?? projectName;
                requestWorkItem.Fields["System.State"].Value = TFSCommonCollectionContsntans.StateWorkItemCreation;
                requestWorkItem.Fields["System.Reason"].Value = ReasonWorkItemCreation;
                WorkItemLinkTypeEnd linkTypeEnd = impersonatedWorkitemStore.WorkItemLinkTypes.LinkTypeEnds[LinkChildWorkItem];
                requestWorkItem.Links.Add(new RelatedLink(linkTypeEnd, responseWorkItem.Id));

                SaveWorkItem(requestWorkItem, changeset.ChangesetId);

                // Актуализируем дату последней проверки проекта, проставляю в неё время последнего проверенного набора изменений.
                tfsProject.LastCheckDate = new NullableDateTime { Value = changeset.CreationDate };
            }
            
            // В конце успешной проверки всех наборов изменений проставляем время последней проверки в текущее.
            tfsProject.LastCheckDate = new NullableDateTime { Value = _now };            
        }

        /// <summary>
        /// Обрезать заголовок для Workitem'а.
        /// </summary>
        /// <param name="title">Заголовок, который необходимо обрезать.</param>
        /// <returns>Обрезанный заголовок для Workitem'а.</returns>
        private string CutWorkitemTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                title = "<<<Пустой комментарий к набору изменений>>>";
            }

            return title.Length < 255 ? title : title.Substring(0, 255);
        }

        /// <summary>
        /// Получить случайного <see cref="TFSProgrammer"/> из среза команды.
        /// </summary>
        /// <param name="teamSlice">Срез команды для опредления доступных программистов.</param>
        /// <param name="exactProgrammerName">Имя программиста, который должен быть исключен из выборки.</param>
        /// <returns>Случайно полученный <see cref="TFSProgrammer"/>.</returns>
        private TFSProgrammer GetRandomProgrammer(TeamSlice teamSlice, string exactProgrammerName)
        {
            int maxCountReviewers = teamSlice.Reviewers.Count;

            if (maxCountReviewers == 0)
                return null;

            int index = _random.Next(maxCountReviewers);

            // Если случайный выбор не упал на программиста, которого стоит исключить из выборки, то возвращаем его.
            if (!teamSlice.Reviewers[index].TFSProgrammer.Name.Equals(exactProgrammerName, StringComparison.InvariantCultureIgnoreCase))
                return teamSlice.Reviewers[index].TFSProgrammer;

            // Если случайный выбор программиста упал не на первого в списке, то возвращаем предыдущего.
            if (index > 0)
                return teamSlice.Reviewers[index - 1].TFSProgrammer;
                
            // Если количество программистов в выборке больше одного, то вернем следующего, после выбранного изначально.
            if (maxCountReviewers > 1)
                return teamSlice.Reviewers[index + 1].TFSProgrammer;

            // Программист в выборке был всего один и его необходимо было исключить из неё. Вернем в этом случае null.
            return null;
        }

        /// <summary>
        /// Получить все <see cref="Changeset"/> для конретного проекта за указанный период.
        /// </summary>
        /// <param name="teamProjectCollection">Коллекция проектов в которой находится проект с Changeset'ами.</param>
        /// <param name="projectName">Наименование проекта с Changeset'ами.</param>
        /// <param name="startDate">Дата с которой следует начинать искать.</param>
        /// <param name="endDate">Дата до которой следует искать.</param>
        /// <returns>Все <see cref="Changeset"/> для конретного проекта за указанный период. </returns>
        private IEnumerable<Changeset> GetChangesets(TfsTeamProjectCollection teamProjectCollection, string projectName, DateTime startDate, DateTime endDate)
        {
            var versionControlServer = teamProjectCollection.GetService<VersionControlServer>();
            var history = versionControlServer.QueryHistory(
                @"$/" + projectName,
                VersionSpec.Latest,
                0,
                RecursionType.Full,
                null,
                null,
                VersionSpec.Latest,
                _maxCountChangeSets,
                false,
                false).Cast<Changeset>();

            return history.Where(n => n.CreationDate >= startDate && n.CreationDate <= endDate)
                .OrderBy(n => n.CreationDate).ToList();
        }

        /// <summary>
        /// Получить все <see cref="TeamSlice"/> для конкретного проекта с момента последней проверки.
        /// </summary>
        /// <param name="project">
        /// Проект для которого необходимо получить все <see cref="TeamSlice"/>.
        /// </param>
        /// <returns>
        /// Все <see cref="TeamSlice"/> для указанного проекта с момента последней проверки.
        /// </returns>
        private List<TeamSlice> GetTeamSlices(TFSProject project)
        {
            ExternalLangDef languageDef = ExternalLangDef.LanguageDef;
            LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(TeamSlice), TeamSlice.Views.TeamSliceE);
            VariableDef startDate = new VariableDef(languageDef.DateTimeType, Information.ExtractPropertyName<TeamSlice>(n => n.StartDate));
            VariableDef endDate = new VariableDef(languageDef.DateTimeType, Information.ExtractPropertyName<TeamSlice>(n => n.EndDate));

            // Условие: либо посленяя дата проверки или текущая дата попадают в дипазон дат среза команды, либо диапазон дат среза команды содержится между ними.
            Function dateLimit = languageDef.GetFunction(
                languageDef.funcOR,
                languageDef.GetFunction(languageDef.funcBETWEEN, project.LastCheckDate, startDate, endDate),
                languageDef.GetFunction(languageDef.funcBETWEEN, _now, startDate, endDate),
                languageDef.GetFunction(
                    languageDef.funcAND,
                    languageDef.GetFunction(languageDef.funcL, endDate, _now),
                    languageDef.GetFunction(languageDef.funcG, startDate, project.LastCheckDate)));

            string agregatorName = Information.ExtractPropertyName<TFSProjectForReview>(n => n.Team);
            string detailPath = Information.ExtractPropertyPath<TeamSlice>(n => n.Team);
            Function projectLimit = languageDef.GetFunction(
                languageDef.funcExist,
                new DetailVariableDef(languageDef.DetailsType, agregatorName, TFSProjectForReview.Views.TFSProjectForReviewE, agregatorName, detailPath),
                languageDef.GetFunction(
                    languageDef.funcEQ,
                    new VariableDef(languageDef.GuidType, Information.ExtractPropertyName<TFSProjectForReview>(n => n.TFSProject)),
                    project.__PrimaryKey));

            lcs.LimitFunction = languageDef.GetFunction(languageDef.funcAND, dateLimit, projectLimit);

            return DataServiceProvider.DataService.LoadObjects(lcs).Cast<TeamSlice>().ToList();
        }

        /// <summary>
        /// Сохранить рабочий элемент в TFS.
        /// </summary>
        /// <param name="workItem">Рабочий элемент для сохранения.</param>
        /// <param name="changesetNumber">Номер Changeset'а для адекватной выдачи ошибок валидации.</param>
        private void SaveWorkItem(WorkItem workItem, int changesetNumber)
        {
            var result = workItem.Validate();

            foreach (Field item in result)
                LogService.LogErrorFormat("Неверный формат поля: {0}. Changeset {1}.", item.Name, changesetNumber);

            workItem.Save();
        }
    }
}
