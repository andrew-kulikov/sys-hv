namespace RabbitMQCommunications.Communications.Interfaces
{
    public interface ISender<T>
    {
        void Send(T dto);
    }
}
