using Core.Persist;

namespace MQ.Queue.Impl
{

    public class PersistQueue : IMessageQueue {

        public IPersistentService Storage { get; set; }

        public void SendMessage(Message message)
        {
            throw new System.NotImplementedException();
        }

    }

}