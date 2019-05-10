
using Newtonsoft.Json;

namespace RabbitMQCommunications.Communications.Decoding
{
    public static class Decoder
    {
        public static T Decode<T>(string json) => JsonConvert.DeserializeObject<T>(json);
    }
}
