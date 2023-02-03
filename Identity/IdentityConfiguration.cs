using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace Flora.Identity;

public class IdentityConfiguration
{
    public static IEnumerable<ApiScope> ApiScopes =
        new List<ApiScope> { new ApiScope("FloraWebApi", "Web Api") };

    public static IEnumerable<IdentityResource> IdentityResources =
        new List<IdentityResource> { new IdentityResources.OpenId(), new IdentityResources.Profile() };

    public static IEnumerable<ApiResource> ApiResources =
        new List<ApiResource>
        {
            new ApiResource("FloraWebApi", "Web Api", new[] { JwtClaimTypes.Name, JwtClaimTypes.Role })
                { Scopes = { "FloraWebApi" } }
        };

    public static IEnumerable<Client> Clients =
        new List<Client>
        {
            new Client
            {
                ClientId = "job-board-web-app",
                ClientName = "Flora Web",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequirePkce = true,
                RedirectUris =
                {
                    "https://localhost:4200"
                },
                AllowedCorsOrigins =
                {
                    "https://localhost:4200"
                },
                PostLogoutRedirectUris =
                {
                    "https://localhost:4200"
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "FloraWebApi",
                },
                AllowAccessTokensViaBrowser = true,
                AllowOfflineAccess = true
            },
        };
}