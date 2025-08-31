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
    /// Logger instance for logging information related to the BasicWashService.
    /// </summary>
    private readonly ILogger<AwesomeWashService> _logger;

    /// <summary>
    /// Constructor for AwesomeWashService.
    /// </summary>
    /// <param name="logger">
	/// Logger instance for logging information related to the BasicWashService.
	/// </param>
    public AwesomeWashService(ILogger<AwesomeWashService> logger)
	{
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
    public async Task DoAwesomeWashAsync(CarJob carJob)
	{
        // Wait a second (simulating wash type-specific work).
        await Task.Delay(TimeSpan.FromSeconds(1));

        // Log information
        // TODO: BUG - Structured logging without named parameters. Named templates are queryable in KQL.
        _logger.LogInformation("--> Awesome wash performed for customer {}!", carJob.CustomerId);
	}
}
