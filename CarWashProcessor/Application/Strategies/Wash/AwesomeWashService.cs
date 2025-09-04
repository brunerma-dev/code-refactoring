// Copyright (c) 2025 Car Wash Processor, All Rights Reserved.

using CarWashProcessor.Application.Abstractions.Registration;   // For WashTypeAttribute
using CarWashProcessor.Domain.Abstractions.Services;            // For IWashServiceStrategy
using CarWashProcessor.Logging;
using CarWashProcessor.Models;                                  // For CarJob, EServiceWash

namespace CarWashProcessor.Application.Strategies.Wash;

/// <summary>
/// Service responsible for performing an awesome car wash.
/// </summary>
[WashType(EServiceWash.Awesome)]    // Facilitates automatic registration and resolution of this service for the Awesome wash type.
public class AwesomeWashService : IWashServiceStrategy
{
    /// <summary>
    /// Logger instance for logging information related to the AwesomeWashService.
    /// </summary>
    private readonly ILogger<AwesomeWashService> _logger;

    /// <summary>
    /// Constructor for AwesomeWashService.
    /// </summary>
    /// <param name="logger">
	/// Logger instance for logging information related to the AwesomeWashService.
	/// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="logger"/> parameter is null.
    /// </exception>
    public AwesomeWashService(ILogger<AwesomeWashService> logger)
	{
        // Defensive programming, validate input parameters in constructor, or fast fail, followed by assignments.
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        // Set services
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task PerformWashAsync(CarJob carJob, CancellationToken cancellationToken = default)
    {
        // Defensive programming. Validate input parameters on public methods.
        ArgumentNullException.ThrowIfNull(carJob, nameof(carJob));

        // Wait a second (simulating wash type-specific work).
        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

        // Log information
        AppLog.WashCompleted(_logger, carJob.ServiceWash, carJob.CustomerId);
    }
}
