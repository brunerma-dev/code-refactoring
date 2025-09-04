// Copyright (c) 2025 Car Wash Processor, All Rights Reserved.

using CarWashProcessor.Application.Abstractions.Registration;   // For WashTypeAttribute
using CarWashProcessor.Domain.Abstractions.Services;            // For IWashServiceStrategy
using CarWashProcessor.Logging;                                 // For AppLog
using CarWashProcessor.Models;                                  // For CarJob, EServiceWash

namespace CarWashProcessor.Application.Strategies.Wash;

/// <summary>
/// Service responsible for performing a basic car wash.
/// </summary>
[WashType(EServiceWash.Basic)]  // Facilitates automatic registration and resolution of this service for the Basic wash type.
public class BasicWashService : IWashServiceStrategy
{
    /// <summary>
    /// Logger instance for logging information related to the BasicWashService.
    /// </summary>
    private readonly ILogger<BasicWashService> _logger;

    /// <summary>
    /// Constructor for BasicWashService.
    /// </summary>
    /// <param name="logger">
	/// Logger instance for logging information related to the BasicWashService.
	/// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="logger"/> parameter is null.
    /// </exception>
    public BasicWashService(ILogger<BasicWashService> logger)
	{
        // Defensive programming.
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        // Set services
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task PerformWashAsync(CarJob carJob, CancellationToken cancellationToken = default)
    {
        // Defensive programming.
        ArgumentNullException.ThrowIfNull(carJob, nameof(carJob));

        // Wait a second (simulating wash type-specific work).
        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

        // Log information
        AppLog.WashCompleted(_logger, carJob.ServiceWash, carJob.CustomerId);
    }
}
