// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ISL.Reidentification.Core.Brokers.Loggings
{
    internal class LoggingBroker : ILoggingBroker
    {
        private readonly ILogger<LoggingBroker> logger;

        public LoggingBroker(ILogger<LoggingBroker> logger) =>
            this.logger = logger;

        public async ValueTask LogInformationAsync(string message) =>
            logger.LogInformation(message);

        public async ValueTask LogTraceAsync(string message) =>
            logger.LogTrace(message);

        public async ValueTask LogDebugAsync(string message) =>
            logger.LogDebug(message);

        public async ValueTask LogWarningAsync(string message) =>
            logger.LogWarning(message);

        public async ValueTask LogErrorAsync(Exception exception) =>
            logger.LogError(exception, exception.Message);

        public async ValueTask LogCriticalAsync(Exception exception) =>
            logger.LogCritical(exception, exception.Message);
    }
}