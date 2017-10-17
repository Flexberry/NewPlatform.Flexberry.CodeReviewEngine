using CheckingLibrary;

namespace WebApplication
{
    using System;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Web.Tools;

    public partial class Site1 : MasterPage
    {
        #region PageLayout enum

        public enum PageLayout
        {
            [Caption("M")] MainColumn,

            [Caption("LM")] LeftAndMainColumns,

            [Caption("LMR")] LeftMainAndRightColumns,

            [Caption("MR")] MainAndRightColumns
        }

        #endregion

        private PageLayout _bodyID = PageLayout.LeftAndMainColumns;

        public PageLayout BodyID
        {
            get { return _bodyID; }
            set { _bodyID = value; }
        }

        public string GetBodyID()
        {
            string bodyId = EnumCaption.GetCaptionFor(BodyID);
            return bodyId;
        }

        /// <summary>
        /// Чтение Cookies для TreeView.
        /// </summary>
        private void ApplyTreeViewCookie()
        {
            // чтение Cookies для TreeView
            var cookie = HttpContext.Current.Request.Cookies["treeView"];
            if (cookie == null || cookie.Value == "1")
            {
                // Показать TreeView
                pageForm.Attributes["class"] = "page-form treeview-visible";
                treeviewHideSpan.Attributes["class"] = String.Empty;
                treeviewShowSpan.Attributes["class"] = "Hide";
            }
            else
            {
                // Скрыть TreeView
                pageForm.Attributes["class"] = "page-form treeview-hidden";
                treeviewHideSpan.Attributes["class"] = "Hide";
                treeviewShowSpan.Attributes["class"] = String.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string lookUpQueryString = Request["LookUp"];
            bool openAsLookUp = !String.IsNullOrEmpty(lookUpQueryString) && lookUpQueryString.ToLower().Equals("true");

            if (openAsLookUp)
            {
                BodyID = PageLayout.MainColumn;
            }

            ApplyTreeViewCookie();
            if (AuthenticationAdapter.GetDbUser(Context.User.Identity.Name) == null)
                AuthenticationAdapter.CreateDbUser(Context.User.Identity.Name);

            fio.Text = Context.User.Identity.Name;
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            var metatag = new HtmlMeta();
            metatag.Attributes.Add("http-equiv", "X-UA-Compatible");
            metatag.Attributes.Add("content", "IE=edge");
            Page.Header.Controls.AddAt(0, metatag);
        }

        protected void ExitButton_Click(object sender, EventArgs e)
        {
            new WebLockHelper().ClearWebLock(Context.User.Identity.Name);
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}