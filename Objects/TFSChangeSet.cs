//------------------------------------------------------------------------------
// <auto-generated>
//     ���� ��� ������ ����������.
//     ����������� ������:4.0.30319.18010
//
//     ��������� � ���� ����� ����� �������� � ������������ ������ � ����� �������� � ������
//     ��������� ��������� ����.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IIS.CodeReviewEngine
{
    using System;
    using System.Xml;
    using ICSSoft.STORMNET;
    
    
    // *** Start programmer edit section *** (Using statements)

    // *** End programmer edit section *** (Using statements)


    /// <summary>
    /// ����� ��������� �� TFS.
    /// </summary>
    // *** Start programmer edit section *** (TFSChangeSet CustomAttributes)

    // *** End programmer edit section *** (TFSChangeSet CustomAttributes)
    [AutoAltered()]
    [Caption("����� ��������� �� TFS")]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    [View("ChangeSetE", new string[] {
            "Number as \'Number\'",
            "Reviewed",
            "Comment as \'Comment\'"})]
    [View("ChangeSetL", new string[] {
            "Number as \'Number\'",
            "Reviewed as \'Reviewed\'",
            "Comment as \'Comment\'"})]
    [View("TFSChangeSetE", new string[] {
            "Number as \'Number\'",
            "Reviewed as \'Reviewed\'",
            "Comment as \'Comment\'",
            "Date as \'Date\'",
            "ReviewerNote as \'Reviewer note\'",
            "Notify as \'Notify\'",
            "Accept as \'Accept\'",
            "ChangesetAuthor as \'Changeset author\'",
            "ChangesetAuthor.Name as \'Name\'",
            "Reviewer as \'Reviewer\'",
            "Reviewer.Name as \'Name\'",
            "TeamSlice as \'Team slice\'",
            "TeamSlice.StartDate as \'Start date\'"})]
    [View("TFSChangeSetL", new string[] {
            "Number as \'Number\'",
            "Reviewed as \'Reviewed\'",
            "Comment as \'Comment\'",
            "Date as \'Date\'",
            "ReviewerNote as \'Reviewer note\'",
            "Notify as \'Notify\'",
            "Accept as \'Accept\'",
            "ChangesetAuthor.Name as \'Name\'",
            "Reviewer.Name as \'Name\'",
            "TeamSlice.StartDate as \'Start date\'"})]
    public class TFSChangeSet : ICSSoft.STORMNET.DataObject
    {
        
        private int fNumber;
        
        private bool fReviewed = false;
        
        private string fComment;
        
        private System.DateTime fDate;
        
        private string fReviewerNote;
        
        private bool fNotify = false;
        
        private bool fAccept = false;
        
        private IIS.CodeReviewEngine.TFSProgrammer fChangesetAuthor;
        
        private IIS.CodeReviewEngine.TFSProgrammer fReviewer;
        
        private IIS.CodeReviewEngine.TeamSlice fTeamSlice;
        
        // *** Start programmer edit section *** (TFSChangeSet CustomMembers)

        // *** End programmer edit section *** (TFSChangeSet CustomMembers)

        
        /// <summary>
        /// �����
        /// </summary>
        // *** Start programmer edit section *** (TFSChangeSet.Number CustomAttributes)

        // *** End programmer edit section *** (TFSChangeSet.Number CustomAttributes)
        [NotNull()]
        public virtual int Number
        {
            get
            {
                // *** Start programmer edit section *** (TFSChangeSet.Number Get start)

                // *** End programmer edit section *** (TFSChangeSet.Number Get start)
                int result = this.fNumber;
                // *** Start programmer edit section *** (TFSChangeSet.Number Get end)

                // *** End programmer edit section *** (TFSChangeSet.Number Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (TFSChangeSet.Number Set start)

                // *** End programmer edit section *** (TFSChangeSet.Number Set start)
                this.fNumber = value;
                // *** Start programmer edit section *** (TFSChangeSet.Number Set end)

                // *** End programmer edit section *** (TFSChangeSet.Number Set end)
            }
        }
        
        /// <summary>
        /// ��������
        /// </summary>
        // *** Start programmer edit section *** (TFSChangeSet.Reviewed CustomAttributes)

        // *** End programmer edit section *** (TFSChangeSet.Reviewed CustomAttributes)
        [NotNull()]
        public virtual bool Reviewed
        {
            get
            {
                // *** Start programmer edit section *** (TFSChangeSet.Reviewed Get start)

                // *** End programmer edit section *** (TFSChangeSet.Reviewed Get start)
                bool result = this.fReviewed;
                // *** Start programmer edit section *** (TFSChangeSet.Reviewed Get end)

                // *** End programmer edit section *** (TFSChangeSet.Reviewed Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (TFSChangeSet.Reviewed Set start)

                // *** End programmer edit section *** (TFSChangeSet.Reviewed Set start)
                this.fReviewed = value;
                // *** Start programmer edit section *** (TFSChangeSet.Reviewed Set end)

                // *** End programmer edit section *** (TFSChangeSet.Reviewed Set end)
            }
        }
        
        /// <summary>
        /// ����������� � ������ ���������
        /// </summary>
        // *** Start programmer edit section *** (TFSChangeSet.Comment CustomAttributes)

        // *** End programmer edit section *** (TFSChangeSet.Comment CustomAttributes)
        [StrLen(255)]
        public virtual string Comment
        {
            get
            {
                // *** Start programmer edit section *** (TFSChangeSet.Comment Get start)

                // *** End programmer edit section *** (TFSChangeSet.Comment Get start)
                string result = this.fComment;
                // *** Start programmer edit section *** (TFSChangeSet.Comment Get end)

                // *** End programmer edit section *** (TFSChangeSet.Comment Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (TFSChangeSet.Comment Set start)

                // *** End programmer edit section *** (TFSChangeSet.Comment Set start)
                this.fComment = value;
                // *** Start programmer edit section *** (TFSChangeSet.Comment Set end)

                // *** End programmer edit section *** (TFSChangeSet.Comment Set end)
            }
        }
        
        /// <summary>
        /// ����
        /// </summary>
        // *** Start programmer edit section *** (TFSChangeSet.Date CustomAttributes)

        // *** End programmer edit section *** (TFSChangeSet.Date CustomAttributes)
        [NotNull()]
        public virtual System.DateTime Date
        {
            get
            {
                // *** Start programmer edit section *** (TFSChangeSet.Date Get start)

                // *** End programmer edit section *** (TFSChangeSet.Date Get start)
                System.DateTime result = this.fDate;
                // *** Start programmer edit section *** (TFSChangeSet.Date Get end)

                // *** End programmer edit section *** (TFSChangeSet.Date Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (TFSChangeSet.Date Set start)

                // *** End programmer edit section *** (TFSChangeSet.Date Set start)
                this.fDate = value;
                // *** Start programmer edit section *** (TFSChangeSet.Date Set end)

                // *** End programmer edit section *** (TFSChangeSet.Date Set end)
            }
        }
        
        /// <summary>
        /// ����������� ������������
        /// </summary>
        // *** Start programmer edit section *** (TFSChangeSet.ReviewerNote CustomAttributes)

        // *** End programmer edit section *** (TFSChangeSet.ReviewerNote CustomAttributes)
        [StrLen(255)]
        public virtual string ReviewerNote
        {
            get
            {
                // *** Start programmer edit section *** (TFSChangeSet.ReviewerNote Get start)

                // *** End programmer edit section *** (TFSChangeSet.ReviewerNote Get start)
                string result = this.fReviewerNote;
                // *** Start programmer edit section *** (TFSChangeSet.ReviewerNote Get end)

                // *** End programmer edit section *** (TFSChangeSet.ReviewerNote Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (TFSChangeSet.ReviewerNote Set start)

                // *** End programmer edit section *** (TFSChangeSet.ReviewerNote Set start)
                this.fReviewerNote = value;
                // *** Start programmer edit section *** (TFSChangeSet.ReviewerNote Set end)

                // *** End programmer edit section *** (TFSChangeSet.ReviewerNote Set end)
            }
        }
        
        /// <summary>
        /// ����������� ��� ������
        /// </summary>
        // *** Start programmer edit section *** (TFSChangeSet.Notify CustomAttributes)

        // *** End programmer edit section *** (TFSChangeSet.Notify CustomAttributes)
        [NotNull()]
        public virtual bool Notify
        {
            get
            {
                // *** Start programmer edit section *** (TFSChangeSet.Notify Get start)

                // *** End programmer edit section *** (TFSChangeSet.Notify Get start)
                bool result = this.fNotify;
                // *** Start programmer edit section *** (TFSChangeSet.Notify Get end)

                // *** End programmer edit section *** (TFSChangeSet.Notify Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (TFSChangeSet.Notify Set start)

                // *** End programmer edit section *** (TFSChangeSet.Notify Set start)
                this.fNotify = value;
                // *** Start programmer edit section *** (TFSChangeSet.Notify Set end)

                // *** End programmer edit section *** (TFSChangeSet.Notify Set end)
            }
        }
        
        /// <summary>
        /// ����� ������������� �� �����������
        /// </summary>
        // *** Start programmer edit section *** (TFSChangeSet.Accept CustomAttributes)

        // *** End programmer edit section *** (TFSChangeSet.Accept CustomAttributes)
        [NotNull()]
        public virtual bool Accept
        {
            get
            {
                // *** Start programmer edit section *** (TFSChangeSet.Accept Get start)

                // *** End programmer edit section *** (TFSChangeSet.Accept Get start)
                bool result = this.fAccept;
                // *** Start programmer edit section *** (TFSChangeSet.Accept Get end)

                // *** End programmer edit section *** (TFSChangeSet.Accept Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (TFSChangeSet.Accept Set start)

                // *** End programmer edit section *** (TFSChangeSet.Accept Set start)
                this.fAccept = value;
                // *** Start programmer edit section *** (TFSChangeSet.Accept Set end)

                // *** End programmer edit section *** (TFSChangeSet.Accept Set end)
            }
        }
        
        /// <summary>
        /// ����� ��������� �� TFS.
        /// </summary>
        // *** Start programmer edit section *** (TFSChangeSet.ChangesetAuthor CustomAttributes)

        // *** End programmer edit section *** (TFSChangeSet.ChangesetAuthor CustomAttributes)
        [PropertyStorage(new string[] {
                "ChangesetAuthor"})]
        [NotNull()]
        public virtual IIS.CodeReviewEngine.TFSProgrammer ChangesetAuthor
        {
            get
            {
                // *** Start programmer edit section *** (TFSChangeSet.ChangesetAuthor Get start)

                // *** End programmer edit section *** (TFSChangeSet.ChangesetAuthor Get start)
                IIS.CodeReviewEngine.TFSProgrammer result = this.fChangesetAuthor;
                // *** Start programmer edit section *** (TFSChangeSet.ChangesetAuthor Get end)

                // *** End programmer edit section *** (TFSChangeSet.ChangesetAuthor Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (TFSChangeSet.ChangesetAuthor Set start)

                // *** End programmer edit section *** (TFSChangeSet.ChangesetAuthor Set start)
                this.fChangesetAuthor = value;
                // *** Start programmer edit section *** (TFSChangeSet.ChangesetAuthor Set end)

                // *** End programmer edit section *** (TFSChangeSet.ChangesetAuthor Set end)
            }
        }
        
        /// <summary>
        /// ����� ��������� �� TFS.
        /// </summary>
        // *** Start programmer edit section *** (TFSChangeSet.Reviewer CustomAttributes)

        // *** End programmer edit section *** (TFSChangeSet.Reviewer CustomAttributes)
        [PropertyStorage(new string[] {
                "Reviewer"})]
        [NotNull()]
        public virtual IIS.CodeReviewEngine.TFSProgrammer Reviewer
        {
            get
            {
                // *** Start programmer edit section *** (TFSChangeSet.Reviewer Get start)

                // *** End programmer edit section *** (TFSChangeSet.Reviewer Get start)
                IIS.CodeReviewEngine.TFSProgrammer result = this.fReviewer;
                // *** Start programmer edit section *** (TFSChangeSet.Reviewer Get end)

                // *** End programmer edit section *** (TFSChangeSet.Reviewer Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (TFSChangeSet.Reviewer Set start)

                // *** End programmer edit section *** (TFSChangeSet.Reviewer Set start)
                this.fReviewer = value;
                // *** Start programmer edit section *** (TFSChangeSet.Reviewer Set end)

                // *** End programmer edit section *** (TFSChangeSet.Reviewer Set end)
            }
        }
        
        /// <summary>
        /// ����� ��������� �� TFS.
        /// </summary>
        // *** Start programmer edit section *** (TFSChangeSet.TeamSlice CustomAttributes)

        // *** End programmer edit section *** (TFSChangeSet.TeamSlice CustomAttributes)
        [PropertyStorage(new string[] {
                "TeamSlice"})]
        [NotNull()]
        public virtual IIS.CodeReviewEngine.TeamSlice TeamSlice
        {
            get
            {
                // *** Start programmer edit section *** (TFSChangeSet.TeamSlice Get start)

                // *** End programmer edit section *** (TFSChangeSet.TeamSlice Get start)
                IIS.CodeReviewEngine.TeamSlice result = this.fTeamSlice;
                // *** Start programmer edit section *** (TFSChangeSet.TeamSlice Get end)

                // *** End programmer edit section *** (TFSChangeSet.TeamSlice Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (TFSChangeSet.TeamSlice Set start)

                // *** End programmer edit section *** (TFSChangeSet.TeamSlice Set start)
                this.fTeamSlice = value;
                // *** Start programmer edit section *** (TFSChangeSet.TeamSlice Set end)

                // *** End programmer edit section *** (TFSChangeSet.TeamSlice Set end)
            }
        }
        
        /// <summary>
        /// Class views container
        /// </summary>
        public class Views
        {
            
            /// <summary>
            /// "ChangeSetE" view
            /// </summary>
            public static ICSSoft.STORMNET.View ChangeSetE
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("ChangeSetE", typeof(IIS.CodeReviewEngine.TFSChangeSet));
                }
            }
            
            /// <summary>
            /// "ChangeSetL" view
            /// </summary>
            public static ICSSoft.STORMNET.View ChangeSetL
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("ChangeSetL", typeof(IIS.CodeReviewEngine.TFSChangeSet));
                }
            }
            
            /// <summary>
            /// "TFSChangeSetE" view
            /// </summary>
            public static ICSSoft.STORMNET.View TFSChangeSetE
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("TFSChangeSetE", typeof(IIS.CodeReviewEngine.TFSChangeSet));
                }
            }
            
            /// <summary>
            /// "TFSChangeSetL" view
            /// </summary>
            public static ICSSoft.STORMNET.View TFSChangeSetL
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("TFSChangeSetL", typeof(IIS.CodeReviewEngine.TFSChangeSet));
                }
            }
        }
    }
}
