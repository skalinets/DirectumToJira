using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Quartz;
using DirectumToJira.libraries;
using NLog;

namespace DirectumToJira
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            try
            {
                var builder = new ContainerBuilder();

                builder.Register(x => LogManager.GetCurrentClassLogger()).As<ILogger>();
                builder.Register(x => new JiraProvider(ApplicationSettings.JiraEndpointName)).As<IJiraProvider>();
                builder.Register(x => new EmployeeProvider(ApplicationSettings.EmployeeEndpointName)).As<IEmployeeProvider>();
                builder.Register(x => new DirectumJiraExchangeProvider(ApplicationSettings.DirectumJiraExchange)).As<IDirectumJiraExchangeProvider>();
                builder.Register(x => new WorkDayProvider(ApplicationSettings.WorkDaysEndpointName)).As<IWorkDayProvider>();
                builder.Register(x => new Cache(MemoryCache.Default));
                builder.RegisterType<AtbCalendar>();
                builder.RegisterType<JiraImporter>();
                builder.RegisterType<DirectumToJiraMapper>();
                //builder.RegisterType<DirectumToJiraService>();
                builder.RegisterType<DirectumIssueStrategy>();
                builder.RegisterType<DirectumRegistryChangeStrategy>();


                builder.RegisterModule(new QuartzAutofacFactoryModule());
                builder.RegisterModule(new QuartzAutofacJobsModule(typeof(LiveJob).Assembly));


                var container = builder.Build();

                //var service = container.Resolve<DirectumToJiraService>();

                //WindowsServiceCli.Run(service, args);
            }
            catch (Exception e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(e);
            }
        }
    }
}
