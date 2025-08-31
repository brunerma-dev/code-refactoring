using CarWashProcessor.Services;

namespace CarWashProcessor;

public class Program
{
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

		// Register services
		services.AddSingleton<CarJobProcessorService>();
		services.AddSingleton<BasicWashService>();
		services.AddSingleton<AwesomeWashService>();
		services.AddSingleton<ToTheMaxWashService>();
		services.AddSingleton<TireShineService>();
		services.AddSingleton<InteriorCleanService>();
		services.AddSingleton<HandWaxAndShineService>();
	}
}
