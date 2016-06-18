using System;

namespace Core.Service
{

    public interface IScheduleService : IService
    {
        IScheduledFuture Schedule(Action action, long delay, TimeSpan span);
        IScheduledFuture ScheduleAtFixedRate(Action action,   long initialDelay, long period, TimeSpan span);
        IScheduledFuture ScheduleWithFixedRate(Action action, long initialDelay, long period, TimeSpan span);
    }

}
