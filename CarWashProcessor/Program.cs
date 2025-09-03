// Copyright (c) 2024 Car Wash Processor., All Rights Reserved

using System.Reflection;

using CarWashProcessor.Application.Abstractions.Registration;
using CarWashProcessor.Application.Abstractions.Resolution;
using CarWashProcessor.Domain.Abstractions.Services;
using CarWashProcessor.Infrastructure.DependencyInjection;
using CarWashProcessor.Models;
using CarWashProcessor.Services;

namespace CarWashProcessor;

public class Program
{
    /// <summary>
    /// The main entry point for the Car Wash Processor application.
    /// </summary>
    /// <param name="args">
    /// Command-line arguments passed to the application.
    /// </param>
    /// <remarks>
    /// Do not touch, per README.txt constraints.
    /// </remarks>
	public static void Main(string[] args)
	{
		// Create builder
		var builder = Host.CreateApplicationBuilder(args);
		// Configure logging
		builder.Logging.AddSystemdConsole();
		// Register services
		builder.Services.AddHostedService<Worker>();
		builder.Services.AddSingleton<CarJobService>();
		_RegisterServices(builder.Services);
		// Build and run host
		var host = builder.Build();
		host.Run();
	}

    /// <summary>
    /// Registers application services with the dependency injection container.
    /// </summary>
    /// <param name="services">
	/// The service collection to which application services are to be added.
	/// </param>
    private static void _RegisterServices(IServiceCollection services)
	{
        /* TODO: Optimize registration. 
		 * Currently each concrete class is registered. This needs to be optimized to register per 
		 * interface/implementation pair. At the moment, when adding a new wash or add-on, an engineer 
		 * needs to:
		 * - add a new class (addon/wash)
		 * - modify Program.cs, CarJobProcessor.cs, and CarJob.cs [EServiceWash, EServiceAddon]
		 * 
		 * That is four touchpoints, we can eliminate modifying Program.cs with assembly scanning and 
		 * reflection during startup. 
		 */

        // ─────────────────────────────────────────────────────────────────────────────
        // Orchestrator (Application use-case coordinator)
        // ─────────────────────────────────────────────────────────────────────────────
        services.AddSingleton<CarJobProcessorService>();

        // ─────────────────────────────────────────────────────────────────────────────
        // Keyed Service Resolvers (Application abstraction, Infrastructure implementation)
        // Using .NET 8 keyed DI at runtime; O(1) resolution by enum key.
        // ─────────────────────────────────────────────────────────────────────────────
        services.AddSingleton<IServiceResolver<EServiceWash, IWashServiceStrategy>, KeyedServiceResolver<EServiceWash, IWashServiceStrategy>>();
        services.AddSingleton<IServiceResolver<EServiceAddon, IAddonStrategy>, KeyedServiceResolver<EServiceAddon, IAddonStrategy>>();

        // ─────────────────────────────────────────────────────────────────────────────
        // Convention-based, keyed registrations for policies (ATTRIBUTE-DRIVEN)
        // - Scan all loaded assemblies once.
        // - Keep fail-fast semantics for duplicate keys (enforced by the registrar).
        // ─────────────────────────────────────────────────────────────────────────────

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            // Register wash service strategies by convention (keyed by EServiceWash via WashTypeAttribute).
            services.AddKeyedImplementationsByConvention<IWashServiceStrategy, EServiceWash, WashTypeAttribute>(assembly);

            // TODO: Add addon strategies when implemented.
        }

        // TODO: Remove explicit registrations once convention-based registration is verified.
        services.AddSingleton<TireShineService>();
        services.AddSingleton<InteriorCleanService>();
        services.AddSingleton<HandWaxAndShineService>();
    }
}
