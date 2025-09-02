// Copyright (c) 2025 Car Wash Processor, All Rights Reserved

using CarWashProcessor.Models;

namespace CarWashProcessor.Services
{
    /// <summary>
    /// Interface defining a strategy for performing a specific type of car wash service.
    /// </summary>
    public interface IWashServiceStrategy
    {
        /// <summary>
        /// Gets the key representing the type of wash service this strategy handles.
        /// </summary>
        EServiceWash Key { get; }

        /// <summary>
        /// Performs the wash service for the given car job.
        /// </summary>
        /// <param name="carJob">
        /// The car job for which the wash service is to be performed.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation of performing the wash service.
        /// </returns>
        Task PerformWashAsync(CarJob carJob);
    }
}
