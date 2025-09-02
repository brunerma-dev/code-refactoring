// TODO: Consistent file header containing copyright.

using CarWashProcessor.Models;

namespace CarWashProcessor.Services;

/// <summary>
/// Service responsible for shining tires as an addon service.
/// </summary>
/// <remarks>
/// TODO: Each addon service has identical structure. Depending on requirements (execution order doesn't matter or it does) this is either a strategy or a command / step.
/// </remarks>
public class TireShineService
{
    /// <summary>
    /// Logger instance for logging information related to the TireShineService.
    /// </summary>
    private readonly ILogger<TireShineService> _logger;

    /// <summary>
    /// Constructor for TireShineService.
    /// </summary>
    /// <param name="logger">
    /// Logger instance for logging information related to the TireShineService.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="logger"/> parameter is null.
    /// </exception>
	public TireShineService(ILogger<TireShineService> logger)
	{
        // Defensive programming, validate input parameters in constructor, or fast fail, followed by assignments.
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        // Set services
        _logger = logger;
    }

    /// <summary>
    /// Shines the tires for the given car job.
    /// </summary>
    /// <param name="carJob">
    /// The car job for which the tires are to be shined.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation of shining the tires.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="carJob"/> parameter is null.
    /// </exception>
	public async Task ShineTiresAsync(CarJob carJob)
	{
        // Defensive programming. Validate input parameters on public methods.
        ArgumentNullException.ThrowIfNull(carJob, nameof(carJob));

        // Wait a second (simulating addon-specific work).
        await Task.Delay(TimeSpan.FromSeconds(1));

        // Log information
        // TODO: BUG - Structured logging without named parameters. Named templates are queryable in KQL.
        _logger.LogInformation("--> Tires have been shined for customer {}!", carJob.CustomerId);
	}
}
