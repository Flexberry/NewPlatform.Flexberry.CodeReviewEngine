namespace IIS.CodeReviewEngine
{
    /// <summary>
    /// Константы используемые в TFS-шаблонах коллекции TFS-проектов Common.
    /// </summary>
    public class TFSCommonCollectionContsntans
    {
        /// <summary>
        /// Тип WorkItem'а для получения ответа о проверке кода. Значение константы совпадает с WORKITEMTYPE.name из TFS-шаблона.
        /// </summary>
        public const string ResponseWorkItemType = "Ответ на проверку кода";

        /// <summary>
        /// Тип WorkItem'а для запроса проверки кода. Значение константы совпадает с WORKITEMTYPE.name из TFS-шаблона.
        /// </summary>
        public const string RequestWorkItemType = "Запрос на проверку кода";

        /// <summary>
        /// Состояние вновь созданного WorkItem'а. Значение константы совпадает с первым из статусов в списке STATES из TFS-шаблона.
        /// </summary>
        public const string StateWorkItemCreation = "Запрашивается";

        /// <summary>
        /// Состояние закрытого WorkItem'а. Значение константы совпадает со вторым статусом в списке STATES из TFS-шаблона.
        /// </summary>
        public const string StateWorkItemClosed = "Закрыто";
    }
}
