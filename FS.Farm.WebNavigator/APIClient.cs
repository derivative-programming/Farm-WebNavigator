using System;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace FS.Farm.WebNavigator
{ 
    public class APIClient
    {
        private string _rootUrl = @"";
        private System.Net.Http.HttpClient _client;
        private string _apiKey = "";  

        public APIClient(string apiKey, string rootUrl)
        { 
            _client = new System.Net.Http.HttpClient();
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _apiKey = apiKey;
            _rootUrl = rootUrl;
        }

        public string GetAPIKey()
        {
            return _apiKey;
        }

        public async Task<Response> PostAsync<Request, Response>(
            string url,
            Request input)
        {
            return await CreateRequest<Response>(url, System.Net.Http.HttpMethod.Post, input);
        }
        public async Task<Response> PutAsync<Request, Response>(
            string url,
            Request input)
        {
            return await CreateRequest<Response>(url, System.Net.Http.HttpMethod.Put, input);
        }
        public async Task<Response> PatchAsync<Request, Response>(
            string url,
            Request input)
        {
            return await CreateRequest<Response>(url, System.Net.Http.HttpMethod.Patch, input);
        }
        public async Task<Response> GetAsync<Response>(
            string url)
        {
            return await CreateRequest<Response>(url, System.Net.Http.HttpMethod.Get);
        }
        public async Task<Response> DeleteAsync<Response>(
            string url)
        {
            return await CreateRequest<Response>(url, System.Net.Http.HttpMethod.Delete);
        }
        #region [ -- Private helper methods -- ]
        async Task<Response> CreateRequest<Response>(
            string url,
            System.Net.Http.HttpMethod method)
        {
            return await CreateRequestMessage(url, method,  async (msg) =>
            {
                return await GetResult<Response>(msg);
            });
        }
        async Task<Response> CreateRequest<Response>(
            string url,
            System.Net.Http.HttpMethod method,
            object input)
        {
            return await CreateRequestMessage(url, method,  async (msg) =>
            {
                using (var content = new System.Net.Http.StringContent(JObject.FromObject(input).ToString()))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    msg.Content = content;
                    return await GetResult<Response>(msg);
                }
            });
        }
        async Task<Response> CreateRequestMessage<Response>(
            string url,
            System.Net.Http.HttpMethod method, 
            Func<System.Net.Http.HttpRequestMessage, Task<Response>> functor)
        {
            using (var msg = new System.Net.Http.HttpRequestMessage())
            {
                msg.RequestUri = new Uri(_rootUrl + url);
                msg.Method = method;
                msg.Headers.Add("Api-Key", _apiKey);
                if (method == System.Net.Http.HttpMethod.Delete)
                {
                    msg.Headers.Add("Depth", "infinity");
                }
                return await functor(msg);
            }
        }
        async Task<Response> GetResult<Response>(System.Net.Http.HttpRequestMessage msg)
        {
            using (var response = await _client.SendAsync(msg))
            {
                using (var content = response.Content)
                {
                    var responseContent = await content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                        throw new Exception(responseContent);
                    if (typeof(IConvertible).IsAssignableFrom(typeof(Response)))
                        return (Response)Convert.ChangeType(responseContent, typeof(Response));
                    return JToken.Parse(responseContent).ToObject<Response>();
                }
            }
        }
        #endregion
    }
}
