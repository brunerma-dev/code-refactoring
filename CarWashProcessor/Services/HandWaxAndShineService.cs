// TODO: Consistent file header containing copyright.

using CarWashProcessor.Models;

namespace CarWashProcessor.Services;

/// <summary>
/// Service responsible for hand waxing and shining as an addon service.
/// </summary>
/// <remarks>
/// TODO: Each addon service has identical structure. Depending on requirements (execution order doesn't matter or it does) this is either a strategy or a command / step.
/// </remarks>
public class HandWaxAndShineService
{
    /// <summary>
    /// Logger instance for logging information related to the HandWaxAndShineService.
    /// </summary>
    private readonly ILogger<HandWaxAndShineService> _logger;

    /// <summary>
    /// Constructor for HandWaxAndShineService.
    /// </summary>
    /// <param name="logger">
	/// Logger instance for logging information related to the HandWaxAndShineService.
	/// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="logger"/> parameter is null.
    /// </exception>
    public HandWaxAndShineService(ILogger<HandWaxAndShineService> logger)
	{
        // Defensive programming, validate input parameters in constructor, or fast fail, followed by assignments.
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        // Set services
        _logger = logger;
    }

    /// <summary>
    /// Performs hand waxing and shining for the given car job.
    /// </summary>
    /// <param name="carJob">
    /// The car job for which hand waxing and shining is to be performed.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation of hand waxing and shining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="carJob"/> parameter is null.
    /// </exception>
    public async Task HandWaxAndShineAsync(CarJob carJob)
	{
        // Defensive programming. Validate input parameters on public methods.
        ArgumentNullException.ThrowIfNull(carJob, nameof(carJob));

        // Wait a second (simulating addon-specific work).
        await Task.Delay(TimeSpan.FromSeconds(1));

        // Log information
        // TODO: BUG - Structured logging without named parameters. Named templates are queryable in KQL.
        _logger.LogInformation("--> Hand waxed and shined for customer {}!", carJob.CustomerId);
	}
}
