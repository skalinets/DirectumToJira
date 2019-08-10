using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira.libraries
{
    class WorkDayProvider : IWorkDayProvider
    {
        private string workDaysEndpointName;

        public WorkDayProvider(string workDaysEndpointName)
        {
            this.workDaysEndpointName = workDaysEndpointName;
        }

        public bool CheckDateIsWork(DateTime requestedDate, bool fromCash)
        {
            throw new NotImplementedException();
        }
    }
}
