namespace ICSSoft.STORMNET.Web
{
    using System;
    using System.Web.UI;

    /// <summary>
    /// Страница по-умолчанию
    /// </summary>
    public partial class Default : Page
    {
        #region Methods

        /// <summary>
        /// The page_ load.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The e. </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // IDataService ds = new MSSQLDataService();
            ////ds.CustomizationString = "SERVER=focus2;DATABASE=King;Uid=someuser;Pwd=somepass;";
            // ds.CustomizationString = "SERVER=space;Trusted_connection=no;DATABASE=MOB_test2;USER ID=webuser;Password=1;";
            // DataServiceProvider.DataService = ds;
        }

        #endregion
    }
}