using BlogData;
using Common;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AzureBlazorCosmosWasm.Data
{
    /// <summary>
    /// Service to access the Cosmos DB blog posts.
    /// </summary>
    public class BlogClient
    {
        /// <summary>
        /// Current copy of token.
        /// </summary>
        private CosmosToken _credentials;

        /// <summary>
        /// The <see cref="TokenClient"/> to retrive credentials.
        /// </summary>
        private readonly TokenClient _tokenClient;

        /// <summary>
        /// Creates a new instance of the <see cref="BlogClient"/> class.
        /// </summary>
        /// <param name="tokenClient">The <see cref="TokenClient"/> to request the token.</param>
        public BlogClient(TokenClient tokenClient)
        {
            _tokenClient = tokenClient;
        }

        /// <summary>
        /// Retrieves a new instance of the <see cref="BlogContext"/>.
        /// </summary>
        /// <returns>A <see cref="BlogContext"/> instance.</returns>
        public async Task<BlogContext> GetDbContextAsync()
        {
            if (_credentials == null)
            {
                _credentials = await _tokenClient.GetTokenAsync();
            }

            BlogContext context = null;

            // use a delegate to always resolve late (no caching)
            CosmosToken getCredentials() => _credentials;

            // configure EF Core to use Cosmos DB
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseCosmos(getCredentials().Endpoint,
                    getCredentials().Key,
                    Context.MyBlogs,
                opt =>
                 opt.ConnectionMode(Microsoft.Azure.Cosmos.ConnectionMode.Gateway));

            try
            {
                // if credentials are stale this will fail
                context = new BlogContext(options.Options);
            }
            catch
            {
                // try again with fresh credentials
                _credentials = await _tokenClient.GetTokenAsync();
                context = new BlogContext(options.Options);
            }

            // give what we got
            return context;
        }
    }
}
