using System;

namespace Core.Service
{

    public class ScheduleService : AbstractScheduleService
    {
        protected override void StartUp()
        {
            
        }

        protected override void ShutDown()
        {
            
        }

        public override IScheduledFuture Schedule(Action action, long delay, TimeSpan span)
        {
            return null;
        }

        public override IScheduledFuture ScheduleAtFixedRate(Action action, long initialDelay, long period, TimeSpan span)
        {
            return null;
        }

        public override IScheduledFuture ScheduleWithFixedRate(Action action, long initialDelay, long period, TimeSpan span)
        {
            return null;
        }

    }

}