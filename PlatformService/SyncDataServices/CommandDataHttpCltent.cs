using PlatformService.Dtos;
using System.Text;
using System.Text.Json;

namespace PlatformService.SyncDataServices
{
    public class CommandDataHttpCltent : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CommandDataHttpCltent(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(platform),Encoding.UTF8,"application/json");
            var res = await _httpClient.PostAsync($"{_configuration["CommandService"]}", httpContent);
            if (res.IsSuccessStatusCode)
            {
                Console.WriteLine("--->  Sync post to command Service was Ok");
            }
            else
            {
                Console.WriteLine("--->  Sync post to command Service was not Ok");
            } 
        }
    }
}
