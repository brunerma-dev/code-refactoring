// Copyright (c) 2025 Car Wash Processor, All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using CarWashProcessor.Models;

namespace CarWashProcessor.Logging
{
    public static partial class AppLog
    {
        [LoggerMessage(
            EventId = 1000,
            Level = LogLevel.Information,
            Message = "--> {WashType} performed for customer {CustomerId}!"
        )]
        public static partial void WashCompleted(ILogger logger, EServiceWash washType, long customerId);

        [LoggerMessage(
            EventId = 2000,
            Level = LogLevel.Information,
            Message = "--> {AddonType} for customer {CustomerId}!"
        )]
        public static partial void AddonCompleted(ILogger logger, EServiceAddon addonType, long customerId);
    }
}
