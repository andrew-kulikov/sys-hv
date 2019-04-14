using System.Web.Script.Serialization;

namespace RabbitMQCommunications.Communications.Decoding
{
    public static class Decoder
    {
        public static T Decode<T>(string json) => new JavaScriptSerializer().Deserialize<T>(json);
    }
}
