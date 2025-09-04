// Copyright (c) 2025 Car Wash Processor, All Rights Reserved.

using CarWashProcessor.Application.Abstractions.Resolution; // For IServiceResolver
using CarWashProcessor.Domain.Abstractions.Services;        // For IWashServiceStrategy
using CarWashProcessor.Models;                              // For CarJob, EServiceWash, EServiceAddon

namespace CarWashProcessor.Services;                        // Namespace for the service, and unrefactored services (TireShineService, InteriorCleanService, HandWaxAndShineService)

/// <summary>
/// Service responsible for processing car jobs by coordinating wash and addon services.
/// </summary>
public class CarJobProcessorService
{
    /// <summary>
    /// Resolver to obtain the appropriate wash service strategy based on the requested wash type.
    /// </summary>
    private readonly IServiceResolver<EServiceWash, IWashServiceStrategy> _washServiceResolver;

    /// <summary>
    /// Resolver to obtain the appropriate addon service strategy based on the requested addon type.
    /// </summary>
    private readonly IServiceResolver<EServiceAddon, IAddonServiceStrategy> _addonServiceResolver;

    /// <summary>
    /// Constructor for CarJobProcessorService.
    /// </summary>
    /// <param name="tireShineService">
    /// The tire shine service to be used for processing car jobs.
    /// </param>
    /// <param name="interiorCleanService">
    /// The interior clean service to be used for processing car jobs.
    /// </param>
    /// <param name="handWaxAndShineService">
    /// The hand wax and shine service to be used for processing car jobs.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any of the service parameters are null.
    /// </exception>
    public CarJobProcessorService(
        IServiceResolver<EServiceWash, IWashServiceStrategy> washResolver,
        IServiceResolver<EServiceAddon, IAddonServiceStrategy> addonResolver
    )
    {
        /* Defensive programming. 
         * Validate input parameters in constructor, or fast fail, followed by assignments. A
         * more concise way is to use a null-coalescing assignment:
         * 
         * e.g. _washServiceResolver = washResolver ?? throw new ArgumentNullException(nameof(washResolver));
         */

        ArgumentNullException.ThrowIfNull(washResolver, nameof(washResolver));
        ArgumentNullException.ThrowIfNull(addonResolver, nameof(addonResolver));

        // Assign service instances to private readonly fields.
        _washServiceResolver = washResolver;
        _addonServiceResolver = addonResolver;
    }

    /// <summary>
    /// Processes a car job by performing the requested wash service and any additional addons.
    /// </summary>
    /// <param name="carJob">
    /// The car job to be processed.
    /// </param>
    /// <param name="cancellationToken">
    /// Optional. Cancellation token to cancel the operation.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation of processing the car job.
    /// </returns>
    /// <remarks>
    /// Per the requirements, <see cref="Worker"/> was not modified, so the cancelation token is
    /// not passed, but included as an optional parameter for future extensibility.
    /// </remarks>
    public async Task ProcessCarJobAsync(CarJob carJob, CancellationToken cancellationToken = default)
    {
        // Defensive programming. 
        ArgumentNullException.ThrowIfNull(carJob, nameof(carJob));

        // Get the appropriate wash service (strategy) based on the requested wash service.
        var washStrategy = _washServiceResolver.Resolve(carJob.ServiceWash);

        // Perform the wash service.
        await washStrategy.PerformWashAsync(carJob, cancellationToken);

        // Step 2: Process any addons requested.

        /* TODO: Optimize ServiceAddons processing. 
         * Business (interviewer) requirements are not clear on how to handle duplicates or order of operations. 
         * Proceeding with a simple approach that handles duplicates and processes sequentially in order.
         * Future improvements could include:
         * - If order doesn't matter, this solution suffices
         * - If order matters, consider using a more complex structure (composer, runtime configuration in appSettings.json, etc.) to maintain order and handle duplicates.
         * - If duplicates should be processed multiple times, remove Distinct() and handle accordingly.
         */

        foreach (var addon in carJob.ServiceAddons.Distinct()) // O(n) time to deduplicate, O(m) space for distinct addons.
        {
            var addonStrategy = _addonServiceResolver.Resolve(addon); // O(1) time to resolve via keyed DI.
            await addonStrategy.PerformAddonAsync(carJob, cancellationToken); // Perform the addon.
        }
    }
}
