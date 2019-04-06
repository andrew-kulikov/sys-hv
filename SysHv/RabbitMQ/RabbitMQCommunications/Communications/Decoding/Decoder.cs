using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace RabbitMQCommunications.Communications.Decoding
{
    public static class Decoder
    {
        public static T Decode<T>(string json)
        {
            return new JavaScriptSerializer().Deserialize<T>(json);
        }
    }
}
