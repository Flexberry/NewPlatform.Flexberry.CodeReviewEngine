namespace ICSSoft.STORMNET.Web.Controls
{
    /// <summary>
    /// Класс прикладной формы лукапов.
    /// </summary>
    public class LookUpForm : AjaxControls.Forms.LookUpForm
    {
        /// <summary>
        /// Применение настроек к wolv лежащему на форме лукапа.
        /// Переопределенный метод реализует прикладную логику донастройки WOLV на форме.
        /// </summary>
        protected override void ApplyWolvSettings()
        {
            new WOLVSettApplyer().SettingsApply(LookUpFormWOLV);

            base.ApplyWolvSettings();
        }
    }
}