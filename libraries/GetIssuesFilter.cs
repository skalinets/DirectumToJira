using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira.libraries
{
    class GetIssuesFilter: BaseGetIssuesFilter
    {
        public string[] Assignee { get; set; }
        public bool WithHistory { get; set; }

    }
}
