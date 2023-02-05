using IdentityServer4.Models;

namespace IdentityServer.AuthServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>()
            {
                new ApiResource("resource_api1")
                {
                    Scopes = { "api1.read", "api1.write", "api1.update" },
                    ApiSecrets = new[]{ new Secret("secretapi1".Sha256())},
                },
                new ApiResource("resource_api2") { 
                    Scopes = { "api2.read", "api2.write", "api2.update" },
                    ApiSecrets = new[]{ new Secret("secretapi2".Sha256())},
                }
            };

        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope>()
            {
                new ApiScope("api1.read","read for API 1"),
                new ApiScope("api1.write","write for API 1"),
                new ApiScope("api1.update","update for API 1"),

                new ApiScope("api2.read","read for API 2"),
                new ApiScope("api2.write","write for API 2"),
                new ApiScope("api2.update","update for API 2")
            };

        public static IEnumerable<Client> GetClients() => new List<Client>()
        {
            new Client()
            {
                ClientId = "Client1",
                ClientName = "Client 1 App",
                ClientSecrets = new List<Secret>() { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes= { "api1.read" }
            },
            new Client()
            {
                ClientId = "Client2",
                ClientName = "Client 2 App",
                ClientSecrets = new List<Secret>() { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes= { "api1.read", "api1.update", "api2.write", "api2.update" }
            }
        };
    }
}
