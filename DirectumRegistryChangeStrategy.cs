using DirectumToJira.libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira
{
    class DirectumRegistryChangeStrategy : DirectumIssueStrategyImport
    {
        private const string PROJECT_ID = "CH";

        private readonly DirectumToJiraMapper _directumToJiraMapper;

        public DirectumRegistryChangeStrategy(IJiraProvider jiraProvider, IDirectumJiraExchangeProvider directumJiraExchangeProvider,
            DirectumToJiraMapper directumToJiraMapper)
            : base(jiraProvider, directumJiraExchangeProvider)
        {
            _directumToJiraMapper = directumToJiraMapper;
        }
        protected override void GetIssuesFromDb(DirectumIssuesFilter _filter, DateTime _dateCreation, LogHelper _logHelper)
        {
            _logHelper.Info($"Начало обработки {DateTime.Now}");

            IssuesByCreateBase = _directumJiraExchangeProvider
                                .GetRegistryItems(_filter)
                                .Where(item => item.Deadline.Date != _dateCreation.Date)
                                .Select(directumRegistryItem => _directumToJiraMapper.MapItem(directumRegistryItem, _logHelper))
                                .ToArray();

            _logHelper.Info($"Получено задач реестра изменений на {_filter.EndDate}: {IssuesByCreateBase.Count()}");
        }


        private void ValidateItemByCreate(ItemByCreate itemByCreate, string[] existJiraIssueKeys)
        {
            ValidateIssueByCreate(itemByCreate, existJiraIssueKeys);

            if (string.IsNullOrWhiteSpace(itemByCreate.Summary))
                throw new ArgumentException($"Тема не определена");

            if (string.IsNullOrWhiteSpace(itemByCreate.IssueType))
                throw new ArgumentException($"Тип запроса не определён");

            if (string.IsNullOrWhiteSpace(itemByCreate.ChangeManager))
                throw new ArgumentException($"Менеджер изменения не определён");

            if (string.IsNullOrWhiteSpace(itemByCreate.ConsequencesFailureImplement))
                throw new ArgumentException($"Последствия от невнедрения не определены");

            if (string.IsNullOrWhiteSpace(itemByCreate.Description))
                throw new ArgumentException($"Описание не определено");

            if (string.IsNullOrWhiteSpace(itemByCreate.Priority))
                throw new ArgumentException($"Приоритет инициатора не определён");

            if (string.IsNullOrWhiteSpace(itemByCreate.DepartmentId))
                throw new ArgumentException($"Подразделение ИТ не определено");

            if (string.IsNullOrWhiteSpace(itemByCreate.Initiator))
                throw new ArgumentException($"Инициатор не определён");

            if (string.IsNullOrWhiteSpace(itemByCreate.ReasonImplementation))
                throw new ArgumentException($"Основание не определено");
        }
        protected override CreateIssueResult CreateIssue(IssueByCreateBase issueByCreateBase, string[] existJiraRegistryItemKeys, LogHelper _logHelper)
        {
            var itemByCreate = (ItemByCreate)issueByCreateBase;

            var result = new CreateIssueResult
            {
                AssignmentId = Convert.ToInt32(itemByCreate.SourceNumber),
                Project = PROJECT_ID
            };


            try
            {
                _logHelper.Info($"{itemByCreate.Summary} DeadLine:{itemByCreate.DeadLine} Project:{itemByCreate.SourceNumber}");
                ValidateItemByCreate(itemByCreate, existJiraRegistryItemKeys);

                result.IssueKey = _jiraProvider.CreateRegistryItem(itemByCreate);
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
