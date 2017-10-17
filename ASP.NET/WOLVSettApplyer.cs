namespace ICSSoft.STORMNET.Web
{
    /// <summary>
    /// Тюнер для WOLV'а доступный для прикладных программистов.
    /// </summary>
    public class WOLVSettApplyer : AjaxControls.WolvSettApplyer
    {
        /// <summary>
        /// Применение настроек внешнего вида для WOLV'а с прикладной логикой.
        /// </summary>
        /// <param name="wolv">WOLV который необходимо настроить.</param>
        public override void SettingsApply(AjaxControls.WebObjectListView wolv)
        {
            base.SettingsApply(wolv);
        }
    }
}