using DirectumToJira.libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira
{
    class DirectumRegistryChangeStrategy : DirectumIssueStrategyBase<ItemByCreate>
    {
        public DirectumRegistryChangeStrategy(IJiraProvider jiraProvider, IDirectumJiraExchangeProvider directumJiraExchangeProvider,
            DirectumToJiraMapper directumToJiraMapper)
            : base(jiraProvider, directumJiraExchangeProvider, directumToJiraMapper)
        {
        }

        protected override string PROJECT_ID => "CH";


        protected override ItemByCreate[] GetIssues(DirectumIssuesFilter _filter, DateTime _dateCreation, LogHelper _logHelper)
        {
            return _directumJiraExchangeProvider
                .GetRegistryItems(_filter)
                .Where(item => item.Deadline.Date != _dateCreation.Date)
                .Select(directumRegistryItem => _directumToJiraMapper.MapItem(directumRegistryItem, _logHelper))
                .ToArray();
        }

        protected override string GetMessage(ItemByCreate issueByCreate)
        {
            return $"{issueByCreate.Summary} DeadLine:{issueByCreate.DeadLine} Project:{issueByCreate.SourceNumber}";
        }

        protected override Task<string> CreateInJira(ItemByCreate item)
        {
            return _jiraProvider.CreateRegistryItemAsync(item);
        }

        protected override void ValidateIssueByCreate(ItemByCreate issue, string[] existJiraIssueKeys)
        {
            base.ValidateIssueByCreate(issue, existJiraIssueKeys);

            if (string.IsNullOrWhiteSpace(issue.Summary))
                throw new ArgumentException($"Тема не определена");

            if (string.IsNullOrWhiteSpace(issue.IssueType))
                throw new ArgumentException($"Тип запроса не определён");

            if (string.IsNullOrWhiteSpace(issue.ChangeManager))
                throw new ArgumentException($"Менеджер изменения не определён");

            if (string.IsNullOrWhiteSpace(issue.ConsequencesFailureImplement))
                throw new ArgumentException($"Последствия от невнедрения не определены");

            if (string.IsNullOrWhiteSpace(issue.Description))
                throw new ArgumentException($"Описание не определено");

            if (string.IsNullOrWhiteSpace(issue.Priority))
                throw new ArgumentException($"Приоритет инициатора не определён");

            if (string.IsNullOrWhiteSpace(issue.DepartmentId))
                throw new ArgumentException($"Подразделение ИТ не определено");

            if (string.IsNullOrWhiteSpace(issue.Initiator))
                throw new ArgumentException($"Инициатор не определён");

            if (string.IsNullOrWhiteSpace(issue.ReasonImplementation))
                throw new ArgumentException($"Основание не определено");
        }
    }
}
