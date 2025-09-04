// Copyright (c) 2025 Car Wash Processor, All Rights Reserved.

using CarWashProcessor.Models;  // For CarJob

namespace CarWashProcessor.Domain.Abstractions.Services
{
    /// <summary>
    /// Interface defining the contract for addon service strategies.
    /// </summary>
    public interface IAddonServiceStrategy
    {
        /// <summary>
        /// Performs the addon service for the given car job.
        /// </summary>
        /// <param name="carJob">
        /// The car job for which the addon service is to be performed.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation of performing the addon service.
        /// </returns>
        Task PerformAddonAsync(CarJob carJob, CancellationToken cancellationToken);
    }
}
