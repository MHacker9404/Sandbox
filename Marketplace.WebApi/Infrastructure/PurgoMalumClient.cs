using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace Marketplace.WebApi.Infrastructure
{
    public class PurgoMalumClient
    {
        private readonly HttpClient _client;

        public PurgoMalumClient() : this(new HttpClient()) { }

        public PurgoMalumClient(HttpClient client) => _client = client;

        public async Task<bool> CheckForProfanity(string text)
        {
            var result = await _client.GetAsync(
                             QueryHelpers.AddQueryString("https://www.purgomalum.com/service/containsprofanity", "text", text));
            var value = await result.Content.ReadAsStringAsync();
            return bool.Parse(value);
        }
    }
}