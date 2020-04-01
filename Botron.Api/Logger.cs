using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Botron.Api
{
    public static class Logger
    {
        static ILogger logger;


        public static void InitializeDevelopment(string filename)
        {
            logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File($"Logs/{filename}-{{Date}}.log")
                .CreateLogger();
        }

        public static void InitializeProduction(string filename)
        {
            logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File($"Logs/{filename}-{{Date}}.log")
                .CreateLogger();
        }


        public static void Debug(string text)
        {
            logger.Debug(text);
        }

        public static void Debug(string format, params object[] args)
        {
            logger.Debug(format, args);
        }

        public static void Information(string text)
        {
            logger.Information(text);
        }

        public static void Information(string format, params object[] args)
        {
            logger.Information(format, args);
        }

        public static void Warning(string text)
        {
            logger.Warning(text);
        }

        public static void Warning(string format, params object[] args)
        {
            logger.Warning(format, args);
        }

        public static void Warning(Exception error, string message = null, params object[] args)
        {
            logger.Warning(error, message, args);
        }

        public static void Error(string text)
        {
            logger.Error(text);
        }

        public static void Error(string format, params object[] args)
        {
            logger.Error(format, args);
        }

        public static void Error(Exception error, string message = null, params object[] args)
        {
            logger.Error(error, message, args);
        }
    }
}
