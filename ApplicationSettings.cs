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
        public const string JiraEndpointName2 = "ServiceJira2";
        public const string EmployeeEndpointName = "ServiceItUsers";
        public static string DirectumJiraExchange { get; } = "foo"; 
        
        //ConfigurationManager.ConnectionStrings["DirectumJiraExchange"];
        public static TimeSpan LivePollingPeriod { get; } = TimeSpan.FromMinutes(1);
        public static TimeSpan BylawPollingTime { get; } = TimeSpan.Zero;
    }
}
