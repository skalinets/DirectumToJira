using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira.libraries
{
    class LogHelper
    {
        private readonly ILogger _logger;
        private readonly StringBuilder _builder = new StringBuilder();
        public LogHelper(ILogger logger)
        {
            this._logger = logger;
        }
        public void Debug(string message, params object[] args)
        {
            var res = string.Format(CultureInfo.CurrentUICulture, message, args);
            _logger.Debug(res, CultureInfo.CurrentUICulture);
            _builder.AppendLine(res);
        }

        public void Warn(string message, params object[] args)
        {
            var res = string.Format(CultureInfo.CurrentUICulture, message, args);
            _logger.Warn(res, CultureInfo.CurrentUICulture);
            _builder.AppendLine("* " + res);
        }
        public void Error(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }
            _logger.Error(exception);
            _builder.AppendLine("! ERROR Happened");
            _builder.AppendLine("! " + exception.Message);
        }

        public string Result => _builder.ToString();

        public void Info(string message, params object[] args)
        {
            var res = string.Format(CultureInfo.CurrentUICulture, message, args);
            _logger.Info(res, CultureInfo.CurrentUICulture);
            _builder.AppendLine(res);
        }
    }
}
