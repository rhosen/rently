using Microsoft.Extensions.Logging;
using Rently.Shared.Logging;

namespace Rently.Shared.Services
{
    public class LogService : ILogService
    {
        private readonly ILogger<LogService> _logger;
        public LogService(ILogger<LogService> logger) => _logger = logger;

        public void LogInfo(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogError(string message, Exception? ex = null)
        {
            _logger.LogError(ex, message);
        }
    }
}
