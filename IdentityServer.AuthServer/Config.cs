using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

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
            },
            new Client()
            {
                ClientId = "Client1-Mvc",
                RequirePkce= false,
                ClientName = "Client 1 Mvc App",
                ClientSecrets = new List<Secret>() { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Hybrid,
                RedirectUris = new List<string>() { "https://localhost:7100/signin-oidc" },
                PostLogoutRedirectUris =new List<string> { "https://localhost:7100/signout-callback-oidc" },
                AllowedScopes= { IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, "api1.read", IdentityServerConstants.StandardScopes.OfflineAccess },
                AccessTokenLifetime = 7200,
                AllowOfflineAccess= true,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = TokenExpiration.Absolute,
                SlidingRefreshTokenLifetime = (int)(DateTime.UtcNow.AddDays(60) - DateTime.UtcNow).TotalSeconds
            }
        };

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1", Username = "test1", Password= "1234", Claims = new List<Claim>
                    {
                        new Claim("given_name","Erk"),
                        new Claim("family_name","Y")
                    }
                },
                new TestUser
                {
                    SubjectId = "2", Username = "test2", Password= "1234", Claims = new List<Claim>
                    {
                        new Claim("given_name","Era"),
                        new Claim("family_name","Y")
                    }
                }
            };
        }
    }
}
