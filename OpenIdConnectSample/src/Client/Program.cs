using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            CallIdentityEndpoint().Wait();
            Console.WriteLine("Hello World!");
        }


        static async Task<(DiscoveryDocumentResponse response, HttpClient httpClient)> GetDiscoveryDocumentAsync()
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5001");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
            }
            return (disco, client);
        }


        static async Task CallIdentityEndpoint()
        {
            var token = await GetToken();

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(token.AccessToken);
            Console.WriteLine("Token : {0}",token.AccessToken);
            var response = await apiClient.GetAsync("http://localhost:5002/weatherforecast");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Status:{0}", response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Error : {0}", JArray.Parse(content));
            }
        }
        static async Task<TokenResponse> GetToken()
        {
            var (response, client) = await GetDiscoveryDocumentAsync();

            var request = new ClientCredentialsTokenRequest
            {
                Address = response.TokenEndpoint,

                ClientId = "khurram",
                ClientSecret = "secret",
                Scope = "api1"
            };

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(request);

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
            }

            Console.WriteLine(tokenResponse.Json);
            return tokenResponse;
        }
    }
}
