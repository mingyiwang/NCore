namespace MQ.Queue {

    public interface IMessageQueue
    {
        void SendMessage(Message message);
    }

}