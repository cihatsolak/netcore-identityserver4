using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServer.WebClient1.Services
{
    public interface IHttpClientService
    {
        Task<HttpClient> GetHttpClientAsync();
    }
}
