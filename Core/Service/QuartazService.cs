using System;

namespace Core.Service
{

    public class QuartazService : AbstractScheduleService
    {
        protected override void StartUp()
        {
            throw new NotImplementedException();
        }

        protected override void ShutDown()
        {
            throw new NotImplementedException();
        }

        public override IScheduledFuture Schedule(Action action, long delay, TimeSpan span)
        {
            throw new NotImplementedException();
        }

        public override IScheduledFuture ScheduleAtFixedRate(Action action, long initialDelay, long period, TimeSpan span)
        {
            throw new NotImplementedException();
        }

        public override IScheduledFuture ScheduleWithFixedRate(Action action, long initialDelay, long period, TimeSpan span)
        {
            throw new NotImplementedException();
        }
    }

}
