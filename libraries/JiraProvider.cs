using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira.libraries
{
    class JiraProvider : IJiraProvider
    {
        private string jiraEndpointName;

        public JiraProvider(string jiraEndpointName)
        {
            this.jiraEndpointName = jiraEndpointName;
        }

        public string ServerUrl => throw new NotImplementedException();

        public string UrlCreateTicket => throw new NotImplementedException();

        public string ChangeStatus(string IssueKey, int StatusId)
        {
            throw new NotImplementedException();
        }

        public string CreateIssue(IssueByCreate issueCr)
        {
            throw new NotImplementedException();
        }

        public string CreateRegistryItem(ItemByCreate registryItemCr)
        {
            throw new NotImplementedException();
        }

        public DepartITModel GetCataloges(string projectKey)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Issue> GetIssues(GetIssuesFilter filter)
        {
            throw new NotImplementedException();
        }

        public List<JiraProject> GetProjects()
        {
            throw new NotImplementedException();
        }

        public List<IssueType> GetTypes()
        {
            throw new NotImplementedException();
        }
    }
}
