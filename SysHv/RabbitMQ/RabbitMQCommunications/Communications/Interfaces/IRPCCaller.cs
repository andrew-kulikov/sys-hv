using System.Threading.Tasks;

namespace RabbitMQCommunications.Communications.Interfaces
{
    public interface IRPCCaller
    {
        Task<string> Call(string message);
    }
}
