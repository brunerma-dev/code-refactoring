// Copyright (c) 2025 Car Wash Processor, All Rights Reserved.

using CarWashProcessor.Application.Abstractions.Registration;   // For WashTypeAttribute
using CarWashProcessor.Domain.Abstractions.Services;            // For IWashServiceStrategy
using CarWashProcessor.Models;                                  // For CarJob, EServiceWash

namespace CarWashProcessor.Application.Strategies.Wash;

/// <summary>
/// Service responsible for performing a "To The Max" car wash.
/// </summary>
[WashType(EServiceWash.ToTheMax)]   // Facilitates automatic registration and resolution of this service for the ToTheMax wash type.
public class ToTheMaxWashService : IWashServiceStrategy
{
    /// <summary>
    /// Logger instance for logging information related to the ToTheMaxWashService.
    /// </summary>
    private readonly ILogger<ToTheMaxWashService> _logger;

    /// <summary>
    /// Constructor for ToTheMaxWashService.
    /// </summary>
    /// <param name="logger">
    /// Logger instance for logging information related to the ToTheMaxWashService.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="logger"/> parameter is null.
    /// </exception>
	public ToTheMaxWashService(ILogger<ToTheMaxWashService> logger)
	{
        // Defensive programming.
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        // Set services
        _logger = logger;
    }

    /// <summary>
    /// Does a "To The Max" wash for the given car job.
    /// </summary>
    /// <param name="carJob">
    /// The car job for which the "To The Max" wash is to be performed.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation of performing the "To The Max" wash.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="carJob"/> parameter is null.
    /// </exception>
    public async Task DoToTheMaxWashAsync(CarJob carJob) => await PerformWashAsync(carJob);

    /// <inheritdoc />
    public async Task PerformWashAsync(CarJob carJob, CancellationToken cancellationToken = default)
    {
        // Defensive programming. Validate input parameters on public methods.
        ArgumentNullException.ThrowIfNull(carJob, nameof(carJob));

        // Wait a second (simulating wash type-specific work).
        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

        // Log information
        _logger.LogInformation("--> To The Max wash performed for customer {CustomerId}!", carJob.CustomerId);
    }
}
