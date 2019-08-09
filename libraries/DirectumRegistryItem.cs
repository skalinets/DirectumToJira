using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira.libraries
{
    class DirectumRegistryItem
    {
        public int AssignmentID { get; set; }
        public string ProjectId { get; set; }
        public string IssuetypeId { get; set; }
        public string Number { get; set; }
        public string Initiator { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime CreateDate { get; set; }
        public string ConsequencesFailureImplement { get; set; }
        public string PriorityInitiatorId { get; set; }
        public string DepartmentITId { get; set; }
        public string ChangeManager { get; set; }
        public string AssigneeLoginAd { get; set; }
    }
}
