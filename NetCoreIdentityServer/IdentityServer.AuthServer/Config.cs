using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer.AuthServer
{
    /// <summary>
    /// 
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Api'leri tanıtıyorum
        /// </summary>
        /// <returns>List of api resource</returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            List<ApiResource> apiResources = new()
            {
                new ApiResource
                {
                    Name = "IdentityServer.API1",
                    Scopes = //Identity Servera API1'in nelere yetkisi olduğunu belirtiyorum
                    {
                        "IdentityServer.API1.Read",
                        "IdentityServer.API1.Write",
                        "IdentityServer.API1.Update"
                    }
                },
                new ApiResource
                {
                    Name = "IdentityServer.API2",
                    Scopes =
                    {
                        "IdentityServer.API2.Read",
                        "IdentityServer.API2.Write",
                        "IdentityServer.API2.Update"
                    }
                }
            };

            return apiResources;
        }

        /// <summary>
        /// Api Yetkileri
        /// </summary>
        /// <returns>List of api scope</returns>
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            List<ApiScope> apiScopes = new()
            {
                new ApiScope
                {
                    Name = "IdentityServer.API1.Read",
                    DisplayName = "API 1 - Okuma İzni"
                },
                new ApiScope
                {
                    Name = "IdentityServer.API1.Write",
                    DisplayName = "API 1 - Yazma İzni"
                },
                new ApiScope
                {
                    Name = "IdentityServer.API1.Update",
                    DisplayName = "API 1 - Güncelleme İzni"
                },

                new ApiScope
                {
                    Name = "IdentityServer.API2.Read",
                    DisplayName = "API 2 - Okuma İzni"
                },
                new ApiScope
                {
                    Name = "IdentityServer.API2.Write",
                    DisplayName = "API 2 - Yazma İzni"
                },
                new ApiScope
                {
                    Name = "IdentityServer.API2.Update",
                    DisplayName = "API 2 - Güncelleme İzni"
                },
            };

            return apiScopes;
        }
    }
}
