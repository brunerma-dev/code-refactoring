// Copyright (c) 2025 Car Wash Processor, All Rights Reserved.

using CarWashProcessor.Models;  // For EServiceAddon

namespace CarWashProcessor.Application.Abstractions.Registration
{
    /// <summary>
    /// Attribute to specify the type of add-on service (See <see cref="EServiceAddon"/> implemented by a class.
    /// </summary>
    /// <remarks>
    /// Constructor for AddOnTypeAttribute, which assigns the provided add-on type to the Key property.
    /// </remarks>
    /// <param name="addOnType">
    /// The specific type of add-on service that the decorated class implements.
    /// </param>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class AddonTypeAttribute(EServiceAddon addOnType) : Attribute, IKeyedRegistration<EServiceAddon>
    {
        /// <summary>
        /// Gets the specific type of add-on service that the decorated class implements.
        /// </summary>
        public EServiceAddon Key { get; } = addOnType;
    }
}
