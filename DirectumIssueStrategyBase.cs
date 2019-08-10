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
        Task Import(LogHelper logHelper, DateTime dateCreation, DirectumIssuesFilter filter);
    }
    abstract class DirectumIssueStrategyBase<T> : IDirectumIssueStrategyImport where T: IssueByCreateBase
    {
        protected abstract string PROJECT_ID { get; }
        protected readonly IJiraProvider _jiraProvider;
        protected readonly IDirectumJiraExchangeProvider _directumJiraExchangeProvider;
        protected readonly DirectumToJiraMapper _directumToJiraMapper;

        protected DirectumIssueStrategyBase(IJiraProvider jiraProvider,
            IDirectumJiraExchangeProvider directumJiraExchangeProvider, 
            DirectumToJiraMapper directumToJiraMapper)
        {
            _jiraProvider = jiraProvider;
            _directumJiraExchangeProvider = directumJiraExchangeProvider;
            _directumToJiraMapper = directumToJiraMapper;

        }

        protected T[] IssuesByCreateBase { get; set; }


        public Task Import(LogHelper logHelper, DateTime dateCreation, DirectumIssuesFilter filter)
        {
            GetIssuesFromDb(filter, dateCreation, logHelper);
            return CreateIssueResults(logHelper, dateCreation);
        }
        private void GetIssuesFromDb(DirectumIssuesFilter _filter, DateTime _dateCreation, LogHelper _logHelper)
        {
            _logHelper.Info($"Начало обработки {DateTime.Now}");

            IssuesByCreateBase = GetIssues(_filter, _dateCreation, _logHelper);

            _logHelper.Info($"Получено задач реестра изменений на {_filter.EndDate}: {IssuesByCreateBase.Count()}");
        }


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

        protected virtual void ValidateIssueByCreate(T issue, string[] existJiraIssueKeys)
        {
            if (issue.Assignee == null)
                throw new ArgumentException($"Доменный пользователь не найден");

            if ((issue.Project ?? string.Empty) == string.Empty)
                throw new ArgumentException($"Проект не определен");

            var existJiraIssueKey = existJiraIssueKeys.FirstOrDefault(sourceNumber => sourceNumber == issue.SourceNumber);
            if (existJiraIssueKey != null)
                throw new ArgumentException($"Задача уже существует: {existJiraIssueKey}");
        }

        protected async Task CreateIssueResults(LogHelper _logHelper, DateTime dateCreation)
        {
            var existJiraIssueKeys = GetExistJiraIssues(dateCreation);

            var sw = Stopwatch.StartNew();

            var resultIssues = await Task.WhenAll(IssuesByCreateBase
                .Select(e => CreateIssue(e, existJiraIssueKeys, _logHelper)));
            //.ToArray();

            _directumJiraExchangeProvider.UpdateState(resultIssues);

            _logHelper.Info($"Обработка завершена за время {sw.Elapsed:ss\\.fff} сек.");
        }

        protected abstract string GetMessage(T issueByCreate);

        protected virtual async Task<CreateIssueResult> CreateIssue(T issueByCreateBase, string[] existJiraIssueKeys,
            LogHelper _logHelper)
        {
            var issueByCreate = issueByCreateBase;
            var result = new CreateIssueResult
            {
                AssignmentId = Convert.ToInt32(issueByCreate.SourceNumber),
                Project = PROJECT_ID
            };

            try
            {
                _logHelper.Info(GetMessage(issueByCreate));
                ValidateIssueByCreate(issueByCreate, existJiraIssueKeys);

                result.IssueKey = await CreateInJira(issueByCreate);
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

        protected abstract Task<string> CreateInJira(T item);
        protected abstract T[] GetIssues(DirectumIssuesFilter filter, DateTime dateCreation, LogHelper logHelper);
    }
}
