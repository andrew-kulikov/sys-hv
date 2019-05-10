using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using WinAdminClientCore.Dtos;
using WinAdminClientCore.UIHelpers;

namespace WinAdminClientCore.DataHelpers
{
    public static class RequestBuilder
    {
        public static StringContent GenerateLoginBody(string login, string password)
        {
            return new StringContent(
                JsonConvert.SerializeObject(new UserLoginDTO { Email = login, Password = password }),
                Encoding.UTF8,
                "application/json");
        }

        public static void SetAuthToken(HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", PropertiesManager.Token);
        }

        public static void SetJsonAsAcceptable(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}