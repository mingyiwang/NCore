using System.Threading;

namespace MQ.Queue
{

    public class LooperThread
    {

        private Thread _current;

        public LooperThread(Thread current)
        {
            this._current = current;
        }

    }

    
}