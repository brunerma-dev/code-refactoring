// Copyright (candidate) 2025 Car Wash Processor, All Rights Reserved.

using System.Reflection;                                        // For Assembly
using CarWashProcessor.Application.Abstractions.Registration;   // For IKeyedRegistration

namespace CarWashProcessor.Infrastructure.DependencyInjection
{
    /// <summary>
    /// Extension methods for IServiceCollection to register services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers implementations of a specified service type from a given assembly.
        /// </summary>
        /// <typeparam name="TServiceInterface">
        /// The service interface type that implementations must implement.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The type of the key used for registration. Must be a non-nullable enum.
        /// </typeparam>
        /// <typeparam name="TAttribute">
        /// The attribute type used to mark implementations for registration.
        /// </typeparam>
        /// <param name="services">
        /// The service collection to which the implementations will be added.
        /// </param>
        /// <param name="assembly">
        /// The assembly to scan for implementations of the service interface.
        /// </param>
        /// <param name="lifetime">
        /// The lifetime with which to register the implementations. Defaults to Singleton.
        /// </param>
        /// <returns>
        /// The modified service collection, allowing for method chaining.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if multiple implementations are found with the same key or if a type has multiple attributes.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if an unsupported ServiceLifetime is provided.
        /// </exception>
        /// <remarks>
        /// This moves registration logic from explicit code registration to convention-based
        /// registration, reducing boilerplate code and the number of files needing modification
        /// when adding new implementations.
        /// </remarks>
        public static IServiceCollection AddKeyedImplementationsByConvention<TServiceInterface, TKey, TAttribute>(
            this IServiceCollection services,
            Assembly assembly,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TServiceInterface : class
            where TKey : struct, Enum
            where TAttribute : Attribute, IKeyedRegistration<TKey>
        {
            // Defensive programming. 
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(assembly);

            // Get the type of the service interface
            var typedService = typeof(TServiceInterface);

            // Find all non-abstract types in the assembly that implement the service interface and
            // have exactly one TAttribute attribute applied. Throw if multiple attributes are found.
            var implementationCandidates = assembly
                .DefinedTypes
                .Where(ti => !ti.IsAbstract && typedService.IsAssignableFrom(ti.AsType()))  // Ensure the type is not abstract and implements TServiceInterface contract.
                .Select(ti =>
                {
                    var attrs = ti.GetCustomAttributes<TAttribute>(inherit: false).ToArray();    // Get all TAttribute attributes applied to the specific type.
                    // Validate that there is at most one TAttribute attribute
                    if (attrs.Length > 1)
                    {
                        throw new InvalidOperationException($"Type '{ti.FullName}' has multiple '{typeof(TAttribute).Name}' attributes. Exactly one is allowed.");
                    }
                    return new { Impl = ti.AsType(), Attr = attrs.SingleOrDefault() };
                })
                .Where(x => x.Attr is not null)
                .ToList();

            // Check for duplicate keys among the implementation candidates (i.e., multiple implementations with the same key).
            var duplicates = implementationCandidates.GroupBy(x => x.Attr!.Key).FirstOrDefault(g => g.Count() > 1);
            if (duplicates is not null)
            {
                var key = duplicates.Key;
                var types = string.Join(", ", duplicates.Select(x => x.Impl.Name));
                throw new InvalidOperationException($"Duplicate keyed registration for key '{key}': {types}");
            }

            // Register each implementation with the specified lifetime and associated key.
            foreach (var candidate in implementationCandidates)
            {
                // Key is non-nullable enum, so safe to cast to object.
                var keyObj = (object)candidate.Attr!.Key!;

                // Register the implementation with the specified lifetime.
                switch (lifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddKeyedSingleton(typeof(TServiceInterface), keyObj, candidate.Impl);
                        break;
                    case ServiceLifetime.Scoped:
                        services.AddKeyedScoped(typeof(TServiceInterface), keyObj, candidate.Impl);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddKeyedTransient(typeof(TServiceInterface), keyObj, candidate.Impl);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
                }
            }

            // Return the modified service collection for chaining.
            return services;
        }
    }
}
