// Copyright (c) 2025 Car Wash Processor, All Rights Reserved.

using CarWashProcessor.Models;  // For CarJob

namespace CarWashProcessor.Domain.Abstractions.Services
{
    /// <summary>
    /// Defines a strategy for performing wash services on a car job.
    /// </summary>
    public interface IWashServiceStrategy
    {
        /// <summary>
        /// Performs the wash service for the given car job.
        /// </summary>
        /// <param name="carJob">
        /// The car job for which the wash service is to be performed.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation of performing the wash service.
        /// </returns>
        Task PerformWashAsync(CarJob carJob, CancellationToken cancellationToken);
    }
}
