using DirectumToJira.libraries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira
{
    interface IDirectumIssueStrategyImport
    {
        void Import(LogHelper logHelper, DateTime dateCreation, DirectumIssuesFilter filter);
    }
    abstract class DirectumIssueStrategyImport : IDirectumIssueStrategyImport
    {
        protected readonly IJiraProvider _jiraProvider;
        protected readonly IDirectumJiraExchangeProvider _directumJiraExchangeProvider;

        public DirectumIssueStrategyImport(IJiraProvider jiraProvider, IDirectumJiraExchangeProvider directumJiraExchangeProvider)
        {
            _jiraProvider = jiraProvider;
            _directumJiraExchangeProvider = directumJiraExchangeProvider;

        }

        protected IssueByCreateBase[] IssuesByCreateBase { get; set; }


        public void Import(LogHelper logHelper, DateTime dateCreation, DirectumIssuesFilter filter)
        {
            GetIssuesFromDb(filter, dateCreation, logHelper);
            CreateIssueResults(logHelper, dateCreation);
        }
        protected abstract void GetIssuesFromDb(DirectumIssuesFilter filter, DateTime dateCreation, LogHelper logHelper);

        private string[] GetExistJiraIssues(DateTime _dateCreation)
        {
            var existJiraIssues = _jiraProvider.GetIssues(new GetIssuesFilter
            {
                DueDateStart = _dateCreation,
                DueDateEnd = _dateCreation,
                Assignee = IssuesByCreateBase.Where(x => x.Assignee != null).Select(x => x.Assignee).ToArray()
            }).Select(a => a.IssueKey).ToArray();

            return existJiraIssues;
        }



        protected void ValidateIssueByCreate(IssueByCreateBase issueByCreateBase, string[] existJiraIssueKeys)
        {
            if (issueByCreateBase.Assignee == null)
                throw new ArgumentException($"Доменный пользователь не найден");

            if ((issueByCreateBase.Project ?? string.Empty) == string.Empty)
                throw new ArgumentException($"Проект не определен");

            var existJiraIssueKey = existJiraIssueKeys.FirstOrDefault(sourceNumber => sourceNumber == issueByCreateBase.SourceNumber);
            if (existJiraIssueKey != null)
                throw new ArgumentException($"Задача уже существует: {existJiraIssueKey}");
        }
        protected abstract CreateIssueResult CreateIssue(IssueByCreateBase issueByCreateBase, string[] existJiraIssueKeys, LogHelper _logHelper);

        protected void CreateIssueResults(LogHelper _logHelper, DateTime dateCreation)
        {
            var existJiraIssueKeys = GetExistJiraIssues(dateCreation);

            var sw = Stopwatch.StartNew();

            var resultIssues = IssuesByCreateBase
               .AsParallel()
               .Select(e => CreateIssue(e, existJiraIssueKeys, _logHelper))
               .ToArray();

            _directumJiraExchangeProvider.UpdateState(resultIssues);

            _logHelper.Info($"Обработка завершена за время {sw.Elapsed:ss\\.fff} сек.");
        }
    }
}
