namespace Common
{
    /// <summary>
    /// A Cosmos DB ephemeral token.
    /// </summary>
    public class CosmosToken
    {
        /// <summary>
        /// The database endpoint.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// The token to use.
        /// </summary>
        public string Key { get; set; }
    }
}
