using ICSSoft.STORMNET.FunctionalLanguage;
using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
using ICSSoft.STORMNET.Security;

namespace IIS.Product_19312
{
    using System;
    using System.Web.UI;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Web;

    public partial class UserL : Page
    {
        /// <summary>
        /// Обычный Page_Load ASP.NET
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event Args</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SQLWhereLanguageDef languageDef = SQLWhereLanguageDef.LanguageDef;
            Function limit = languageDef.GetFunction(languageDef.funcEQ, new VariableDef(languageDef.BoolType, "IsUser"), true);

            WebObjectListView1.LimitFunction = limit;
            WebObjectListView1.View = Information.GetView("Sec_AgentL", typeof(Agent));
            WebObjectListView1.EditPage = "~//forms//Security//User//UserE.aspx";

            WOLVSettApplyer wsa = new WOLVSettApplyer();
            wsa.SettingsApply(WebObjectListView1);
        }
    }
}
