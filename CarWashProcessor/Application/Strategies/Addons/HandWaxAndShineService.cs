// Copyright (c) 2025 Car Wash Processor, All Rights Reserved.

using CarWashProcessor.Application.Abstractions.Registration;   // For AddonTypeAttribute
using CarWashProcessor.Domain.Abstractions.Services;            // For IAddonServiceStrategy
using CarWashProcessor.Logging;                                 // For AppLog
using CarWashProcessor.Models;                                  // For CarJob, EServiceAddon

namespace CarWashProcessor.Application.Strategies.Addons;

/// <summary>
/// Service (Addon Strategy) responsible for hand waxing and shining as an addon service.
/// </summary>
[AddonType(EServiceAddon.HandWaxAndShine)]
public class HandWaxAndShineService : IAddonServiceStrategy
{
    /// <summary>
    /// Logger instance for logging information related to the HandWaxAndShineService.
    /// </summary>
    private readonly ILogger<HandWaxAndShineService> _logger;

    /// <summary>
    /// Constructor for HandWaxAndShineService.
    /// </summary>
    /// <param name="logger">
	/// Logger instance for logging information related to the HandWaxAndShineService.
	/// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="logger"/> parameter is null.
    /// </exception>
    public HandWaxAndShineService(ILogger<HandWaxAndShineService> logger)
	{
        // Defensive programming.
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        // Set services
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task PerformAddonAsync(CarJob carJob, CancellationToken cancellationToken = default)
    {
        // Defensive programming. Validate input parameters on public methods.
        ArgumentNullException.ThrowIfNull(carJob, nameof(carJob));

        // Wait a second (simulating addon-specific work).
        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken).ConfigureAwait(false);

        // Log information
        AppLog.AddonCompleted(_logger, EServiceAddon.HandWaxAndShine, carJob.CustomerId);
    }

}
