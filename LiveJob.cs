using DirectumToJira.libraries;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira
{
    class LiveJob : IJob
    {
        private readonly JiraImporter _jiraImporter;

        public LiveJob(JiraImporter jiraImporter)
        {
            _jiraImporter = jiraImporter ?? throw new ArgumentNullException(nameof(jiraImporter));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var dateSelect = DateTime.Today;
            var filter = new DirectumIssuesFilter
            {
                BeginDate = dateSelect,
                EndDate = dateSelect,
                InternalStates = new[] { InternalState.New, InternalState.Proceed, InternalState.Error }
            };

            await Task.Run(() =>
            {
                _jiraImporter.Import(dateSelect, filter);
            });
        }
    }
}
