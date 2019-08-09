using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira.libraries
{
    class DirectumJiraExchangeProvider
    {
        private ConnectionStringSettings directumJiraExchange;

        public DirectumJiraExchangeProvider(ConnectionStringSettings directumJiraExchange)
        {
            this.directumJiraExchange = directumJiraExchange;
        }

        public IEnumerable<DirectumIssue> GetIssues(DirectumIssuesFilter filter)
        {
            return null;
        }

        public IEnumerable<DirectumRegistryItem> GetRegistryItems(DirectumIssuesFilter filter)
        {
            return null;
        }

        public void UpdateState(CreateIssueResult[] result)
        {

        }

        public IEnumerable<DirectumIssue> GetIssuesForCreate(DirectumIssuesFilter filter, DateTime dateCreation)
        {
            return GetIssues(filter)
                    .Where(issue => !(issue.InternalState == InternalState.Complete && issue.ModificationTime.Date == dateCreation.Date));
        }
    }

    public enum DirectumState { Start, Yes, No, Terminated, Done }

    public enum InternalState { New = 1, Proceed = 2, Complete = 3, Error = 4 }
}