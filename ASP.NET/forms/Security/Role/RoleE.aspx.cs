using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ICSSoft.STORMNET.Business;
using ICSSoft.STORMNET.FunctionalLanguage;
using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
using ICSSoft.STORMNET.Security;
using ICSSoft.STORMNET.Web.Tools;

namespace IIS.Product_19312
{
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Web.Controls;

    public partial class RoleE : BaseEditForm<Agent>
    {

        private SQLWhereLanguageDef _languageDef;
        private List<Agent> _allUsers;
        private List<Agent> _allRoles;
        private List<Subject> _allClasses;

        /// <summary>
        /// Конструктор формы
        /// </summary>
        public RoleE() : base("Sec_AgentL")
        {
            _languageDef = SQLWhereLanguageDef.LanguageDef;


            Function limitIsUser = _languageDef.GetFunction(_languageDef.funcEQ, new VariableDef(_languageDef.BoolType, "IsUser"), true);
            LoadingCustomizationStruct lcsAllUsers = new LoadingCustomizationStruct(null);
            lcsAllUsers.LoadingTypes = new[] { typeof(Agent) };
            lcsAllUsers.LimitFunction = limitIsUser;
            lcsAllUsers.View = Information.GetView("Sec_AgentL", typeof(Agent));
            _allUsers =
                BridgeToDS.GetDataService().LoadObjects(lcsAllUsers).Cast<Agent>().ToList();


            Function limitIsRole = _languageDef.GetFunction(_languageDef.funcEQ, new VariableDef(_languageDef.BoolType, "IsRole"), true);
            LoadingCustomizationStruct lcsAllRoles = new LoadingCustomizationStruct(null);
            lcsAllRoles.LoadingTypes = new[] { typeof(Agent) };
            lcsAllRoles.LimitFunction = limitIsRole;
            lcsAllRoles.View = Information.GetView("Sec_RolesL", typeof(Agent));
            _allRoles =
                BridgeToDS.GetDataService().LoadObjects(lcsAllRoles).Cast<Agent>().ToList();


            Function limitIsClass = _languageDef.GetFunction(_languageDef.funcEQ, new VariableDef(_languageDef.BoolType, "IsClass"), true);
            LoadingCustomizationStruct lcsAllClasses = new LoadingCustomizationStruct(null);
            lcsAllClasses.LoadingTypes = new[] { typeof(Subject) };
            lcsAllClasses.LimitFunction = limitIsClass;
            lcsAllClasses.View = Information.GetView("Sec_SubjectL", typeof(Subject));
            _allClasses =
                BridgeToDS.GetDataService().LoadObjects(lcsAllClasses).Cast<Subject>().ToList();
        }

        /// <summary>
        /// Вызывается самым первым в Page_Load
        /// </summary>
        protected override void Preload()
        {
        }

        /// <summary>
        /// Здесь лучше всего писать бизнес-логику, оперируя только объектом данных
        /// </summary>
        protected override void PreApplyToControls()
        {
        }

        /// <summary>
        /// Здесь лучше всего изменять свойства контролов на странице, которые не обрабатываются WebBinder
        /// </summary>
        protected override void PostApplyToControls()
        {
            // Нужна проверка блокировки формы
            bool readOnly = false;

            if (readOnly)
            {
                /*
                SaveAndCloseBtn.Visible = false;
                SaveBtn.Visible = false;
                wb.SetReadOnlyForm(this.Controls, _view, true);
                 */
            }

            string backURL = Request["ReturnUrl"];
            if (string.IsNullOrEmpty(backURL))
            {
                SaveAndCloseBtn.Visible = false;
                CancelBtn.Visible = false;
            }

            Page.Validate();
        }

        /// <summary>
        /// Вызывается самым последним в Page_Load
        /// </summary>
        protected override void Postload()
        {
            if (PK == null)
                return;


            Function limitByRole = _languageDef.GetFunction(_languageDef.funcEQ, new VariableDef(_languageDef.GuidType, "Role"), this.PK);

            LoadingCustomizationStruct lcsOwnUsers = new LoadingCustomizationStruct(null);
            lcsOwnUsers.LoadingTypes = new[] { typeof(LinkRole) };
            lcsOwnUsers.LimitFunction = limitByRole;
            View usersView = Information.GetView("Sec_LinkRoleL", typeof(LinkRole));
            lcsOwnUsers.View = usersView;

            List<LinkRole> roleOwnUsers =
                BridgeToDS.GetDataService().LoadObjects(lcsOwnUsers).Cast<LinkRole>().ToList();

            if (IsPostBack)
            {
                #region Save Roles

                List<LinkRole> linkUsers = new List<LinkRole>();

                foreach (Agent user in _allUsers)
                {
                    string userKey = "User" + user.__PrimaryKey;

                    if (Request.Form.AllKeys.Any(fk => fk.Contains(userKey)))
                    {
                        LinkRole linkUser = new LinkRole();
                        linkUser.Role = DataObject;
                        linkUser.Agent = user;

                        linkUsers.Add(linkUser);
                    }
                }

                List<LinkRole> deletedUsers = new List<LinkRole>();

                foreach (LinkRole ownUser in roleOwnUsers)
                {
                    string ownId = ownUser.Agent.__PrimaryKey.ToString();

                    LinkRole singleOrDefault =
                        linkUsers.SingleOrDefault(r => r.Agent.__PrimaryKey.ToString().Equals(ownId));

                    if (singleOrDefault != null)
                    {
                        linkUsers.Remove(singleOrDefault);
                    }
                    else
                    {
                        ownUser.SetStatus(ObjectStatus.Deleted);
                        deletedUsers.Add(ownUser);
                    }
                }

                roleOwnUsers = roleOwnUsers.Except(deletedUsers).Union(linkUsers).ToList();
                linkUsers = linkUsers.Union(deletedUsers).ToList();

                if (linkUsers.Count > 0)
                {
                    DataObject[] dataObjects = linkUsers.Cast<DataObject>().ToArray();
                    BridgeToDS.GetDataService().UpdateObjects(ref dataObjects);
                }

                #endregion
            }

            GenerateUsersTable(roleOwnUsers);




            Function limitByAgent = _languageDef.GetFunction(_languageDef.funcEQ, new VariableDef(_languageDef.GuidType, "Agent"), this.PK);

            LoadingCustomizationStruct lcsParentRoles = new LoadingCustomizationStruct(null);
            lcsParentRoles.LoadingTypes = new[] { typeof(LinkRole) };
            lcsParentRoles.LimitFunction = limitByAgent;
            View rolesView = Information.GetView("Sec_LinkRoleL", typeof(LinkRole));
            lcsParentRoles.View = rolesView;

            List<LinkRole> parentRoles =
                BridgeToDS.GetDataService().LoadObjects(lcsParentRoles).Cast<LinkRole>().ToList();

            Agent single = _allRoles.Single(r => r.__PrimaryKey.ToString().Equals(this.PK));
            _allRoles.Remove(single);

            if (IsPostBack)
            {
                #region Save Roles

                List<LinkRole> linkRoles = new List<LinkRole>();

                foreach (Agent role in _allRoles)
                {
                    string roleKey = "Role" + role.__PrimaryKey;

                    if (Request.Form.AllKeys.Any(fk => fk.Contains(roleKey)))
                    {
                        LinkRole linkRole = new LinkRole();
                        linkRole.Agent = DataObject;
                        linkRole.Role = role;

                        linkRoles.Add(linkRole);
                    }
                }

                List<LinkRole> deletedRoles = new List<LinkRole>();

                foreach (LinkRole ownRole in parentRoles)
                {
                    string ownId = ownRole.Role.__PrimaryKey.ToString();

                    LinkRole singleOrDefault =
                        linkRoles.SingleOrDefault(r => r.Role.__PrimaryKey.ToString().Equals(ownId));

                    if (singleOrDefault != null)
                    {
                        linkRoles.Remove(singleOrDefault);
                    }
                    else
                    {
                        ownRole.SetStatus(ObjectStatus.Deleted);
                        deletedRoles.Add(ownRole);
                    }
                }

                parentRoles = parentRoles.Except(deletedRoles).Union(linkRoles).ToList();
                linkRoles = linkRoles.Union(deletedRoles).ToList();

                if (linkRoles.Count > 0)
                {
                    DataObject[] dataObjects = linkRoles.Cast<DataObject>().ToArray();
                    BridgeToDS.GetDataService().UpdateObjects(ref dataObjects);
                }

                #endregion
            }

            GenerateRolesTable(parentRoles);




//            Function limitByAgent = _languageDef.GetFunction(_languageDef.funcEQ, new VariableDef(_languageDef.GuidType, "Agent"), this.PK);

            LoadingCustomizationStruct lcsOwnClasses = new LoadingCustomizationStruct(null);
            lcsOwnClasses.LoadingTypes = new[] { typeof(Permition) };
            lcsOwnClasses.LimitFunction = limitByAgent;
            View classesView = Information.GetView("Sec_PermitionE", typeof(Permition));
            lcsOwnClasses.View = classesView;

            List<Permition> userOwnClasses =
                BridgeToDS.GetDataService().LoadObjects(lcsOwnClasses).Cast<Permition>().ToList();

            List<Permissions> permissions = new List<Permissions>();

            foreach (Subject @class in _allClasses)
            {
                Permissions u2c = new Permissions();

                u2c.ObjectId = @class.__PrimaryKey.ToString();
                u2c.ObjectName = @class.Name;

                Permition firstOrDefault =
                    userOwnClasses.FirstOrDefault(c => c.Subject.__PrimaryKey.ToString().Equals(u2c.ObjectId));

                if (firstOrDefault != null && firstOrDefault.Access != null)
                {
                    for (int i = 0; i < firstOrDefault.Access.Count; i++)
                    {
                        Access access = firstOrDefault.Access[i];

                        switch (access.TypeAccess)
                        {
                            case tTypeAccess.Delete:
                                u2c.Delete = true;
                                break;
                            case tTypeAccess.Execute:
                                u2c.Execute = true;
                                break;
                            case tTypeAccess.Full:
                                u2c.Full = true;
                                break;
                            case tTypeAccess.Insert:
                                u2c.Insert = true;
                                break;
                            case tTypeAccess.Read:
                                u2c.Read = true;
                                break;
                            case tTypeAccess.Update:
                                u2c.Update = true;
                                break;
                        }

                    }
                }

                permissions.Add(u2c);
            }

            if (IsPostBack)
            {
                #region Save Class Permissions

                List<Permissions> list = new List<Permissions>();

                foreach (Subject @class in _allClasses)
                {
                    string fullKey = "Full" + @class.__PrimaryKey;
                    string readKey = "Read" + @class.__PrimaryKey;
                    string insertKey = "Insert" + @class.__PrimaryKey;
                    string updateKey = "Update" + @class.__PrimaryKey;
                    string deleteKey = "Delete" + @class.__PrimaryKey;
                    string executeKey = "Execute" + @class.__PrimaryKey;

                    bool isClassPermissionsSet = false;

                    Permissions permission = new Permissions();
                    permission.ObjectId = @class.__PrimaryKey.ToString();
                    permission.ObjectName = @class.Name;

                    if (Request.Form.AllKeys.Any(fk => fk.Contains(fullKey)))
                    {
                        permission.Full = true;
                        isClassPermissionsSet = true;
                    }

                    if (Request.Form.AllKeys.Any(fk => fk.Contains(readKey)))
                    {
                        permission.Read = true;
                        isClassPermissionsSet = true;
                    }

                    if (Request.Form.AllKeys.Any(fk => fk.Contains(insertKey)))
                    {
                        permission.Insert = true;
                        isClassPermissionsSet = true;
                    }

                    if (Request.Form.AllKeys.Any(fk => fk.Contains(updateKey)))
                    {
                        permission.Update = true;
                        isClassPermissionsSet = true;
                    }

                    if (Request.Form.AllKeys.Any(fk => fk.Contains(deleteKey)))
                    {
                        permission.Delete = true;
                        isClassPermissionsSet = true;
                    }

                    if (Request.Form.AllKeys.Any(fk => fk.Contains(executeKey)))
                    {
                        permission.Execute = true;
                        isClassPermissionsSet = true;
                    }

                    if (isClassPermissionsSet)
                        list.Add(permission);
                }



                List<Permissions> updatedPermissions = new List<Permissions>();
                List<Permissions> deletedPermissions = new List<Permissions>();

                foreach (Permissions p in permissions)
                {
                    Permissions singleOrDefault = list.SingleOrDefault(c => c.ObjectId.Equals(p.ObjectId));

                    if (singleOrDefault != null)
                    {
                        updatedPermissions.Add(singleOrDefault);
                        list.Remove(singleOrDefault);
                    }
                    else
                    {
                        deletedPermissions.Add(p);
                    }
                }

                List<Permissions> addedPermissions = list.Union(updatedPermissions).ToList();



                List<DataObject> deletedObjects = new List<DataObject>();
                List<DataObject> addedObjects = new List<DataObject>();

                foreach (Permition p in userOwnClasses)
                {
                    string classId = p.Subject.__PrimaryKey.ToString();

                    if (updatedPermissions.Any(x => x.ObjectId.Equals(classId)) ||
                        deletedPermissions.Any(x => x.ObjectId.Equals(classId)))
                    {
                        p.SetStatus(ObjectStatus.Deleted);
                        deletedObjects.Add(p);
                    }
                }

                foreach (Permissions p in addedPermissions)
                {
                    Permition permission = new Permition();

                    permission.Agent = DataObject;
                    permission.Subject = new Subject();
                    permission.Subject.SetExistObjectPrimaryKey(p.ObjectId);

                    permission.Access = new DetailArrayOfAccess(permission);

                    if (p.Delete)
                        permission.Access.Add(new Access() { TypeAccess = tTypeAccess.Delete });

                    if (p.Execute)
                        permission.Access.Add(new Access() { TypeAccess = tTypeAccess.Execute });

                    if (p.Full)
                        permission.Access.Add(new Access() { TypeAccess = tTypeAccess.Full });

                    if (p.Insert)
                        permission.Access.Add(new Access() { TypeAccess = tTypeAccess.Insert });

                    if (p.Read)
                        permission.Access.Add(new Access() { TypeAccess = tTypeAccess.Read });

                    if (p.Update)
                        permission.Access.Add(new Access() { TypeAccess = tTypeAccess.Update });

                    addedObjects.Add(permission);
                }

                DataObject[] dataObjects = deletedObjects.Union(addedObjects).ToArray();

                if (dataObjects.Length > 0)
                {
                    BridgeToDS.GetDataService().UpdateObjects(ref dataObjects);
                }

                #endregion
            }

            GenerateClassesTable(permissions);
        }

        private void GenerateRolesTable(List<LinkRole> parentRoles)
        {
            TableHeaderRow tableHeaderRow = new TableHeaderRow();
            tableHeaderRow.CssClass = "ObjectListViewCaptionCell";

            TableCell headName = new TableCell();
            headName.CssClass = "ObjectListViewCaptionCell";
            TableCell headChecked = new TableCell();
            headChecked.CssClass = "ObjectListViewCaptionCell";

            headName.Text = "Наименование";
            headChecked.Text = "Наличие";

            tableHeaderRow.Cells.Add(headName);
            tableHeaderRow.Cells.Add(headChecked);

            int i = 0;
            tblParentRoles.Rows.Add(tableHeaderRow);

            foreach (Agent role in _allRoles)
            {
                TableRow tr = new TableRow();
                tr.CssClass = i++ % 2 == 0 ? "ObjectListViewCell" : "ObjectListViewAlternateCell";

                TableCell cellName = new TableCell();
                cellName.CssClass = "ObjectListViewCellValue";
                TableCell cellChecked = new TableCell();
                cellChecked.CssClass = "ObjectListViewCellValue";

                CheckBox checkBox = new CheckBox();
                checkBox.ID = "Role" + role.__PrimaryKey;

                foreach (LinkRole parentRole in parentRoles)
                {
                    if (parentRole.Role.__PrimaryKey.ToString().Equals(role.__PrimaryKey.ToString()))
                        checkBox.Checked = true;
                }

                cellName.Text = role.Name;
                cellChecked.Controls.Add(checkBox);

                tr.Cells.Add(cellName);
                tr.Cells.Add(cellChecked);

                tblParentRoles.Rows.Add(tr);
            }
        }

        private void GenerateClassesTable(List<Permissions> permissions)
        {
            TableHeaderRow tableHeaderRow = new TableHeaderRow();
            tableHeaderRow.CssClass = "ObjectListViewCaptionCell";

            TableCell headName = new TableCell();
            headName.CssClass = "ObjectListViewCaptionCell";
            TableCell headDelete = new TableCell();
            headDelete.CssClass = "ObjectListViewCaptionCell";
            TableCell headExecute = new TableCell();
            headExecute.CssClass = "ObjectListViewCaptionCell";
            TableCell headFull = new TableCell();
            headFull.CssClass = "ObjectListViewCaptionCell";
            TableCell headInsert = new TableCell();
            headInsert.CssClass = "ObjectListViewCaptionCell";
            TableCell headRead = new TableCell();
            headRead.CssClass = "ObjectListViewCaptionCell";
            TableCell headUpdate = new TableCell();
            headUpdate.CssClass = "ObjectListViewCaptionCell";

            headName.Text = "Наименование";
            headFull.Text = "Полный доступ";
            headRead.Text = "Чтение";
            headInsert.Text = "Вставка";
            headUpdate.Text = "Обновление";
            headDelete.Text = "Удаление";
            headExecute.Text = "Исполнение";

            tableHeaderRow.Cells.Add(headName);
            tableHeaderRow.Cells.Add(headFull);
            tableHeaderRow.Cells.Add(headRead);
            tableHeaderRow.Cells.Add(headInsert);
            tableHeaderRow.Cells.Add(headUpdate);
            tableHeaderRow.Cells.Add(headDelete);
            tableHeaderRow.Cells.Add(headExecute);

            int i = 0;
            tblRoleClasses.Rows.Add(tableHeaderRow);

            foreach (Permissions u2c in permissions)
            {
                TableRow tableRow = new TableRow();
                tableRow.CssClass = i++ % 2 == 0 ? "ObjectListViewCell" : "ObjectListViewAlternateCell";

                TableCell cellName = new TableCell();
                cellName.CssClass = "ObjectListViewCellValue";
                TableCell cellDelete = new TableCell();
                cellDelete.CssClass = "ObjectListViewCellValue";
                TableCell cellExecute = new TableCell();
                cellExecute.CssClass = "ObjectListViewCellValue";
                TableCell cellFull = new TableCell();
                cellFull.CssClass = "ObjectListViewCellValue";
                TableCell cellInsert = new TableCell();
                cellInsert.CssClass = "ObjectListViewCellValue";
                TableCell cellRead = new TableCell();
                cellRead.CssClass = "ObjectListViewCellValue";
                TableCell cellUpdate = new TableCell();
                cellUpdate.CssClass = "ObjectListViewCellValue";

                cellName.Text = u2c.ObjectName;

                CheckBox checkBoxFull = new CheckBox();
                checkBoxFull.ID = "Full" + u2c.ObjectId;
                checkBoxFull.Checked = u2c.Full;
                cellFull.Controls.Add(checkBoxFull);

                CheckBox checkBoxRead = new CheckBox();
                checkBoxRead.ID = "Read" + u2c.ObjectId;
                checkBoxRead.Checked = u2c.Read;
                cellRead.Controls.Add(checkBoxRead);

                CheckBox checkBoxInsert = new CheckBox();
                checkBoxInsert.ID = "Insert" + u2c.ObjectId;
                checkBoxInsert.Checked = u2c.Insert;
                cellInsert.Controls.Add(checkBoxInsert);

                CheckBox checkBoxUpdate = new CheckBox();
                checkBoxUpdate.ID = "Update" + u2c.ObjectId;
                checkBoxUpdate.Checked = u2c.Update;
                cellUpdate.Controls.Add(checkBoxUpdate);

                CheckBox checkBoxDelete = new CheckBox();
                checkBoxDelete.ID = "Delete" + u2c.ObjectId;
                checkBoxDelete.Checked = u2c.Delete;
                cellDelete.Controls.Add(checkBoxDelete);

                CheckBox checkBoxExecute = new CheckBox();
                checkBoxExecute.ID = "Execute" + u2c.ObjectId;
                checkBoxExecute.Checked = u2c.Execute;
                cellExecute.Controls.Add(checkBoxExecute);

                tableRow.Cells.Add(cellName);

                tableRow.Cells.Add(cellFull);
                tableRow.Cells.Add(cellRead);
                tableRow.Cells.Add(cellInsert);
                tableRow.Cells.Add(cellUpdate);
                tableRow.Cells.Add(cellDelete);
                tableRow.Cells.Add(cellExecute);

                tblRoleClasses.Rows.Add(tableRow);
            }
        }

        private void GenerateUsersTable(List<LinkRole> roleOwnUsers)
        {
            TableHeaderRow tableHeaderRow = new TableHeaderRow();
            tableHeaderRow.CssClass = "ObjectListViewCaptionCell";

            TableCell headName = new TableCell();
            headName.CssClass = "ObjectListViewCaptionCell";
            TableCell headChecked = new TableCell();
            headChecked.CssClass = "ObjectListViewCaptionCell";

            headName.Text = "Наименование";
            headChecked.Text = "Наличие";

            tableHeaderRow.Cells.Add(headName);
            tableHeaderRow.Cells.Add(headChecked);

            int i = 0;
            tblRoleUsers.Rows.Add(tableHeaderRow);

            foreach (Agent user in _allUsers)
            {
                TableRow tr = new TableRow();
                tr.CssClass = i++ % 2 == 0 ? "ObjectListViewCell" : "ObjectListViewAlternateCell";

                TableCell cellName = new TableCell();
                cellName.CssClass = "ObjectListViewCellValue";
                TableCell cellChecked = new TableCell();
                cellChecked.CssClass = "ObjectListViewCellValue";

                CheckBox checkBox = new CheckBox();
                checkBox.ID = "User" + user.__PrimaryKey;

                foreach (LinkRole ownUser in roleOwnUsers)
                {
                    if (ownUser.Agent.__PrimaryKey.ToString().Equals(user.__PrimaryKey.ToString()))
                        checkBox.Checked = true;
                }

                cellName.Text = user.Name;
                cellChecked.Controls.Add(checkBox);

                tr.Cells.Add(cellName);
                tr.Cells.Add(cellChecked);

                tblRoleUsers.Rows.Add(tr);
            }
        }

        /// <summary>
        /// Валидация объекта для сохранения
        /// </summary>
        /// <returns>true - продолжать сохранение, иначе - прекратить</returns>
        protected override bool PreSaveObject()
        {
            DataObject.IsRole = true;

            return base.PreSaveObject();
        }

        /// <summary>
        /// Нетривиальная логика сохранения объекта
        /// </summary>
        /// <returns>Объект данных, который сохранился</returns>
        protected override DataObject SaveObject()
        {
            RightManager.ClearCache();
            return base.SaveObject();
        }
    }
}