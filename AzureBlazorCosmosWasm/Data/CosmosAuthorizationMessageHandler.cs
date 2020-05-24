using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;

namespace AzureBlazorCosmosWasm.Data
{
    /// <summary>
    /// Handler to ensure proper credentials are sent with requests.
    /// </summary>
    public class CosmosAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public string Endpoint { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="CosmosAuthorizationMessageHandler"/>
        /// class.
        /// </summary>
        /// <param name="config"><see cref="IConfiguration"/> to access endpoint.</param>
        /// <param name="provider"><see cref="IAccessTokenProvider"/> service.</param>
        /// <param name="navigation"><see cref="NavigationManager"/> to navigate based on authentication.</param>
        public CosmosAuthorizationMessageHandler(
            IConfiguration config,
            IAccessTokenProvider provider, 
            NavigationManager navigation) : base(provider, navigation)
        {
            var section = config.GetSection(nameof(TokenClient));
            Endpoint = section.GetValue<string>(nameof(Endpoint));
            ConfigureHandler(new[] { Endpoint });
        }
    }
}
