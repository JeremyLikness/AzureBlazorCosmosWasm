using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CosmosAuthentication
{
    /// <summary>
    /// Client for CosmosDB interactions.
    /// </summary>
    public class CosmosClientWrapper
    {
        /// <summary>
        /// SDK reference.
        /// </summary>
        private readonly CosmosClient _client;

        /// <summary>
        /// Logging.
        /// </summary>
        private readonly ILogger<CosmosClientWrapper> _logger;

        /// <summary>
        /// Name of the connection string.
        /// </summary>
        private readonly string CosmosConnection = nameof(CosmosConnection);

        /// <summary>
        /// Creates a new instance of the <see cref="CosmosClientWrapper"/> class.
        /// </summary>
        /// <param name="config">The <see cref="IConfiguration"/> instance to reference connection strings.</param>
        /// <param name="logger">The <see cref="ILogger{CosmosClientWrapper}"/> instance.</param>
        public CosmosClientWrapper(
            IConfiguration config,
            ILogger<CosmosClientWrapper> logger)
        {
            _logger = logger;
            _client = new CosmosClient(config.GetConnectionString(CosmosConnection));
        }

        /// <summary>
        /// Creates or reads an existing user and returns.
        /// </summary>
        /// <param name="database">The Cosmos DB <see cref="Database"/> for the user.</param>
        /// <param name="id">The user id.</param>
        /// <returns>The <see cref="User"/> instance.</returns>
        private async Task<User> CreateOrReadUserAsync(
            Database database, string id)
        {
            _logger.LogInformation("User request for {user}.", id);
            var user = database.GetUser(id);
            UserResponse userResult = null;
            try
            {
                userResult = await user.ReadAsync();
            }
            catch (CosmosException cex)
            {
                if (cex.StatusCode != System.Net.HttpStatusCode.NotFound)
                {
                    throw;
                }
            }
            if (userResult?.Resource == null)
            {
                _logger.LogInformation("User {user} not found.", id);
                var newUser = await database.CreateUserAsync(id);
                user = newUser.User;
                _logger.LogInformation("User {user} created.", id);
            }
            else
            {
                _logger.LogInformation("User {user} exists.", id);
            }
            return user;
        }

        /// <summary>
        /// Creates permissions and generates the token for the user.
        /// </summary>
        /// <param name="id">The user's id.</param>
        /// <returns>A populated <see cref="CosmosToken"/> instance.</returns>
        public async Task<CosmosToken> GetTokenForId(string id)
        {
            var database = _client.GetDatabase(BlogContext.MyBlogs);
            var cosmosUser = await CreateOrReadUserAsync(database, id);
            var permissionId = $"Permission-{id}-blogs";
            var container = database.GetContainer(nameof(BlogContext));
            var permissions = new PermissionProperties(
                id: permissionId,
                permissionMode: PermissionMode.Read,
                container: container);
            await cosmosUser.UpsertPermissionAsync(permissions);
            _logger.LogInformation("Permissions upsert for {user} successful.", id);
            var token = await cosmosUser.GetPermission(permissionId).ReadAsync();
            return new CosmosToken
            {
                Endpoint = _client.Endpoint.ToString(),
                Key = token.Resource.Token
            };
        }
    }
}
