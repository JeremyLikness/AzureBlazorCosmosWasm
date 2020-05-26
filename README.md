# Azure Active Directory from a Blazor WebAssembly App to Access Cosmos DB

A completely serverless solution to access Cosmos DB from Blazor WebAssembly using EF Core.

To configure your application and learn more, read:

[Azure AD Secured Serverless Cosmos DB from Blazor WebAssembly](https://blog.jeremylikness.com/blog/azure-ad-secured-serverless-cosmosdb-from-blazor-webassembly/)

## "Quick" start

1. Optional: fork the repo
1. `git clone https://github.com/jeremylikness/AzureBlazorCosmosWasm.git` (or your fork)
1. Create an app registration for the Blazor app (there is an Azure CLI script for this linked in the comments of the blog post)
1. Update `wwwroot/appsettings.json` in the Blazor WebAssembly client to use the tenant (directory) and client id
1. Create a Cosmos DB database and seed some initial values
1. Deploy the Azure Functions app
1. Add the Cosmos DB connection string as "CosmosConnection" under connection strings for the Azure Functions app
1. Update authentication for the Azure Functions app to use Azure AD
1. Update `wwwroot/appsettings.json` in the Blazor WebAssembly project to point to your functions app (under "TokenClient: Endpoint")
1. Run it!
