namespace Todo.Api.Configurations;

internal static class Log
{
    private static readonly Action<ILogger, string, Exception?> MigrationError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(1, nameof(DatabaseMigrationError)),
            "This error occurred while migrating the database {Error}");

    public static void DatabaseMigrationError(ILogger logger, Exception exception)
    {
        MigrationError(logger, exception.Message, exception);
    }
}