using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira.libraries
{
    class BaseGetIssuesFilter
    {
        public string Author { get; set; }
        public DateTime DueDateStart { get; set; }
        public DateTime? DueDateEnd { get; set; }
        public string[] IssueKeyList { get; set; }
        public string[] ProjectKeyList { get; set; }
        public string[] IssueTypeIdList { get; set; }
        public int[] IssueStatusIdList { get; set; }
        public DateTime? UpdateDateTime { get; set; }
    }
}
