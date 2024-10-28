

namespace CodeBridgeTestTask.Application.Logging
{
    public interface ILoggerService
    {
        void LogInformation(string message);
        void LogError(string message, Exception ex);
        
    }
}
