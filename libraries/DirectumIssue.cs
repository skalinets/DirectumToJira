using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira.libraries
{
    class DirectumIssue
    {
        public int AssignmentId { get; set; }
        public string Number { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string AssigneeLoginAd { get; set; }
        public string DirectumStateStr { get; set; }
        public DirectumState DirectumState => (DirectumState)Enum.Parse(typeof(DirectumState), DirectumStateStr, true);
        public string InternalStateStr { get; set; }
        public InternalState InternalState => (InternalState)Enum.Parse(typeof(InternalState), InternalStateStr, true);
        public DateTime CreateDate { get; set; }
        public DateTime ModificationTime { get; set; }
        public string Body { get; set; }
    }
}
