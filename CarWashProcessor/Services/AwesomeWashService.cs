// TODO: Consistent file header containing copyright.

using CarWashProcessor.Models;

namespace CarWashProcessor.Services;

/// <summary>
/// Service responsible for performing an awesome car wash.
/// </summary>
/// <remarks>
/// TODO: Implement IWashServiceStrategy (align with strategy pattern & DI).
/// Needs a common interface with other wash services. All *WashService classes have a single public method, 
/// accepting the CarJob. These are strategies for washing a car.
/// </remarks>
public class AwesomeWashService
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

    /// <summary>
    /// Does an awesome wash for the given car job.
    /// </summary>
    /// <param name="carJob">
    /// The car job for which the awesome wash is to be performed.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation of performing the awesome wash.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="carJob"/> parameter is null.
    /// </exception>
    public async Task DoAwesomeWashAsync(CarJob carJob)
	{
        // Defensive programming. Validate input parameters on public methods.
        ArgumentNullException.ThrowIfNull(carJob, nameof(carJob));

        // Wait a second (simulating wash type-specific work).
        await Task.Delay(TimeSpan.FromSeconds(1));

        // Log information
        /* TODO: Evaluate CA1848: Use LoggerMessage.Define to pre-define logging messages for better performance. 
         * This is a suggestion from code analysis, but for simplicity and readability in this example, we are 
         * using the straightforward approach as it can be argued this does not invalidate the requirement: 
         * "The system is reasonably performant". */
#pragma warning disable CA1848 // Use the LoggerMessage delegates
        _logger.LogInformation("--> Awesome wash performed for customer {CustomerId}!", carJob.CustomerId);
#pragma warning restore CA1848 // Use the LoggerMessage delegates
    }
}
