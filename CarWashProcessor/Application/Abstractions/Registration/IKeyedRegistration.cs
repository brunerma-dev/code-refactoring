// Copyright (c) 2025 Car Wash Processor, All Rights Reserved.

namespace CarWashProcessor.Application.Abstractions.Registration
{
    /// <summary>
    /// Defines a contract for registering services with a specific key.
    /// </summary>
    /// <typeparam name="TKey">
    /// The type of the key used for registration. Must be a non-nullable enum.
    /// </typeparam>
    public interface IKeyedRegistration<out TKey> where TKey : struct, Enum
    {
        /// <summary>
        /// Gets the key associated with the registration.
        /// </summary>
        TKey Key { get; }
    }
}
