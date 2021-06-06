using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
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
                    PostLogoutRedirectUris = new List<string> //Uygulamadan çıkış yaptıldığında
                    {
                        "https://localhost:5001/signout-callback-oidc"
                    },
                    AllowedScopes = new List<string> //Bu web uygulaması hangi izinlere sahip olacak?
                    {
                        IdentityServerConstants.StandardScopes.OpenId, //OpenId bilgisine erişeyeceğim
                        IdentityServerConstants.StandardScopes.Profile, //Kullanıcı bilgilerine erişeceğim
                        IdentityServerConstants.StandardScopes.OfflineAccess, //Refresh token
                        "IdentityServer.API1.Read", //Api1 için okuma izni
                        "CountryAndCityCustomResource", //Custom claims
                        "RolesCustomResource"
                    },
                    RequirePkce = false,
                    AccessTokenLifetime = 2 * 60 * 30, //Access token ömrünü 2 saat ayarladım
                    AllowOfflineAccess = true, //Refresh token yayınlanması için
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(50) - DateTime.Now).TotalSeconds, //Refresh token ömrünü 50 gün ayarladım
                    RefreshTokenUsage = TokenUsage.ReUse, //OneTimeOnly: Bu refresh token'ı bir kere kullanbilirsin. || ReUse: Tekrar tekrar kullanabilirsin.
                    RequireConsent = true //login'de onay ekranı çıkmayacak.
                },
                new Client()
                {
                    ClientName = "Web - Sample Client 4",
                    ClientId = "SampleClient4",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("SampleClientSecret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid, //Serverdan response olarak hem token hemde id_token istediğimden dolayı akış hybrid olacaktır.
                    RedirectUris = new List<string> //token alımını gerçekleştiren url ve bu urldende herhangi bir sayfaya yönlendirme işlemi gerçekleştirilecek.
                    {
                        "https://localhost:5002/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string> //Uygulamadan çıkış yaptıldığında
                    {
                        "https://localhost:5002/signout-callback-oidc"
                    },
                    AllowedScopes = new List<string> //Bu web uygulaması hangi izinlere sahip olacak?
                    {
                        IdentityServerConstants.StandardScopes.OpenId, //OpenId bilgisine erişeyeceğim
                        IdentityServerConstants.StandardScopes.Profile, //Kullanıcı bilgilerine erişeceğim
                        IdentityServerConstants.StandardScopes.OfflineAccess, //Refresh token
                        "IdentityServer.API1.Read", //Api1 için okuma izni
                        "CountryAndCityCustomResource", //Custom claims
                        "RolesCustomResource"
                    },
                    RequirePkce = false,
                    AccessTokenLifetime = 2 * 60 * 30, //Access token ömrünü 2 saat ayarladım
                    AllowOfflineAccess = true, //Refresh token yayınlanması için
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(50) - DateTime.Now).TotalSeconds, //Refresh token ömrünü 50 gün ayarladım
                    RefreshTokenUsage = TokenUsage.ReUse, //OneTimeOnly: Bu refresh token'ı bir kere kullanbilirsin. || ReUse: Tekrar tekrar kullanabilirsin.
                    RequireConsent = false //login'de onay ekranı çıkmayacak.
                }
            };

            return clients;
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            List<IdentityResource> identityResources = new()
            {
                new IdentityResources.OpenId(), //SubId
                new IdentityResources.Profile(), //Kullanıcı ile ilgili claimler
                new IdentityResource()
                {
                    Name = "CountryAndCityCustomResource",
                    DisplayName = "Ülke ve Şehir Bilgisi",
                    Description = "Kullanıcının ülke ve şehir bilgisi.",
                    UserClaims = new List<string>
                    {
                        "Country",
                        "City"
                    }
                },
                new IdentityResource
                {
                    Name = "RolesCustomResource",
                    DisplayName = "Roller",
                    Description = "Kullanıcıya ait rol bilgisi",
                    UserClaims = new List<string>
                    {
                        "Role"
                    }
                }
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
                        new Claim("given_name", "Cihat"),
                        new Claim("family_name", "Solak"),
                        new Claim("Country", "Türkiye"),
                        new Claim("City","İstanbul"),
                        new Claim("Role","Admin")
                    }
                },
                new TestUser
                {
                    SubjectId = "2", //Kullanıcı id'si
                    Username = "mesutsolak", //İhtiyaca göre username'i e-posta olarak kullanabiliriz.
                    Password = "123456",
                    Claims = new List<Claim> //Claim: Token içerisinde bulunacak datalar.
                    {
                        new Claim("given_name", "Mesut"),
                        new Claim("family_name","Solak"),
                        new Claim("Country", "Türkiye"),
                        new Claim("City","Ankara"),
                        new Claim("Role","Customer")
                    }
                }
            };

            return testUsers;
        }
    }
}
