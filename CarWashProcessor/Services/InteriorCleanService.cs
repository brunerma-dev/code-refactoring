// TODO: Consistent file header containing copyright.

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
    public InteriorCleanService(ILogger<InteriorCleanService> logger)
	{
        // Set services
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
	public async Task CleanInteriorAsync(CarJob carJob)
	{
        // TODO: Bug. Defensive programming, validate input parameters on public methods. (need this for all public methods)

        // Wait a second (simulating addon-specific work).
        await Task.Delay(TimeSpan.FromSeconds(1));

        // Log information
        // TODO: BUG - Structured logging without named parameters. Named templates are queryable in KQL.
        _logger.LogInformation("--> Interior has been cleaned for customer {}!", carJob.CustomerId);
	}
}
