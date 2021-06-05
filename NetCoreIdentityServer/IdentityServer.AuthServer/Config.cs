using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

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
                        "IdentityServer.API1.Create",
                        "IdentityServer.API1.Update"
                    },
                    ApiSecrets = new List<Secret> // connect/introspect basic Auth
                    {
                        new Secret
                        {
                            Value = "SecretAPI1".Sha256()
                        }
                    }
                },
                new ApiResource
                {
                    Name = "IdentityServer.API2",
                    Scopes = //Identity Servera API2'in nelere yetkisi olduğunu belirtiyorum
                    {
                        "IdentityServer.API2.Read",
                        "IdentityServer.API2.Create",
                        "IdentityServer.API2.Update"
                    },
                    ApiSecrets = new List<Secret> // connect/introspect basic Auth
                    {
                        new Secret
                        {
                            Value = "SecretAPI2".Sha256()
                        }
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
                    Name = "IdentityServer.API1.Create",
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
                    Name = "IdentityServer.API2.Create",
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
                    AllowedScopes = new List<string> //Client1 hangi apilere ne izni var?
                    {
                        "IdentityServer.API1.Read" //Api1'de okuma izni
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
                        "IdentityServer.API2.Create", //Api2'de yazma izmi
                        "IdentityServer.API2.Update", //Api2'de güncelleme izni
                    }
                },
                new Client()
                {
                    ClientName = "Web - Sample Client 3",
                    ClientId = "SampleClient3",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("SampleClientSecret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid, //Serverdan response olarak hem token hemde id_token istediğimden dolayı akış hybrid olacaktır.
                    RedirectUris = new List<string> //token alımını gerçekleştiren url ve bu urldende herhangi bir sayfaya yönlendirme işlemi gerçekleştirilecek.
                    {
                        "https://localhost:5001/signin-oidc"
                    },
                    AllowedScopes = new List<string> //Bu web uygulaması hangi izinlere sahip olacak?
                    {
                        IdentityServerConstants.StandardScopes.OpenId, //OpenId bilgisine erişeyeceğim
                        IdentityServerConstants.StandardScopes.Profile, //Kullanıcı bilgilerine erişeceğim
                    },
                    RequirePkce = false
                }
            };

            return clients;
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            List<IdentityResource> identityResources = new()
            {
                new IdentityResources.OpenId(), //SubId
                new IdentityResources.Profile() //Kullanıcı ile ilgili claimler
            };

            return identityResources;
        }

        /// <summary>
        /// Kullanıcı dataları
        /// TestUser -> namespace IdentityServer4.Test
        /// </summary>
        public static List<TestUser> GetTestUsers()
        {
            List<TestUser> testUsers = new()
            {
                new TestUser
                {
                    SubjectId = "1", //Kullanıcı id'si
                    Username = "cihatsolak", //İhtiyaca göre username'i e-posta olarak kullanabiliriz.
                    Password = "123456",
                    Claims = new List<Claim> //Claim: Token içerisinde bulunacak datalar.
                    {
                        new Claim(ClaimTypes.Name, "Cihat"),
                        new Claim(ClaimTypes.Surname, "Solak")
                    }
                },
                new TestUser
                {
                    SubjectId = "2", //Kullanıcı id'si
                    Username = "mesutsolak", //İhtiyaca göre username'i e-posta olarak kullanabiliriz.
                    Password = "123456",
                    Claims = new List<Claim> //Claim: Token içerisinde bulunacak datalar.
                    {
                        new Claim(ClaimTypes.Name, "Mesut"),
                        new Claim(ClaimTypes.Surname, "Solak")
                    }
                }
            };

            return testUsers;
        }
    }
}
