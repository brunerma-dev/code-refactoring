// Copyright (c) 2025 Car Wash Processor, All Rights Reserved.

using CarWashProcessor.Models;  // For EServiceAddon

namespace CarWashProcessor.Application.Abstractions.Registration
{
    /// <summary>
    /// Attribute to specify the type of add-on service (See <see cref="EServiceAddon"/> implemented by a class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class AddonTypeAttribute : Attribute, IKeyedRegistration<EServiceAddon>
    {
        /// <summary>
        /// Gets the specific type of add-on service that the decorated class implements.
        /// </summary>
        public EServiceAddon Key { get; }

        /// <summary>
        /// Constructor for AddOnTypeAttribute, which assigns the provided add-on type to the Key property.
        /// </summary>
        /// <param name="addOnType">
        /// The specific type of add-on service that the decorated class implements.
        /// </param>
        public AddonTypeAttribute(EServiceAddon addOnType) => Key = addOnType;
    }
}
