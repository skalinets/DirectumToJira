using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira
{
    class AtbCalendar
    {
        private readonly IWorkDayProvider _workDayProvider;

        private DateTime _lastCheckDate = new DateTime();
        private bool _workingDay = true;

        public AtbCalendar(IWorkDayProvider workDayProvider)
        {
            _workDayProvider = workDayProvider ?? throw new ArgumentNullException(nameof(workDayProvider));
        }

        public Day GetDay(DateTime date)
        {
            if (_lastCheckDate != date)
            {
                _workingDay = _workDayProvider.CheckDateIsWork(date, false);
                _lastCheckDate = date;
            }
            return new Day { Date = date, IsWorkingDay = _workingDay };
        }
    }

    public class Day
    {
        public DateTime Date { get; set; }
        public bool IsWorkingDay { get; set; }
    }
}
