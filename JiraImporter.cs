using DirectumToJira.libraries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira
{
    class JiraImporter
    {
        private readonly NLog.ILogger _logger;
        private readonly AtbCalendar _atbCalendar;
        private readonly DirectumIssueStrategy _directumIssueStrategy;
        private readonly DirectumRegistryChangeStrategy _directumRegistryChangeStrategy;
        private readonly IEnumerable<IDirectumIssueStrategyImport> _strategies;

        public JiraImporter(NLog.ILogger logger,
             AtbCalendar atbCalendar, 
//             DirectumIssueStrategy directumIssueStrategy, DirectumRegistryChangeStrategy directumRegistryChangeStrategy,
             IEnumerable<IDirectumIssueStrategyImport> strategies
             )
        {
            _logger = logger; 
            _atbCalendar = atbCalendar; 
            _strategies = strategies;
        }

        public string Foo()
        {
            Debug.Assert(_strategies.Count() == 2);
            return _strategies.Count().ToString();
        }

        public void Import(DateTime dateCreation, DirectumIssuesFilter filter)
        {
            var logHelper = new LogHelper(_logger);

            var dayCreation = _atbCalendar.GetDay(dateCreation);

            if (!dayCreation.IsWorkingDay)
            {
                logHelper.Info($"{dayCreation.Date} не рабочий день");
                return;
            }
            try
            {
                _strategies
                    .ToList()
                    .ForEach(_ => _.Import(logHelper, dateCreation, filter));

            }
            catch (Exception ex)
            {
                logHelper.Error(ex);

            }

        }
    }
}
