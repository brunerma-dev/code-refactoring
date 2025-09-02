// TODO: Consistent file header containing copyright.

using CarWashProcessor.Models;

namespace CarWashProcessor.Services;

/// <summary>
/// Service responsible for performing a basic car wash.
/// </summary>
/// <remarks>
/// TODO: Implement IWashServiceStrategy (align with strategy pattern & DI).
/// Needs a common interface with other wash services. All *WashService classes have a single public method, 
/// accepting the CarJob. These are strategies for washing a car.
/// </remarks>
public class BasicWashService
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
        // Defensive programming, validate input parameters in constructor, or fast fail, followed by assignments.
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        // Set services
        _logger = logger;
    }

    /// <summary>
    /// Does a basic wash for the given car job.
    /// </summary>
    /// <param name="carJob">
    /// The car job for which the basic wash is to be performed.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation of performing the basic wash.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="carJob"/> parameter is null.
    /// </exception>
    public async Task DoBasicWashAsync(CarJob carJob)
	{
        // Defensive programming. Validate input parameters on public methods.
        ArgumentNullException.ThrowIfNull(carJob, nameof(carJob));

        // Wait a second (simulating wash type-specific work).
        await Task.Delay(TimeSpan.FromSeconds(1));

		// Log information
        // TODO: BUG - Structured logging without named parameters. Named templates are queryable in KQL.
		_logger.LogInformation("--> Basic wash performed for customer {}!", carJob.CustomerId);
	}
}
