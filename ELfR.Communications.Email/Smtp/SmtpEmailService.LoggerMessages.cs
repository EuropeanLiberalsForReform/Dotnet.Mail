using Microsoft.Extensions.Logging;

namespace ELfR.Communications.Email.Smtp
{
    public partial class SmtpEmailService
    {
        private enum LogEvents
        {
            Initialized =
                0x00000000,
            SendStart =
                0x00000100,
            SendError =
                0x000001FE,
            SendFinish =
                0x000001FF,
            SendBulkStart =
                0x00000200,
            SendBulkError =
                0x000002FE,
            SendBulkFinish =
                0x000002FF
        }

        private static readonly Action<ILogger, Exception?> LogInitialized =
            LoggerMessage.Define(LogLevel.Debug, new EventId((int)LogEvents.Initialized, nameof(LogEvents.Initialized)), "SMTP e-mail service initialized.");

        private static readonly Action<ILogger, Exception?> LogSendStart =
            LoggerMessage.Define(LogLevel.Debug, new EventId((int)LogEvents.SendStart, nameof(LogEvents.SendStart)), "Started sending e-mail.");

        private static readonly Action<ILogger, Exception?> LogSendError =
            LoggerMessage.Define(LogLevel.Error, new EventId((int)LogEvents.SendError, nameof(LogEvents.SendError)), "Error sending e-mail.");

        private static readonly Action<ILogger, Exception?> LogSendFinish =
            LoggerMessage.Define(LogLevel.Debug, new EventId((int)LogEvents.SendFinish, nameof(LogEvents.SendFinish)), "Finished sending e-mail.");

        private static readonly Action<ILogger, Exception?> LogSendBulkStart =
            LoggerMessage.Define(LogLevel.Debug, new EventId((int)LogEvents.SendBulkStart, nameof(LogEvents.SendBulkStart)), "Started sending e-mail bulk.");

        private static readonly Action<ILogger, Exception?> LogSendBulkError =
            LoggerMessage.Define(LogLevel.Error, new EventId((int)LogEvents.SendBulkError, nameof(LogEvents.SendBulkError)), "Error sending e-mail bulk.");

        private static readonly Action<ILogger, Exception?> LogSendBulkFinish =
            LoggerMessage.Define(LogLevel.Debug, new EventId((int)LogEvents.SendBulkFinish, nameof(LogEvents.SendBulkFinish)), "Finished sending e-mail bulk.");
    }
}