using System;

namespace Core.Service
{

    public abstract class AbstractScheduleService : AbstractService, IScheduleService
    {

        public abstract IScheduledFuture Schedule(Action action, long delay, TimeSpan span);
        public abstract IScheduledFuture ScheduleAtFixedRate(Action action, long initialDelay, long period, TimeSpan span);
        public abstract IScheduledFuture ScheduleWithFixedRate(Action action, long initialDelay, long period, TimeSpan span);

        
    }


}