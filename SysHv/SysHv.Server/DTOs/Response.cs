using Newtonsoft.Json;

namespace SysHv.Server.DTOs
{
    public class Response<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("result")]
        public T Result { get; set; }
    }
}
