using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira
{
    interface IWorkDayProvider
    {
        bool CheckDateIsWork(DateTime requestedDate, bool fromCash);
    }
}
