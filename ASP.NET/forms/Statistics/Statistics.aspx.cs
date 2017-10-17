namespace IIS.CodeReviewEngine.forms.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Script.Serialization;
    using System.Web.UI.WebControls;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.FunctionalLanguage;
    using ICSSoft.STORMNET.Web.Tools;
    using ICSSoft.STORMNET.Windows.Forms;

    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.WorkItemTracking.Client;

    /// <summary>
    /// Форма для отображения различной статистической информации по ревью.
    /// </summary>
    public partial class Statistics : System.Web.UI.Page
    {
        /// <summary>
        /// Вспомогательный класс для сортировки статистики.
        /// </summary>
        private class ProgrammerStatistic
        {
            /// <summary>
            /// Имя программиста.
            /// </summary>
            public string ProgrammerName;

            /// <summary>
            /// Количество чекинов, соответствующее программисту.
            /// </summary>
            public int CheckInCount;
        }

        /// <summary>
        /// Наименование параметра для передачи идентификатора команды на форму.
        /// </summary>
        protected const string TeamPkParameterName = "PK";

        /// <summary>
        /// Количество невыполненных запросов на Code Review (программист - количество).
        /// </summary>
        private readonly Dictionary<string, int> _countNotCompletedResponse;

        /// <summary>
        /// Количество запросов на Code Review находящихся в работе (программист - количество).
        /// </summary>
        private readonly Dictionary<string, int> _countResponseInProgress;

        /// <summary>
        /// Количество необработанных ответов на Code Review (программист - количество).
        /// </summary>
        private readonly Dictionary<string, int> _countNotCompletedRequest;

        /// <summary>
        /// Количество обрабатываемых ответов на Code Review (программист - количество).
        /// </summary>
        private readonly Dictionary<string, int> _countRequestInProgress;

        /// <summary>
        /// Количество обработанных ответов на Code Review (программист - количество).
        /// </summary>
        private readonly Dictionary<string, int> _countCompletedRequest;

        /// <summary>
        /// Конструктор класса.                
        /// </summary>
        public Statistics()
        {
            _countNotCompletedResponse = new Dictionary<string, int>();
            _countResponseInProgress = new Dictionary<string, int>();
            _countNotCompletedRequest = new Dictionary<string, int>();
            _countRequestInProgress = new Dictionary<string, int>();
            _countCompletedRequest = new Dictionary<string, int>();
        }

        /// <summary>
        /// Пересчитать данные о выполненных Codereview на команду. 
        /// </summary>
        /// <param name="teamGuid">Идентификатор команды.</param>
        public void LoadStatistic(Guid teamGuid)
        {
            var team = new Team();
            team.SetExistObjectPrimaryKey(teamGuid);
            DataServiceProvider.DataService.LoadObject(Team.Views.TeamE, team);
            
            List<TFSProgrammer> programmers = GetProgrammersForTeam(team);

            if (programmers.Count == 0)
                return;
            
            foreach (TFSProjectForReview project in team.Projects)
            {
                string projectName = project.TFSProject.GetProjectName();
                if (projectName.Contains('/'))
                {
                    projectName = projectName.Split('/')[0];
                }

                var projectUri = new Uri(project.TFSProject.GetProjectCollectionUrl());
                var projectCollection = new TfsTeamProjectCollection(projectUri);
                projectCollection.EnsureAuthenticated();
                var workitemStore = projectCollection.GetService<WorkItemStore>();

                if (workitemStore == null)
                    throw new InvalidOperationException(string.Format("У авторизованного пользователя недостаточно полномочий для получения списка рабочих элементов проекта '{0}'.", projectName));

                foreach (TFSProgrammer programmer in programmers)
                {
                    int countNotCompletedResponse = GetCountSimple(
                        workitemStore,
                        projectName,
                        TFSCommonCollectionContsntans.ResponseWorkItemType,
                        programmer.Name,
                        "Запрашивается");
                    if (countNotCompletedResponse > 0) 
                        AddNumberToDictionary(_countNotCompletedResponse, programmer.Name, countNotCompletedResponse);

                    int countResponseInProgress = GetCountSimple(
                        workitemStore,
                        projectName,
                        TFSCommonCollectionContsntans.ResponseWorkItemType,
                        programmer.Name,
                        "Принято");
                    if (countResponseInProgress > 0) 
                        AddNumberToDictionary(_countResponseInProgress, programmer.Name, countResponseInProgress);

                    int countNotCompletedRequest = GetCountComplex(
                        workitemStore,
                        projectName,
                        programmer.Name,
                        "Закрыто");
                    if (countNotCompletedRequest > 0)
                        AddNumberToDictionary(_countNotCompletedRequest, programmer.Name, countNotCompletedRequest);

                    int countRequestInProgress = GetCountComplex(
                        workitemStore, 
                        projectName, 
                        programmer.Name, 
                        "Принято");
                    if (countRequestInProgress > 0) 
                        AddNumberToDictionary(_countRequestInProgress, programmer.Name, countRequestInProgress);

                    int countCompletedRequest = GetCountSimple(
                        workitemStore,
                        projectName,
                        TFSCommonCollectionContsntans.RequestWorkItemType,
                        programmer.Name,
                        "Закрыто");
                    AddNumberToDictionary(_countCompletedRequest, programmer.Name, countCompletedRequest);
                }
            }
        }

        /// <summary>
        /// Получить данные для отображения на диаграммах.
        /// </summary>
        /// <returns>Данные для отображения на диаграммах.</returns>
        protected string GetData()
        {
            List<ProgrammerStatistic> countNotCompletedResponseList = GetOrderedStatistic(_countNotCompletedResponse);
            List<ProgrammerStatistic> countResponseInProgressList = GetOrderedStatistic(_countResponseInProgress);
            List<ProgrammerStatistic> countNotCompletedRequestList = GetOrderedStatistic(_countNotCompletedRequest);
            List<ProgrammerStatistic> countRequestInProgressList = GetOrderedStatistic(_countRequestInProgress);
            List<ProgrammerStatistic> countCompletedRequestList = GetOrderedStatistic(_countCompletedRequest);

            return new JavaScriptSerializer().Serialize(new
                {
                    namesNotCompletedResponse = countNotCompletedResponseList.Select(x => x.ProgrammerName).ToList(),
                    numbersNotCompletedResponse = countNotCompletedResponseList.Select(x => x.CheckInCount).ToList(),

                    namesResponseInProgress = countResponseInProgressList.Select(x => x.ProgrammerName).ToList(),
                    numbersResponseInProgress = countResponseInProgressList.Select(x => x.CheckInCount).ToList(),

                    namesNotCompletedRequest = countNotCompletedRequestList.Select(x => x.ProgrammerName).ToList(),
                    numbersNotCompletedRequest = countNotCompletedRequestList.Select(x => x.CheckInCount).ToList(),

                    namesRequestInProgress = countRequestInProgressList.Select(x => x.ProgrammerName).ToList(),
                    numbersRequestInProgress = countRequestInProgressList.Select(x => x.CheckInCount).ToList(),

                    namesCompletedRequest = countCompletedRequestList.Select(x => x.ProgrammerName).ToList(),
                    numbersCompletedRequest = countCompletedRequestList.Select(x => x.CheckInCount).ToList(),
                });
        }

        /// <summary>
        /// Обработчик события загрузки формы.
        /// Устаналивает дополнительные статистические данные в контролы.
        /// </summary>
        /// <param name="e">Объект, содержащий параметры события.</param>
        protected override void OnLoad(EventArgs e)
        {
            var teamGuid = UrlHelper.GetGuidParam(TeamPkParameterName);

            if (teamGuid == null)
            {
                var teams = DataServiceProvider.DataService.LoadObjects(Team.Views.TeamE);
                ctrlTeam.Items.Add(new ListItem(string.Empty));

                foreach (Team team in teams)
                {
                    ctrlTeam.Items.Add(new ListItem(team.Name, team.__PrimaryKey.ToString()));
                }

                return;
            }

            ctrlTeam.Visible = false;

            LoadStatistic(teamGuid.Value);
            
            base.OnLoad(e);
        }

        /// <summary>
        /// Получить данных по количеству чекинов у программистов в отсортированном виде: сначала идут программисты с максимальным количеством чекинов, потом по убывающей.
        /// </summary>
        /// <param name="statisticDictionary">Текущая статистика, хранящаяся в виде словаря.</param>
        /// <returns>Отсортированная по убыванию количества чекинов статистика.</returns>
        private List<ProgrammerStatistic> GetOrderedStatistic(Dictionary<string, int> statisticDictionary)
        {
            return statisticDictionary
                    .Select(x => new ProgrammerStatistic() { CheckInCount = x.Value, ProgrammerName = x.Key })
                    .OrderByDescending(x => x.CheckInCount).ToList();
        }
        
        /// <summary>
        /// Добавить число в коллекцию.
        /// </summary>
        /// <param name="dictionary">Коллекция в которую будет добавлять число.</param>
        /// <param name="key">Ключ по которому будет добавлено число.</param>
        /// <param name="value">Число, которое необходимо добавить.</param>
        private void AddNumberToDictionary(Dictionary<string, int> dictionary, string key, int value)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] += value;
            else
                dictionary.Add(key, value);
        }

        /// <summary>
        /// Получить всех программистов для конкретной команды.
        /// </summary>
        /// <param name="team">Команда для которой необходимо получить всех программистов.</param>
        /// <returns>Все программисты для конкретной команды.</returns>
        private List<TFSProgrammer> GetProgrammersForTeam(Team team)
        {
            LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(TFSProgrammer), TFSProgrammer.Views.TFSProgrammerE);
            ExternalLangDef langDef = ExternalLangDef.LanguageDef;
            string agregatorName = Information.ExtractPropertyName<TeamForProgrammer>(n => n.TFSProgrammer);
            lcs.LimitFunction = langDef.GetFunction(
                langDef.funcExist,
                new DetailVariableDef(langDef.DetailsType, agregatorName, TeamForProgrammer.Views.TeamForProgrammerE, agregatorName),
                langDef.GetFunction(
                    langDef.funcEQ,
                    new VariableDef(langDef.GuidType, Information.ExtractPropertyName<TeamForProgrammer>(n => n.Team)),
                    team.__PrimaryKey));
            
            return DataServiceProvider.DataService.LoadObjects(lcs).Cast<TFSProgrammer>().ToList();
        }

        /// <summary>
        /// Получить количество рабочих элементов удовлетворяющих условиям (без дочерних элементов).
        /// </summary>
        /// <param name="wis">Вспомагательный элемент для работы с рабочими элементами.</param>
        /// <param name="project">Наименование проекта в TFS.</param>
        /// <param name="wit">Тип рабочего элемента.</param>
        /// <param name="programmer">Человек, которому была назначена задача.</param>
        /// <param name="state">Состояние рабочего элемента.</param>
        /// <returns>Количество рабочих элементов.</returns>
        private int GetCountSimple(WorkItemStore wis, string project, string wit,  string programmer, string state)
        {
            return wis.QueryCount(
                string.Format(
                    @"SELECT [System.Id] 
                    FROM WorkItems 
                    WHERE [System.TeamProject] = '{0}' 
	                    AND [System.WorkItemType] = '{1}' 
	                    AND [{2}] = '{3}' 
	                    AND [System.State] = '{4}' 
                    ORDER BY [System.Id]", 
                    project, 
                    wit,
                    wit == TFSCommonCollectionContsntans.ResponseWorkItemType ? "System.AssignedTo" : "Microsoft.VSTS.Common.ClosedBy",
                    programmer, 
                    state));
        }

        /// <summary>
        /// Получить количество рабочих элементов удовлетворяющих условиям (с дочерними элементами).
        /// </summary>
        /// <param name="wis">Вспомагательный элемент для работы с рабочими элементами.</param>
        /// <param name="project">Наименование проекта в TFS.</param>
        /// <param name="programmer">Человек, которому была назначена задача.</param>
        /// <param name="state">Состояние рабочего элемента.</param>
        /// <returns>Количество рабочих элементов.</returns>
        private int GetCountComplex(WorkItemStore wis, string project, string programmer, string state)
        {
            return wis.QueryCount(
                string.Format(
                    @"SELECT [System.Id] 
                    FROM WorkItemLinks 
                    WHERE ([Source].[System.TeamProject] = '{0}' 
		                    AND [Source].[System.WorkItemType] IN GROUP 'Категория запроса проверки кода' 
		                    AND [Source].[System.State] <> 'Закрыто' 
		                    AND [Source].[System.AssignedTo] = '{1}') 
	                    And ([System.Links.LinkType] = 'System.LinkTypes.Hierarchy-Forward') 
	                    And ([Target].[System.WorkItemType] IN GROUP 'Категория ответа на проверку кода' 
		                    AND [Target].[System.State] = '{2}') 
                    ORDER BY [System.Id] mode(MustContain)",
                    project,
                    programmer,
                    state)) / 2;
        }
    }
}