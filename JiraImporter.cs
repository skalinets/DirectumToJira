using DirectumToJira.libraries;
using System;
using System.Collections.Generic;
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

        public JiraImporter(NLog.ILogger logger,
             AtbCalendar atbCalendar, DirectumIssueStrategy directumIssueStrategy, DirectumRegistryChangeStrategy directumRegistryChangeStrategy)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _atbCalendar = atbCalendar ?? throw new ArgumentNullException(nameof(atbCalendar));
            _directumIssueStrategy = directumIssueStrategy ?? throw new ArgumentNullException(nameof(directumIssueStrategy));
            _directumRegistryChangeStrategy = directumRegistryChangeStrategy ?? throw new ArgumentNullException(nameof(directumRegistryChangeStrategy));
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
                _directumIssueStrategy.Import(logHelper, dateCreation, filter);
                _directumRegistryChangeStrategy.Import(logHelper, dateCreation, filter);

            }
            catch (Exception ex)
            {
                logHelper.Error(ex);

            }

        }
    }
}
