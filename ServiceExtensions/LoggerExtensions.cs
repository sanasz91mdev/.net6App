namespace DigitalBanking.ServiceExtensions
{
    using System;
    using Microsoft.Extensions.Logging;


    public static class LoggerExtensions
    {
        private static Action<ILogger, string, string, Exception> _information;

        private static Action<ILogger, string, string, Exception> _error;

        private static Action<ILogger, string, string, Exception> _debug;

         static LoggerExtensions()
        {
            _information = LoggerMessage.Define<string, string>(
                LogLevel.Information,
                new EventId(1, "'{module}'"),
                "module: '{module}' Message = {message})");


            _error = LoggerMessage.Define<string, string>(
                LogLevel.Error,
                new EventId(1, "'{module}'"),
                "module: '{module}' Message = {message})");

            _debug = LoggerMessage.Define<string, string>(
                LogLevel.Debug, new EventId(1, "'{module}'"),
                "module: '{module}' Message = {message})");

        }

        public static void LogInfo(this ILogger logger, string module, string message, Exception ex = null)
        {
            _information(logger, module, message, ex);
        }

        public static void LogDebug(this ILogger logger, string module, string message, Exception ex = null)
        {
            _debug(logger, module, message, null);
        }

        public static void LogError(this ILogger logger, string module, string message, Exception ex = null)
        {
            _error(logger, module, message, null);
        }
    }
}

