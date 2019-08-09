using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira.libraries
{
    public class CreateIssueResult
    {
        public string Project { get; set; }
        public int AssignmentId { get; set; }
        public string IssueKey { get; set; }
        public string ErrorText { get; set; }
        public InternalState TaskStatusCode { get; set; }
    }
}
