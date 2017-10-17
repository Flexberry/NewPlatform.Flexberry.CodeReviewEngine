﻿using ICSSoft.STORMNET.FunctionalLanguage;
using ICSSoft.STORMNET.FunctionalLanguage.SQLWhere;
using ICSSoft.STORMNET.Security;

namespace IIS.Product_19312
{
    using System;
    using System.Web.UI;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Web;

    public partial class ClassL : Page
    {
        /// <summary>
        /// Обычный Page_Load ASP.NET
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event Args</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SQLWhereLanguageDef languageDef = SQLWhereLanguageDef.LanguageDef;
            Function limit = languageDef.GetFunction(languageDef.funcEQ, new VariableDef(languageDef.BoolType, "IsClass"), true);

            WebObjectListView1.LimitFunction = limit;
            WebObjectListView1.View = Information.GetView("Sec_SubjectL", typeof(Subject));
            WebObjectListView1.EditPage = "~//forms//Security//Class//ClassE.aspx";

            WOLVSettApplyer wsa = new WOLVSettApplyer();
            wsa.SettingsApply(WebObjectListView1);
        }
    }
}
