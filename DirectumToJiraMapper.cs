using DirectumToJira.libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira
{
        public enum SourceSystemIds
    {
        JIRA = 10304,
        Directum = 10305,
        Reglament = 10306
    }
    class DirectumToJiraMapper
    {

        private readonly IJiraProvider _jiraProvider;
        private readonly IEmployeeProvider _employeeProvider;
        private readonly TimeSpan _employeeCacheTime = TimeSpan.FromHours(1);
        private readonly TimeSpan _issueTypeCacheTime = TimeSpan.FromHours(48);
        private readonly TimeSpan _jiraProjectCacheTime = TimeSpan.FromHours(8);
        private readonly Cache _cache;

        public DirectumToJiraMapper(IEmployeeProvider employeeProvider, IJiraProvider jiraProvider, Cache cache)
        {
            _jiraProvider = jiraProvider ?? throw new ArgumentNullException(nameof(jiraProvider));
            _employeeProvider = employeeProvider ?? throw new ArgumentNullException(nameof(employeeProvider));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public IssueByCreate Map(DirectumIssue directumIssue, DateTime dateCreation)
        {
            var issueType = _cache.GetObjectFromCache<IssueType>("IssueType", _issueTypeCacheTime, () => _jiraProvider.GetTypes().First(t => t.Name.ToUpper() == "ЗАДАЧА"));
            var employees = _cache.GetObjectFromCache(nameof(_employeeProvider), _employeeCacheTime, _employeeProvider.GetEmployees);

            var assignee = employees.FirstOrDefault(x => x.LoginAd == directumIssue.AssigneeLoginAd);

            var issue = new IssueByCreate
            {
                //Project = "10704",//assignee?.DepartmentJiraId, -- test
                Project = assignee?.DepartmentJiraId,
                Assignee = assignee?.LoginJira,
                Summary = $"{directumIssue.Number} {directumIssue.Title}",
                Description = $"{directumIssue.Number} {directumIssue.Title}\n{directumIssue.Author}\n",
                Estimate = $"{30}",
                SourceSystem = ((int)SourceSystemIds.Directum).ToString(),
                SourceNumber = directumIssue.AssignmentId.ToString(),

                IssueType = issueType?.Id,
                StartDate = dateCreation.Add(DateTime.Now.TimeOfDay),
                DueDate = dateCreation
            };
            return issue;
        }

        public ItemByCreate MapItem(DirectumRegistryItem directumRegistryItem, LogHelper logHelper)
        {
            var issueType = _cache.GetObjectFromCache("IssueType", _issueTypeCacheTime, () => _jiraProvider.GetTypes().First(t => t.Name.ToUpper() == "ИЗМЕНЕНИЕ"));
            var employees = _cache.GetObjectFromCache(nameof(_employeeProvider), _employeeCacheTime, _employeeProvider.GetEmployees);
            var projects = _cache.GetObjectFromCache(nameof(_jiraProvider), _jiraProjectCacheTime, _jiraProvider.GetProjects);

            var catalog = CheckAndReturnCatalogs(directumRegistryItem.ProjectId, directumRegistryItem, issueType, logHelper);

            var assignee = employees.FirstOrDefault(x => x.LoginAd == directumRegistryItem.AssigneeLoginAd);
            var changeManager = employees.FirstOrDefault(x => x.LoginAd == directumRegistryItem.ChangeManager);

            var item = new ItemByCreate
            {
                Project = projects.FirstOrDefault(e => e.Key == directumRegistryItem.ProjectId)?.Id,
                IssueType = catalog.issueType,
                Assignee = assignee?.LoginJira,
                ChangeManager = changeManager?.LoginJira,
                Priority = catalog.priorityInitiator,
                Initiator = directumRegistryItem.Initiator,
                Summary = $"{directumRegistryItem.Number} {directumRegistryItem.Summary}",
                Description = directumRegistryItem.Description,
                SourceNumber = directumRegistryItem.AssignmentID.ToString(),
                DepartmentId = catalog.department,
                ReasonImplementation = directumRegistryItem.Number,
                ConsequencesFailureImplement = directumRegistryItem.ConsequencesFailureImplement,
                DeadLine = directumRegistryItem.Deadline
            };

            return item;
        }
        private (string issueType, string priorityInitiator, string department) CheckAndReturnCatalogs(string project, DirectumRegistryItem directumRegistryItem, IssueType issueType, LogHelper logHelper)
        {
            var catalog = _jiraProvider.GetCataloges(project);
            var catalogIssueTypeResult = catalog.Issuetype.FirstOrDefault(i => i.Name == issueType.Name)?.Id.ToString();



            if (catalogIssueTypeResult == null)
            {
                logHelper.Error(new ArgumentException($"Тип запроса \"{issueType.Name}\" не определён в справочнике JIRA"));
            }


            var catalogPriorityResult = catalog.PriorityInitiator.FirstOrDefault(p => p.Name == directumRegistryItem.PriorityInitiatorId)?.Id.ToString();
            if (catalogPriorityResult == null)
            {
                logHelper.Error(new ArgumentException($"Приоритет \"{directumRegistryItem.PriorityInitiatorId}\" не определён в справочнике JIRA"));
            }


            var catalogDepartmentResult = catalog.DepartIt.FirstOrDefault(d => d.Name == directumRegistryItem.DepartmentITId)?.Id.ToString();
            if (catalogDepartmentResult == null)
            {
                logHelper.Error(new ArgumentException($"Подразделение ИТ \"{directumRegistryItem.DepartmentITId}\" не определено в справочнике JIRA"));
            }

            return (catalogIssueTypeResult, catalogPriorityResult, catalogDepartmentResult);
            //return (null, null, null);
        }
    }
}
