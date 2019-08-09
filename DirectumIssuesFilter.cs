using DirectumToJira.libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira
{
    class DirectumIssuesFilter
    {
        public IEnumerable<DirectumState> DirectumStates { get; set; }
        public IEnumerable<InternalState> InternalStates { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Assignees { get; set; }
        public string Number { get; set; }
    }
}
