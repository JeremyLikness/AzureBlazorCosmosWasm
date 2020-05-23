using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace CosmosAuthentication
{
    /// <summary>
    /// Host for function endpoint to request a token for Cosmos DB access.
    /// </summary>
    public class CosmosPermissions
    {
        /// <summary>
        /// Access to the Cosmos DB SDK.
        /// </summary>
        private readonly CosmosClientWrapper _client;

        /// <summary>
        /// Creates a new instance of the <see cref="CosmosPermissions"/> class.
        /// </summary>
        /// <param name="client">The <see cref="CosmosClientWrapper"/> for Cosmos DB access.</param>
        public CosmosPermissions(CosmosClientWrapper client)
        {
            _client = client;
        }

        /// <summary>
        /// Request a token for the currently authenticated user.
        /// </summary>
        /// <param name="_">The <see cref="HttpTriggerAttribute"/> is used for routing.</param>
        /// <param name="log">The <see cref="ILogger"/> implementation.</param>
        /// <param name="principal">The current user's <see cref="ClaimsPrincipal"/>.</param>
        /// <returns>The user's <see cref="CosmosToken"/>.</returns>
        [FunctionName("RequestToken")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Need for route trigger.")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] 
                HttpRequest req,
            ILogger log,
            ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                log.LogWarning("No principal.");
                return new UnauthorizedResult();
            }

            if (principal.Identity == null)
            {
                log.LogWarning("No identity.");
                return new UnauthorizedResult();
            }

            if (!principal.Identity.IsAuthenticated)
            {
                log.LogWarning("Request was not authenticated.");
                return new UnauthorizedResult();
            }

            var id = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
            log.LogInformation("Authenticated user {user}.", id);
            
            var token = await _client.GetTokenForId(id);
            return new OkObjectResult(token);
        }
    }
}
