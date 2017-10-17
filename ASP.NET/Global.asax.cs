namespace ICSSoft.STORMNET.Web
{
    using System;
    using System.Web;
    using System.Web.Configuration;

    using ICSSoft.STORMNET.Business.Audit;
    using ICSSoft.STORMNET.Web.AjaxControls;
    using ICSSoft.STORMNET.Web.Tools;
    using ICSSoft.STORMNET.Web.Tools.Monads;

    using Resources;

    public class Global : HttpApplication
    {
        // Немного ускорим приложение за счёт отказа от обращения к конфигу каждый раз
        private bool? bNoCache;

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (bNoCache == null)
            {
                string noCache = WebConfigurationManager.AppSettings["NoCache"];

                bNoCache = !string.IsNullOrEmpty(noCache) && noCache.ToLower() == "true";
            }
            if (bNoCache.Value)
            {
                CacheHelper.ClearCache();
            }
        }

        void Application_EndRequest(object sender, EventArgs e)
        {
            /*
            if (Response.StatusCode == 401)
            {
                Response.Redirect("~/LoginForm.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.RawUrl));
            }
             */
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            ServiceInit();
            BaseMasterEditorLookUp.ChangeLookUpSettings = FormUtils.ChangeLookUpSettings;
        }

        private void ServiceInit()
        {
            AuditSetter.InitAuditService(BridgeToDS.GetDataService()); // Инициализация сервиса аудита
        }

        protected void Application_End(object sender, EventArgs e)
        {
            // Для того, чтобы все объекты, которые сейчас в кэше, но должны обновиться в базе обновились:
            CacheHelper.RemoveFromCache(string.Empty);
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["dummy"] = string.Empty;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            string noHideError = WebConfigurationManager.AppSettings["NoHideError"];

            if (string.IsNullOrEmpty(noHideError) || noHideError.ToLower() != "true")
            {
                try
                {
                    // Ловим последнее возникшее исключение 
                    Exception lastError = Server.GetLastError();

                    if (lastError != null)
                    {
                        // Записываем непосредственно исключение, вызвавшее данное, в 
                        HttpContext.Current.Items[WebParamController.STR_ErrorException] = lastError;

                        string strErr = "App_error";
                        if (HttpContext.Current != null)
                            if (HttpContext.Current.User != null)
                                if (HttpContext.Current.User.Identity != null)
                                    if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                                        strErr += " User:" + HttpContext.Current.User.Identity.Name;

                        LogService.Log.Error(strErr, lastError);
                    }

                    // Обнуление ошибки на сервере 
                    Server.ClearError();

                    // Перенаправление на свою страницу отображения ошибки
                    if ((lastError as HttpException).Return(x => x.GetHttpCode() == 404, false))
                    {
                        Server.Transfer("~/Error404.aspx");
                    }
                    else
                    {
                        Server.Transfer("~/ErrorForm.aspx");
                    }
                }
                catch (Exception)
                {
                    // если мы всёже приходим сюда - значит обработка исключения 
                    // сама сгенерировала исключение, мы ничего не делаем, чтобы 
                    // не создать бесконечный цикл 
                    Response.Write(Resource.Crirical_Error);
                }
            }
        }
    }
}