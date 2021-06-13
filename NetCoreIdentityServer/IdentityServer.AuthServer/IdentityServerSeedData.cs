using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using System.Linq;

namespace IdentityServer.AuthServer
{
    public static class IdentityServerSeedData
    {
        public static void SeedData(ConfigurationDbContext configurationDbContext)
        {
            if (!configurationDbContext.Clients.Any())
            {
                var clients = Config.GetClients();
                foreach (var client in clients)
                {
                    configurationDbContext.Clients.Add(client.ToEntity());
                }
            }

            if (!configurationDbContext.ApiResources.Any())
            {
                var apiResources = Config.GetApiResources();
                foreach (var apiResource in apiResources)
                {
                    configurationDbContext.ApiResources.Add(apiResource.ToEntity());
                }
            }

            if (!configurationDbContext.ApiScopes.Any())
            {
                var apiScopes = Config.GetApiScopes();
                foreach (var apiScope in apiScopes)
                {
                    configurationDbContext.ApiScopes.Add(apiScope.ToEntity());
                }
            }

            if (!configurationDbContext.IdentityResources.Any())
            {
                var identityResources = Config.GetIdentityResources();
                foreach (var identityResource in identityResources)
                {
                    configurationDbContext.IdentityResources.Add(identityResource.ToEntity());
                }
            }

            configurationDbContext.SaveChanges();
        }
    }
}
