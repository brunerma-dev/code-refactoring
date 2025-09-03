// Copyright (c) 2025 Car Wash Processor, All Rights Reserved.

using CarWashProcessor.Application.Abstractions.Resolution; // For IServiceResolver
using CarWashProcessor.Domain.Abstractions.Services;        // For IWashServiceStrategy
using CarWashProcessor.Models;                              // For CarJob, EServiceWash, EServiceAddon

namespace CarWashProcessor.Services;                        // Namespace for the service, and unrefactored services (TireShineService, InteriorCleanService, HandWaxAndShineService)

public class CarJobProcessorService
{
    /// <summary>
    /// Resolver to obtain the appropriate wash service strategy based on the requested wash type.
    /// </summary>
    private readonly IServiceResolver<EServiceWash, IWashServiceStrategy> _washServiceResolver;

    private readonly TireShineService _tireShineService;
    private readonly InteriorCleanService _interiorCleanService;
    private readonly HandWaxAndShineService _handWaxAndShineService;

    // TODO: DIP. Use interfaces instead of concrete classes, group comparable services into collections to reduce constructor parameters.

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
        TireShineService tireShineService,
        InteriorCleanService interiorCleanService,
        HandWaxAndShineService handWaxAndShineService
    )
    {
        /* Defensive programming. 
         * Validate input parameters in constructor, or fast fail, followed by assignments. A
         * more concise way is to use a null-coalescing assignment:
         * 
         * e.g. _washServiceResolver = washResolver ?? throw new ArgumentNullException(nameof(washResolver));
         */

        ArgumentNullException.ThrowIfNull(washResolver, nameof(washResolver));
        ArgumentNullException.ThrowIfNull(tireShineService, nameof(tireShineService));
        ArgumentNullException.ThrowIfNull(interiorCleanService, nameof(interiorCleanService));
        ArgumentNullException.ThrowIfNull(handWaxAndShineService, nameof(handWaxAndShineService));

        // Assign service instances to private readonly fields.
        _washServiceResolver = washResolver;
        _tireShineService = tireShineService;
        _interiorCleanService = interiorCleanService;
        _handWaxAndShineService = handWaxAndShineService;
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
         * These could also be strategies with a resolver/factory, but does the processing order matter? This represents real-world 
         * actions, so order might be required (pipeline, command, etc.). If order doesn't matter, we could do them in parallel, but 
         * that doesn't map to the real-world. We could do them sequentially, with a foreach loop, but that doesn't consider order, 
         * or duplicate add-ons (should be handled upstream when constructing the CarJob, but just in case).
         */

        // Step 2.a: Check if tire shine
        if (carJob.ServiceAddons.Contains(EServiceAddon.TireShine))         // Scans the list of addons each time (inefficient). (O(n) time)
        {
            // Shine tires
            await _tireShineService.ShineTiresAsync(carJob);
        }

        // Step 2.b: Check if exterior clean
        if (carJob.ServiceAddons.Contains(EServiceAddon.InteriorClean))     // Scans the list of addons each time (inefficient). (O(n) time)
        {
            // Clean interior
            await _interiorCleanService.CleanInteriorAsync(carJob);
        }

        // Step 2.c: Check if hand wax and shine
        if (carJob.ServiceAddons.Contains(EServiceAddon.HandWaxAndShine))   // Scans the list of addons each time (inefficient). (O(n) time)
        {
            // Hand wax and shine
            await _handWaxAndShineService.HandWaxAndShineAsync(carJob);
        }
    }
}
