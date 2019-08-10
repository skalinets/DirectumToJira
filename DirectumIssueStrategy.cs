using DirectumToJira.libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira
{
    class DirectumIssueStrategy : DirectumIssueStrategyBase<IssueByCreate>
    {
        public DirectumIssueStrategy(IJiraProvider jiraProvider, IDirectumJiraExchangeProvider directumJiraExchangeProvider,
            DirectumToJiraMapper directumToJiraMapper)
            : base(jiraProvider, directumJiraExchangeProvider, directumToJiraMapper)
        {
        }

        protected override string PROJECT_ID => "ASS";

        protected override string GetMessage(IssueByCreate issueByCreate)
        {
            return $"{issueByCreate.SourceNumber} {issueByCreate.Summary} DueDate:{issueByCreate.DueDate} Project:{issueByCreate.Project}";
        }

        protected override Task<string> CreateInJira(IssueByCreate item) => _jiraProvider.CreateIssueAsync(item);

        protected override IssueByCreate[] GetIssues(DirectumIssuesFilter _filter, DateTime _dateCreation, LogHelper _logHelper)
        {
            return _directumJiraExchangeProvider
                .GetIssues(_filter)
                .Where(issue => !(issue.InternalState == InternalState.Complete && issue.ModificationTime.Date == _dateCreation.Date))
                .Select(directumIssue => _directumToJiraMapper.Map(directumIssue, _dateCreation))
                .ToArray();
        }
    }
}
