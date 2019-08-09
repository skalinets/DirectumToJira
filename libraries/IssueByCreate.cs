using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira.libraries
{
    class IssueByCreate : IssueByCreateBase
    {

        public string Estimate { get; set; }
        public string SourceSystem { get; set; }
        public string PriorityId { get; set; }
        public string SourceName { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
    }

    class IssueByCreateBase
    {
        public string Project { get; set; }
        public string IssueType { get; set; }
        public string Assignee { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string SourceNumber { get; set; }
    }
}
