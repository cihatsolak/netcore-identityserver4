using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer.AuthServer
{
    /// <summary>
    /// Identity server ayarları
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
                    Scopes = //Identity Servera API2'in nelere yetkisi olduğunu belirtiyorum
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

        /// <summary>
        /// Apileri hangi clientlar kullanacak?
        /// </summary>
        /// <returns>List of clients</returns>
        public static IEnumerable<Client> GetClients()
        {
            List<Client> clients = new()
            {
                new Client()
                {
                    ClientName = "API - Sample Client 1",
                    ClientId = "SampleClient1",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("SampleClientSecret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials, //Kullanıcıyla ilgili işlemim yok, sadece client istek yaptıgı zaman akışa uygun token ver.
                    AllowedScopes  = new List<string> //Client1 hangi apilere ne izni var?
                    {
                        "IdentityServer.API1.Read", //Api1'de okuma izni
                        "IdentityServer.API2.Write", //Api2'de yazma izmi
                        "IdentityServer.API2.Update", //Api2'de güncelleme izni
                    }
                },
                new Client()
                {
                    ClientName = "API - Sample Client 2",
                    ClientId = "SampleClient2",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("SampleClientSecret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials, //Kullanıcıyla ilgili işlemim yok, sadece client istek yaptıgı zaman akışa uygun token ver.
                    AllowedScopes = new List<string> //Client1 hangi apilere ne izni var?
                    {
                        "IdentityServer.API1.Read", //Api1'de okuma izni
                        "IdentityServer.API2.Write", //Api2'de yazma izmi
                        "IdentityServer.API2.Update", //Api2'de güncelleme izni
                    }
                }
            };

            return clients;
        }
    }
}
