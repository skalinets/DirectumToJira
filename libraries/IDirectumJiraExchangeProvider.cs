using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira.libraries
{
    interface IDirectumJiraExchangeProvider
    {
        IEnumerable<DirectumIssue> GetIssues(DirectumIssuesFilter filter);
        IEnumerable<DirectumRegistryItem> GetRegistryItems(DirectumIssuesFilter filter);
        void UpdateState(CreateIssueResult[] result);
        IEnumerable<DirectumIssue> GetIssuesForCreate(DirectumIssuesFilter filter, DateTime dateCreation);
    }
}
