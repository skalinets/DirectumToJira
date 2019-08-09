using DirectumToJira.libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira
{
    class DirectumIssueStrategy : DirectumIssueStrategyImport
    {
        private const string PROJECT_ID = "ASS";

        private readonly DirectumToJiraMapper _directumToJiraMapper;

        public DirectumIssueStrategy(IJiraProvider jiraProvider, IDirectumJiraExchangeProvider directumJiraExchangeProvider,
            DirectumToJiraMapper directumToJiraMapper)
            : base(jiraProvider, directumJiraExchangeProvider)
        {

            _directumToJiraMapper = directumToJiraMapper;
        }
        protected override void GetIssuesFromDb(DirectumIssuesFilter _filter, DateTime _dateCreation, LogHelper _logHelper)
        {
            _logHelper.Info($"Начало обработки {DateTime.Now}");

            IssuesByCreateBase = _directumJiraExchangeProvider
                .GetIssues(_filter)
                .Where(issue => !(issue.InternalState == InternalState.Complete && issue.ModificationTime.Date == _dateCreation.Date))
                .Select(directumIssue => _directumToJiraMapper.Map(directumIssue, _dateCreation))
                .ToArray();

            _logHelper.Info($"Получено задач по поручениям на {_filter.EndDate}: {IssuesByCreateBase.Count()}");
        }

        protected override CreateIssueResult CreateIssue(IssueByCreateBase issueByCreateBase, string[] existJiraIssueKeys, LogHelper _logHelper)
        {
            var issueByCreate = (IssueByCreate)issueByCreateBase;
            var result = new CreateIssueResult
            {
                AssignmentId = Convert.ToInt32(issueByCreate.SourceNumber),
                Project = PROJECT_ID
            };

            try
            {
                _logHelper.Info($"{issueByCreate.SourceNumber} {issueByCreate.Summary} DueDate:{issueByCreate.DueDate} Project:{issueByCreate.Project}");
                ValidateIssueByCreate(issueByCreate, existJiraIssueKeys);

                result.IssueKey = _jiraProvider.CreateIssue(issueByCreate);
                _logHelper.Info($"Успешно создали задачу: {result.IssueKey}\n");
                result.TaskStatusCode = InternalState.Complete;
            }

            catch (Exception e)
            {
                result.TaskStatusCode = InternalState.Error;
                result.ErrorText = e.Message;

                _logHelper.Error(e);
            }
            return result;
        }
    }
}
