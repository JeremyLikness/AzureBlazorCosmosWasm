using System;
using System.Net.Http;
using System.Threading.Tasks;
using AzureBlazorCosmosWasm.Data;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureBlazorCosmosWasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            // set up a delegate to get the endpoint of the function
            // to fetch the token
            static string functionEndpoint(WebAssemblyHostBuilder builder) =>
                builder.Configuration
                    .GetSection(nameof(TokenClient))
                    .GetValue<string>(nameof(CosmosAuthorizationMessageHandler.Endpoint));

            // sets up Azure Active Directory authentication and adds the 
            // user_impersonation scope to access functions.
            builder.Services.AddMsalAuthentication(options =>
            {
                options.ProviderOptions
                .DefaultAccessTokenScopes.Add($"{functionEndpoint(builder)}user_impersonation");
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
            });

            // set up the authorization handler to inject tokens
            builder.Services.AddTransient<CosmosAuthorizationMessageHandler>();

            // configure the default client (talks to own domain)
            builder.Services.AddTransient(sp => new HttpClient()
            { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // configure the client to talk to the Azure Functions endpoint.
            builder.Services.AddHttpClient(nameof(TokenClient),
                client =>
                {
                    client.BaseAddress = new Uri(functionEndpoint(builder));
                }).AddHttpMessageHandler<CosmosAuthorizationMessageHandler>();

            // register the client to retrieve Cosmos DB tokens.
            builder.Services.AddTransient<TokenClient>();

            // register the client to load blogs from Cosmos DB.
            builder.Services.AddTransient<BlogClient>();

            await builder.Build().RunAsync();
        }
    }
}
