// Copyright (c) 2025 Car Wash Processor, All Rights Reserved

using CarWashProcessor.Models;

namespace CarWashProcessor.Services;

public class CarJobProcessorService
{
    /* TODO: Replace concrete injected service fields with interfaces. */
    private readonly BasicWashService _basicWashService;
    private readonly AwesomeWashService _awesomeWashService;
    private readonly ToTheMaxWashService _toTheMaxWashService;
    private readonly TireShineService _tireShineService;
    private readonly InteriorCleanService _interiorCleanService;
    private readonly HandWaxAndShineService _handWaxAndShineService;

    // TODO: DIP. Use interfaces instead of concrete classes, group comparable services into collections to reduce constructor parameters.

    /// <summary>
    /// Constructor for CarJobProcessorService.
    /// </summary>
    /// <param name="basicWashService">
    /// The basic wash service to be used for processing car jobs.
    /// </param>
    /// <param name="awesomeWashService">
    /// The awesome wash service to be used for processing car jobs.
    /// </param>
    /// <param name="toTheMaxWashService">
    /// The to-the-max wash service to be used for processing car jobs.
    /// </param>
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
        BasicWashService basicWashService,
        AwesomeWashService awesomeWashService,
        ToTheMaxWashService toTheMaxWashService,
        TireShineService tireShineService,
        InteriorCleanService interiorCleanService,
        HandWaxAndShineService handWaxAndShineService
    )
    {
        // Defensive programming, validate input parameters in constructor, or fast fail, followed by assignments.
        ArgumentNullException.ThrowIfNull(basicWashService, nameof(basicWashService));
        ArgumentNullException.ThrowIfNull(awesomeWashService, nameof(awesomeWashService));
        ArgumentNullException.ThrowIfNull(toTheMaxWashService, nameof(toTheMaxWashService));
        ArgumentNullException.ThrowIfNull(tireShineService, nameof(tireShineService));
        ArgumentNullException.ThrowIfNull(interiorCleanService, nameof(interiorCleanService));
        ArgumentNullException.ThrowIfNull(handWaxAndShineService, nameof(handWaxAndShineService));

        // Set services
        _basicWashService = basicWashService;
        _awesomeWashService = awesomeWashService;
        _toTheMaxWashService = toTheMaxWashService;
        _tireShineService = tireShineService;
        _interiorCleanService = interiorCleanService;
        _handWaxAndShineService = handWaxAndShineService;
    }

    /// <summary>
    /// Processes a car job by performing the requested wash service and any additional addons.
    /// </summary>
    /// <param name="carJob">
    /// The car job to be processed, containing details about the customer, car make, requested wash service, and any additional addons.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation of processing the car job.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="carJob"/> parameter is null.
    /// </exception>
    /// <remarks>
    /// TODO: This method is called by the Worker, but doesn't pass the CancellationToken. If the service is shutting down, ongoing tasks 
    /// aren't going to receive the cancellation request. I can't modify Worker.cs, so I can't change this function signature.
    /// </remarks>
    public async Task ProcessCarJobAsync(CarJob carJob)
    {
        // Defensive programming. Validate input parameters on public methods.
        ArgumentNullException.ThrowIfNull(carJob, nameof(carJob));

        // Step 1: Determine which wash service is requested, and perform it.
        /* TODO: Optimize wash services. 
         * All *WashService classes have a single public method, accepting the CarJob. These are strategies for washing a car. */
        switch (carJob.ServiceWash)
        {
            case EServiceWash.Basic:
                // Do basic wash
                await _basicWashService.DoBasicWashAsync(carJob);
                break;
            case EServiceWash.Awesome:
                // Do awesome wash
                await _awesomeWashService.DoAwesomeWashAsync(carJob);
                break;
            case EServiceWash.ToTheMax:
                // Do to the max wash
                await _toTheMaxWashService.DoToTheMaxWashAsync(carJob);
                break;
            default:
                // Throw error
                throw new InvalidOperationException(
                    $"Wash service ({carJob.ServiceWash}) not recognized."
                );
        }

        // Step 2: Process any addons requested.

        /* TODO: Optimize ServiceAddons processing. 
         * These could also be strategies with a resolver/factory, but does the processing order matter? This represents real-world 
         * actions, so order might be required (pipeline, command, etc.). If order doesn't matter, we could do them in parallel, but 
         * that doesn't map to the real-world. We could do them sequentially, with a foreach loop.
         */

        // Step 2.a: Check if tire shine
        if (carJob.ServiceAddons.Contains(EServiceAddon.TireShine))         // Scans the list of addons each time. (O(n) time)
        {
            // Shine tires
            await _tireShineService.ShineTiresAsync(carJob);
        }
        // Step 2.b: Check if exterior clean
        if (carJob.ServiceAddons.Contains(EServiceAddon.InteriorClean))		// Scans the list of addons each time. (O(n) time)
        {
            // Clean interior
            await _interiorCleanService.CleanInteriorAsync(carJob);
        }
        // Step 2.c: Check if hand wax and shine
        if (carJob.ServiceAddons.Contains(EServiceAddon.HandWaxAndShine))	// Scans the list of addons each time. (O(n) time)
        {
            // Hand wax and shine
            await _handWaxAndShineService.HandWaxAndShineAsync(carJob);
        }
    }
}
