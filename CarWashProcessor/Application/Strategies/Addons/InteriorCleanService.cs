// Copyright (c) 2025 Car Wash Processor, All Rights Reserved.

using CarWashProcessor.Application.Abstractions.Registration;   // For AddonTypeAttribute
using CarWashProcessor.Domain.Abstractions.Services;            // For IAddonServiceStrategy
using CarWashProcessor.Logging;                                 // For AppLog
using CarWashProcessor.Models;                                  // For CarJob, EServiceAddon    

namespace CarWashProcessor.Application.Strategies.Addons;

/// <summary>
/// Service responsible for interior cleaning as an addon service.
/// </summary>
[AddonType(EServiceAddon.InteriorClean)]
public class InteriorCleanService : IAddonServiceStrategy
{
    /// <summary>
    /// Logger instance for logging information related to the InteriorCleanService.
    /// </summary>
    private readonly ILogger<InteriorCleanService> _logger;

    /// <summary>
    /// Constructor for InteriorCleanService.
    /// </summary>
    /// <param name="logger">
    /// Logger instance for logging information related to the InteriorCleanService.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="logger"/> parameter is null.
    /// </exception>
    public InteriorCleanService(ILogger<InteriorCleanService> logger)
	{
        // Defensive programming.
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        // Set services
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task PerformAddonAsync(CarJob carJob, CancellationToken cancellationToken = default)
    {
        // Defensive programming.
        ArgumentNullException.ThrowIfNull(carJob, nameof(carJob));

        // Wait a second (simulating addon-specific work).
        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

        // Log information
        AppLog.AddonCompleted(_logger, EServiceAddon.InteriorClean, carJob.CustomerId);
    }
}
