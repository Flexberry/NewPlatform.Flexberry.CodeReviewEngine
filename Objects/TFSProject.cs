﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace IIS.CodeReviewEngine
{
    using System;
    using System.Linq;
    using System.Xml;
    using ICSSoft.STORMNET;
    
    
    // *** Start programmer edit section *** (Using statements)

    // *** End programmer edit section *** (Using statements)


    /// <summary>
    /// Проект из TFS.
    /// </summary>
    // *** Start programmer edit section *** (TFSProject CustomAttributes)

    // *** End programmer edit section *** (TFSProject CustomAttributes)
    [AutoAltered()]
    [Caption("Проект TFS")]
    [AccessType(ICSSoft.STORMNET.AccessType.none)]
    [View("TFSProjectE", new string[] {
            "Name as \'Name\'",
            "LastCheckDate as \'Last check date\'",
            "WorkItemAreaPath",
            "WorkItemIterationPath"})]
    [View("TFSProjectL", new string[] {
            "Name as \'Name\'",
            "LastCheckDate as \'Last check date\'",
            "WorkItemAreaPath",
            "WorkItemIterationPath"})]
    public class TFSProject : ICSSoft.STORMNET.DataObject
    {
        
        private string fName;
        
        private ICSSoft.STORMNET.UserDataTypes.NullableDateTime fLastCheckDate;
        
        private string fWorkItemAreaPath;
        
        private string fWorkItemIterationPath;
        
        // *** Start programmer edit section *** (TFSProject CustomMembers)
        /// <summary>
        /// Получить наименование проекта в TFS.
        /// </summary>
        /// <returns>Наименование проекта в TFS.</returns>
        public string GetProjectName()
        {
            if (string.IsNullOrEmpty(Name))
                return null;

            return Name.Replace(GetProjectCollectionUrl() + '/', "");            
        }

        /// <summary>
        /// Получить Url коллекции проектов в TFS.
        /// </summary>
        /// <returns>Url коллекции проектов в TFS.</returns>
        public string GetProjectCollectionUrl()
        {
            if (string.IsNullOrEmpty(Name))
                return null;

            List<string> partsTfsProjectName = Name.Split('/').ToList();
            var partsTfsCollectionName = partsTfsProjectName.GetRange(0, 4);
            return string.Join("/", partsTfsCollectionName);
        }

        // *** End programmer edit section *** (TFSProject CustomMembers)

        
        /// <summary>
        /// Название.
        /// </summary>
        // *** Start programmer edit section *** (TFSProject.Name CustomAttributes)

        // *** End programmer edit section *** (TFSProject.Name CustomAttributes)
        [StrLen(255)]
        [NotNull()]
        public virtual string Name
        {
            get
            {
                // *** Start programmer edit section *** (TFSProject.Name Get start)

                // *** End programmer edit section *** (TFSProject.Name Get start)
                string result = this.fName;
                // *** Start programmer edit section *** (TFSProject.Name Get end)

                // *** End programmer edit section *** (TFSProject.Name Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (TFSProject.Name Set start)

                // *** End programmer edit section *** (TFSProject.Name Set start)
                this.fName = value;
                // *** Start programmer edit section *** (TFSProject.Name Set end)

                // *** End programmer edit section *** (TFSProject.Name Set end)
            }
        }
        
        /// <summary>
        /// Дата последней проверки проекта. Проставляется автоматически.
        /// </summary>
        // *** Start programmer edit section *** (TFSProject.LastCheckDate CustomAttributes)

        // *** End programmer edit section *** (TFSProject.LastCheckDate CustomAttributes)
        public virtual ICSSoft.STORMNET.UserDataTypes.NullableDateTime LastCheckDate
        {
            get
            {
                // *** Start programmer edit section *** (TFSProject.LastCheckDate Get start)

                // *** End programmer edit section *** (TFSProject.LastCheckDate Get start)
                ICSSoft.STORMNET.UserDataTypes.NullableDateTime result = this.fLastCheckDate;
                // *** Start programmer edit section *** (TFSProject.LastCheckDate Get end)

                // *** End programmer edit section *** (TFSProject.LastCheckDate Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (TFSProject.LastCheckDate Set start)

                // *** End programmer edit section *** (TFSProject.LastCheckDate Set start)
                this.fLastCheckDate = value;
                // *** Start programmer edit section *** (TFSProject.LastCheckDate Set end)

                // *** End programmer edit section *** (TFSProject.LastCheckDate Set end)
            }
        }
        
        /// <summary>
        /// WorkItemAreaPath.
        /// </summary>
        // *** Start programmer edit section *** (TFSProject.WorkItemAreaPath CustomAttributes)

        // *** End programmer edit section *** (TFSProject.WorkItemAreaPath CustomAttributes)
        [StrLen(255)]
        public virtual string WorkItemAreaPath
        {
            get
            {
                // *** Start programmer edit section *** (TFSProject.WorkItemAreaPath Get start)

                // *** End programmer edit section *** (TFSProject.WorkItemAreaPath Get start)
                string result = this.fWorkItemAreaPath;
                // *** Start programmer edit section *** (TFSProject.WorkItemAreaPath Get end)

                // *** End programmer edit section *** (TFSProject.WorkItemAreaPath Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (TFSProject.WorkItemAreaPath Set start)

                // *** End programmer edit section *** (TFSProject.WorkItemAreaPath Set start)
                this.fWorkItemAreaPath = value;
                // *** Start programmer edit section *** (TFSProject.WorkItemAreaPath Set end)

                // *** End programmer edit section *** (TFSProject.WorkItemAreaPath Set end)
            }
        }
        
        /// <summary>
        /// WorkItemIterationPath.
        /// </summary>
        // *** Start programmer edit section *** (TFSProject.WorkItemIterationPath CustomAttributes)

        // *** End programmer edit section *** (TFSProject.WorkItemIterationPath CustomAttributes)
        [StrLen(255)]
        public virtual string WorkItemIterationPath
        {
            get
            {
                // *** Start programmer edit section *** (TFSProject.WorkItemIterationPath Get start)

                // *** End programmer edit section *** (TFSProject.WorkItemIterationPath Get start)
                string result = this.fWorkItemIterationPath;
                // *** Start programmer edit section *** (TFSProject.WorkItemIterationPath Get end)

                // *** End programmer edit section *** (TFSProject.WorkItemIterationPath Get end)
                return result;
            }
            set
            {
                // *** Start programmer edit section *** (TFSProject.WorkItemIterationPath Set start)

                // *** End programmer edit section *** (TFSProject.WorkItemIterationPath Set start)
                this.fWorkItemIterationPath = value;
                // *** Start programmer edit section *** (TFSProject.WorkItemIterationPath Set end)

                // *** End programmer edit section *** (TFSProject.WorkItemIterationPath Set end)
            }
        }
        
        /// <summary>
        /// Class views container.
        /// </summary>
        public class Views
        {
            
            /// <summary>
            /// "TFSProjectE" view.
            /// </summary>
            public static ICSSoft.STORMNET.View TFSProjectE
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("TFSProjectE", typeof(IIS.CodeReviewEngine.TFSProject));
                }
            }
            
            /// <summary>
            /// "TFSProjectL" view.
            /// </summary>
            public static ICSSoft.STORMNET.View TFSProjectL
            {
                get
                {
                    return ICSSoft.STORMNET.Information.GetView("TFSProjectL", typeof(IIS.CodeReviewEngine.TFSProject));
                }
            }
        }
    }
}
