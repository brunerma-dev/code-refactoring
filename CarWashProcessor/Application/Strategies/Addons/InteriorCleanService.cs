// Copyright (c) 2025 Car Wash Processor, All Rights Reserved.

using CarWashProcessor.Application.Abstractions.Registration;   // For AddonTypeAttribute
using CarWashProcessor.Domain.Abstractions.Services;            // For IAddonServiceStrategy
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

    /// <summary>
    /// Cleans the interior for the given car job.
    /// </summary>
    /// <param name="carJob">
    /// The car job for which the interior is to be cleaned.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation of cleaning the interior.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="carJob"/> parameter is null.
    /// </exception>
	public async Task CleanInteriorAsync(CarJob carJob) => await PerformAddonAsync(carJob);

    /// <inheritdoc />
    public async Task PerformAddonAsync(CarJob carJob, CancellationToken cancellationToken = default)
    {
        // Defensive programming.
        ArgumentNullException.ThrowIfNull(carJob, nameof(carJob));

        // Wait a second (simulating addon-specific work).
        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

        // Log information
        _logger.LogInformation("--> Interior has been cleaned for customer {CustomerId}!", carJob.CustomerId);
    }
}
