// Copyright (c) 2025 Car Wash Processor, All Rights Reserved.

namespace CarWashProcessor.Application.Abstractions.Resolution
{
    /// <summary>
    /// Defines a contract for resolving services based on a specific key.
    /// </summary>
    /// <typeparam name="TKey">
    /// The type of the key used for resolution. Must be a non-nullable enum.
    /// </typeparam>
    /// <typeparam name="TService">
    /// The type of the service to be resolved.
    /// </typeparam>
    public interface IServiceResolver<TKey, TService>
    {
        /// <summary>
        /// Resolves and returns an instance of the service associated with the provided key.
        /// </summary>
        /// <param name="key">
        /// The <typeparamref name="TKey"/> used to identify and resolve the specific service instance.
        /// </param>
        /// <returns>
        /// The instance of the <typeparamref name="TService"/> associated with the provided key.
        /// </returns>
        TService Resolve(TKey key);
    }
}
