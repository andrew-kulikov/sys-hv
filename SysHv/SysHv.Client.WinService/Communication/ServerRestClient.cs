using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SysHv.Client.Common.DTOs;
using SysHv.Client.WinService.Helpers;
using Decoder = RabbitMQCommunications.Communications.Decoding.Decoder;

namespace SysHv.Client.WinService.Communication
{
    public class ServerRestClient
    {
        public async Task<Response> Login()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationHelper.ServerAddress);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(
                    JsonConvert.SerializeObject(ConfigurationHelper.LoginDto),
                    Encoding.UTF8,
                    "application/json");

                HttpResponseMessage result;
                try
                {
                    result = await client.PostAsync("/api/client/login", content);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }

                if (result.IsSuccessStatusCode)
                {
                    var resultStr = await result.Content.ReadAsStringAsync();
                    var response = Decoder.Decode<Response>(resultStr);

                    Console.WriteLine(resultStr);

                    return response;
                }
            }

            return null;
        }
    }
}