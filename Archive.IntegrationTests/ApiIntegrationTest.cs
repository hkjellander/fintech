using Archive.Api.Models;
using Archive.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Archive.IntegrationTests
{
    public class ApiIntegrationTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public ApiIntegrationTest()
        {
            _server = new TestServer(new WebHostBuilder()
                                     .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task GetLogsCalledWithEmptyDatabase()
        {
            var response = await _client.GetAsync("/log");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Empty(JsonConvert
                         .DeserializeObject<IEnumerable<LogEntry>>(responseString));
        }

        private LogEntry GetTestData()
        {
            IDictionary<string, string> data = new Dictionary<string, string>();
            data["foo"] = "bar";
            return new LogEntry()
            {
                Id = 0,
                Json = JsonConvert.SerializeObject(data),
            };
        }
    }
}
