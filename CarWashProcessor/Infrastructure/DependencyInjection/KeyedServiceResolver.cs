// Copyright (c) 2025 Car Wash Processor, All Rights Reserved.

using CarWashProcessor.Application.Abstractions.Resolution; // For IServiceResolver

namespace CarWashProcessor.Infrastructure.DependencyInjection
{
    /// <summary>
    /// A resolver that retrieves services based on a specified key.
    /// </summary>
    /// <typeparam name="TKey">
    /// The type of the key used to identify the service. Must be non-nullable.
    /// </typeparam>
    /// <typeparam name="TService">
    /// The type of the service to be resolved. Must be a reference type.
    /// </typeparam>
    public sealed class KeyedServiceResolver<TKey, TService> : IServiceResolver<TKey, TService>
        where TKey : notnull
        where TService : class
    {
        /// <summary>
        /// The service provider used to resolve services.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedServiceResolver{TKey, TService}"/> class.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider used to resolve services.
        /// </param>
        public KeyedServiceResolver(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        /// <summary>
        /// Resolves a service instance based on the provided key.
        /// </summary>
        /// <param name="key">
        /// The key used to identify the desired service instance.
        /// </param>
        /// <returns>
        /// The resolved service instance of type <typeparamref name="TService"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the service corresponding to the provided key is not recognized. This can happen
        /// if the service was not registered correctly in the dependency injection container.
        /// </exception>
        public TService Resolve(TKey key) =>
            _serviceProvider.GetRequiredKeyedService<TService>(key);
    }
}
