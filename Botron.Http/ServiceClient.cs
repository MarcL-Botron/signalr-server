using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Botron.Http.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Botron.Http
{
    public class ServiceClient
    {
        public Dictionary<string, string> RequestHeaders { get; } = new Dictionary<string, string>();

        JsonSerializer serializer = new JsonSerializer();
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public async Task<T> Get<T>(string url)
        {
            using (HttpClient client = InitializeHttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                VerifyResponse(response);

                Stream contentStream = await response.Content.ReadAsStreamAsync();
                return DeserializePayload<T>(contentStream);
            }
        }

        public async Task Post(string url, object payload)
        {
            using (HttpClient client = InitializeHttpClient())
            {
                HttpContent content = CreateHttpContent(payload);
                HttpResponseMessage response = await client.PostAsync(url, content);
                VerifyResponse(response);
            }
        }

        protected HttpClient InitializeHttpClient()
        {
            HttpClient client = CreateHttpClient();
            foreach (KeyValuePair<string, string> header in RequestHeaders)
                client.DefaultRequestHeaders.Add(header.Key, header.Value);

            return client;
        }

        protected virtual HttpClient CreateHttpClient()
        {
            return new HttpClient();
        }

        protected HttpContent CreateHttpContent(object payload)
        {
            return new StringContent(SerializePayload(payload), Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        protected void VerifyResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new BadGatewayException(response.ReasonPhrase);
        }

        protected string SerializePayload(object payload)
        {
            return JsonConvert.SerializeObject(payload, settings);
        }

        protected T DeserializePayload<T>(Stream stream)
        {
            T payload;
            using (StreamReader reader = new StreamReader(stream))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    payload = serializer.Deserialize<T>(jsonReader);
                }
            }
            return payload;
        }
    }
}
