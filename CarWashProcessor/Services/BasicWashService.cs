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
    public BasicWashService(ILogger<BasicWashService> logger)
	{
        // Set services
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
    public async Task DoBasicWashAsync(CarJob carJob)
	{
        // TODO: Bug. Defensive programming, validate input parameters on public methods. (need this for all public methods)

        // Wait a second (simulating wash type-specific work).
        await Task.Delay(TimeSpan.FromSeconds(1));

		// Log information
        // TODO: BUG - Structured logging without named parameters. Named templates are queryable in KQL.
		_logger.LogInformation("--> Basic wash performed for customer {}!", carJob.CustomerId);
	}
}
