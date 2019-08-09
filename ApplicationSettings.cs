using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira
{
    public static class ApplicationSettings
    {
        public const string WorkDaysEndpointName = "ServiceWorkDays";
        public const string JiraEndpointName = "ServiceJira";
        public const string EmployeeEndpointName = "ServiceItUsers";
        public static ConnectionStringSettings DirectumJiraExchange { get; } = ConfigurationManager.ConnectionStrings["DirectumJiraExchange"];
        public static TimeSpan LivePollingPeriod { get; } = TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["LivePollingPeriodMinutes"], CultureInfo.InvariantCulture));
        public static TimeSpan BylawPollingTime { get; } = TimeSpan.Parse(ConfigurationManager.AppSettings["BylawPollingTime"], CultureInfo.InvariantCulture);
    }
}
