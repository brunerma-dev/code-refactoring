// Copyright (c) 2025 Car Wash Processor, All Rights Reserved.

using CarWashProcessor.Models;  // For EServiceWash

namespace CarWashProcessor.Application.Abstractions.Registration
{
    /// <summary>
    /// Attribute to mark a wash service implementation with a specific wash type.
    /// </summary>
    /// <remarks>
    /// This attribute is used to facilitate automatic registration and resolution of wash
    /// service implementations.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class WashTypeAttribute : Attribute, IKeyedRegistration<EServiceWash>
    {
        /// <summary>
        /// Gets the specific type of wash service that the decorated class implements.
        /// </summary>
        public EServiceWash Key { get; }

        /// <summary>
        /// Constructor for WashTypeAttribute, which assigns the provided wash type to the Key property.
        /// </summary>
        /// <param name="washType">
        /// The specific type of wash service that the decorated class implements.
        /// </param>
        public WashTypeAttribute(EServiceWash washType) => Key = washType;
    }
}
