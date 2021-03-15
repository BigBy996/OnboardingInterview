using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestProject.Hooks
{
    public class CustomHttpClient
    {
        private HttpClient _httpClient;

        public CustomHttpClient()
        {
            this._httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> Get(string url, Dictionary<string, string> Headers)
        {
            foreach (var header in Headers)
            {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            return await _httpClient.GetAsync(url);
        }

        public async Task<HttpResponseMessage> Post(string url, Dictionary<string, string> Headers, HttpContent content)
        {
            foreach (var header in Headers)
            {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            return await _httpClient.PostAsync(url, content);
        }
    }
}
