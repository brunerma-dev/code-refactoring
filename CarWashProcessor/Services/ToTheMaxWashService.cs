using CarWashProcessor.Models;

namespace CarWashProcessor.Services;

/// <summary>
/// Service responsible for performing a "To The Max" car wash.
/// </summary>
/// <remarks>
/// TODO: Implement IWashServiceStrategy (align with strategy pattern & DI).
/// Needs a common interface with other wash services. All *WashService classes have a single public method, 
/// accepting the CarJob. These are strategies for washing a car.
/// </remarks>
public class ToTheMaxWashService
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
	public ToTheMaxWashService(ILogger<ToTheMaxWashService> logger)
	{
		// Set services
		_logger = logger; // TODO: Fast fail if dependency injection passed a null constructor parameter. (Need this across the board.)
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
    public async Task DoToTheMaxWashAsync(CarJob carJob)
	{
        // TODO: Bug. Defensive programming, validate input parameters on public methods. (need this for all public methods)

        // Wait a second (simulating wash type-specific work).
        await Task.Delay(TimeSpan.FromSeconds(1));

        // Log information
        // TODO: BUG - Structured logging without named parameters. Named templates are queryable in KQL.
        _logger.LogInformation("--> To The Max wash performed for customer {}!", carJob.CustomerId);
	}
}
