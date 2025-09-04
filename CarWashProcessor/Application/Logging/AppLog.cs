// Copyright (c) 2025 Car Wash Processor, All Rights Reserved.

using CarWashProcessor.Models;

namespace CarWashProcessor.Logging
{
    /// <summary>
    /// Centralized logging class for the Car Wash Processor Application Layer.
    /// </summary>
    public static partial class AppLog
    {
        [LoggerMessage(
            EventId = 1000,
            Level = LogLevel.Information,
            Message = "--> {WashType} wash performed for customer {CustomerId}!"
        )]
        public static partial void WashCompleted(ILogger logger, EServiceWash washType, long customerId);

        [LoggerMessage(
            EventId = 2000,
            Level = LogLevel.Information,
            Message = "--> {AddonType} addon performed for customer {CustomerId}!"
        )]
        public static partial void AddonCompleted(ILogger logger, EServiceAddon addonType, long customerId);
    }
}
