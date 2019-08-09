using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira.libraries
{
    class Issue
    {
        private const int HOURS = 3600;

        public Int64 IssueId { get; set; }
        public string IssueKey { get; set; }
        public string IssueUrl { get; set; }
        public string Assignee { get; set; }
        public string Author { get; set; }

        public string SourceSystem { get; set; }

        public string SourceNumber { get; set; }
        public string SourceName { get; set; }

        public string SourceTicket { get; set; }
        public DateTime? Created { get; set; }
        public string Email { get; set; }
        public string AssigneeLogin { get; set; }

        private double _originalEstimate;
        public double OriginalEstimate
        {
            get { return _originalEstimate; }
            set { _originalEstimate = Math.Round(value / HOURS, 2); }
        }

        private double _timeSpent;

        public double TimeSpent
        {
            get { return _timeSpent; }
            set { _timeSpent = Math.Round(value / HOURS, 2); }
        }

        public string PriorityId { get; set; }
        public string Priority { get; set; }


        public string Summary { get; set; }
        public string Type { get; set; }
        public string TypeId { get; set; }
        public string[] Labels { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? MaxDueDate { get; set; }
        public bool ExistComments { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Tester { get; set; }

        public string Description { get; set; }
        public string Note { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
