namespace Barker.Stewart.Bpdts.Test.LocationApi.Bpdts
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;

    public class BpdtsClient : IBpdtsClient
    {
        private readonly HttpClient _client;

        public BpdtsClient(HttpClient httpClient, IConfiguration configuration)
        {
            httpClient.BaseAddress = new Uri(configuration["BpdtsTestUrl"]);
            _client = httpClient;
        }

        private async Task<string> GetData(string path)
        {
            return await _client.GetStringAsync(path);
        }

        public async Task<string> GetUsersInCity(string city)
        {
            return await this.GetData($"city/{city}/users");
        }

        public async Task<string> GetAllUsers()
        {
            return await this.GetData("users");
        }
    }
}
