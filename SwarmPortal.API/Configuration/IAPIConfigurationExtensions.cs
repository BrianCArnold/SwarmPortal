namespace SwarmPortal.Common;

public static class IAPIConfigurationExtensions
{
    public static IServiceCollection AddAPIConfiguration(this IServiceCollection serviceCollection)
     => serviceCollection.AddScoped<IAPIConfiguration>(services => APIConfiguration.Create(services.GetRequiredService<IConfiguration>()));
}