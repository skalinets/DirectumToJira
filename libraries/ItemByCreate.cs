using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira.libraries
{
    class ItemByCreate : IssueByCreateBase
    {
        public string ChangeManager { get; set; }
        public string Priority { get; set; }
        public string Initiator { get; set; }
        public string DepartmentId { get; set; }
        public string ReasonImplementation { get; set; }
        public string ConsequencesFailureImplement { get; set; }
        public DateTime DeadLine { get; set; }
    }
}
