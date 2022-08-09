using Microsoft.Extensions.Logging;

namespace ELfR.Communications.Templates.Text.ArgumentPlaceholder;

public partial class TextArgumentPlaceholderTemplateService
{
    private enum LogEvents
    {
        Initialized =
            0x00000000,
        HydrateStarted =
            0x00000100,
        HydrateError =
            0x000001FE,
        HydrateFinished =
            0x000001FF
    }

    private static readonly Action<ILogger, Exception?> LogInitialized =
        LoggerMessage.Define(LogLevel.Debug, new EventId((int)LogEvents.Initialized, nameof(LogEvents.Initialized)), "Service initialized.");

    private static readonly Action<ILogger, Exception?> LogHydrateStarted =
        LoggerMessage.Define(LogLevel.Debug, new EventId((int)LogEvents.HydrateStarted, nameof(LogEvents.HydrateStarted)), "Hydrate started.");

    private static readonly Action<ILogger, Exception?> LogHydrateError =
        LoggerMessage.Define(LogLevel.Error, new EventId((int)LogEvents.HydrateError, nameof(LogEvents.HydrateError)), "An unexpected error occurred during the hydration of the text template.");

    private static readonly Action<ILogger, Exception?> LogHydrateFinished =
        LoggerMessage.Define(LogLevel.Debug, new EventId((int)LogEvents.HydrateFinished, nameof(LogEvents.HydrateFinished)), "Hydrate finished.");
}