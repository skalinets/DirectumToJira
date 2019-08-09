using DirectumToJira.libraries;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira
{
    class BylawJob : IJob
    {
        private readonly JiraImporter _jiraImporter;
        private readonly ILogger _logger;

        public BylawJob(JiraImporter jiraImporter, ILogger logger)
        {
            _jiraImporter = jiraImporter ?? throw new ArgumentNullException(nameof(jiraImporter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.Info($"Выполняется создание задач по регламенту");

            var dateSelect = DateTime.Today;
            var dateCreation = dateSelect.AddDays(1);
            var filter = new DirectumIssuesFilter
            {
                EndDate = dateSelect,
                DirectumStates = new[] { DirectumState.Start, DirectumState.Yes }
            };

            await Task.Run(() =>
            {
                _jiraImporter.Import(dateCreation, filter);
            });
        }
    }
}
