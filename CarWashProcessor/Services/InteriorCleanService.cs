// Copyright (c) 2025 Car Wash Processor, All Rights Reserved

using CarWashProcessor.Models;

namespace CarWashProcessor.Services;

/// <summary>
/// Service responsible for interior cleaning as an addon service.
/// </summary>
/// <remarks>
/// TODO: Each addon service has identical structure. Depending on requirements (execution order doesn't matter or it does) this is either a strategy or a command / step.
/// </remarks>
public class InteriorCleanService
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
        // Defensive programming, validate input parameters in constructor, or fast fail, followed by assignments.
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
	public async Task CleanInteriorAsync(CarJob carJob)
	{
        // Defensive programming. Validate input parameters on public methods.
        ArgumentNullException.ThrowIfNull(carJob, nameof(carJob));

        // Wait a second (simulating addon-specific work).
        await Task.Delay(TimeSpan.FromSeconds(1));

        // Log information
        /* TODO: Evaluate CA1848: Use LoggerMessage.Define to pre-define logging messages for better performance. 
         * This is a suggestion from code analysis, but for simplicity and readability in this example, we are 
         * using the straightforward approach as it can be argued this does not invalidate the requirement: 
         * "The system is reasonably performant". */
#pragma warning disable CA1848 // Use the LoggerMessage delegates
        _logger.LogInformation("--> Interior has been cleaned for customer {CustomerId}!", carJob.CustomerId);
#pragma warning restore CA1848 // Use the LoggerMessage delegates
    }
}
