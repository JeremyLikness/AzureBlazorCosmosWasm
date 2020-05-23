using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading.Tasks;
using BlogData;
using Common;

namespace AzureBlazorCosmosWasm.Data
{
    /// <summary>
    /// Client to fetch the Cosmos DB token.
    /// </summary>
    public class TokenClient
    {
        /// <summary>
        /// An <see cref="HttpClient"/> instance configured to securely access
        /// the functions endpoint.
        /// </summary>
        private readonly HttpClient _client;

        /// <summary>
        /// The endpoint of the functions host.
        /// </summary>
        public readonly string Endpoint;

        /// <summary>
        /// Creates a new instance of the <see cref="TokenClient"/> class.
        /// </summary>
        /// <param name="factory"></param>
        public TokenClient(IHttpClientFactory factory)
        {
            _client = factory.CreateClient(nameof(TokenClient));
        }

        /// <summary>
        /// Gets a new <see cref="CosmosToken"/> based on the authenticated user.
        /// </summary>
        /// <returns>A new <see cref="CosmosToken"/> instance.</returns>
        public async Task<CosmosToken> GetTokenAsync()
        {
            return await _client.GetJsonAsync<CosmosToken>($"api/RequestToken");
        }
    }
}
