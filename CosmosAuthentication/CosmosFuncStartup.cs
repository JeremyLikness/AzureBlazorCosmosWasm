using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(CosmosAuthentication.CosmosFuncStartup))]

namespace CosmosAuthentication
{
    /// <summary>
    /// Startup class to configure dependency injection.
    /// </summary>
    public class CosmosFuncStartup : FunctionsStartup
    {
        /// <summary>
        /// Configure services for the app.
        /// </summary>
        /// <param name="builder">The <see cref="IFunctionsHostBuilder"/> implementation.</param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<
                ILogger<CosmosClientWrapper>, Logger<CosmosClientWrapper>>();
            builder.Services.AddSingleton<CosmosClientWrapper>();
        }
    }
}
