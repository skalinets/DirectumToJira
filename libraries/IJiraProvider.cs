using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira.libraries
{
    interface IJiraProvider
    {
        string ServerUrl { get; }
        string UrlCreateTicket { get; }

        /// <summary>
        ///Получение тикетов ERP по фильтру
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>


        string CreateIssue(IssueByCreate issueCr);
        string CreateRegistryItem(ItemByCreate registryItemCr);
        string ChangeStatus(string IssueKey, int StatusId);
        List<IssueType> GetTypes();
        IEnumerable<Issue> GetIssues(GetIssuesFilter filter);
        List<JiraProject> GetProjects();
        DepartITModel GetCataloges(string projectKey);
    }
}
