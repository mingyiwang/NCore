
namespace Core.Service
{
    public interface IScheduledFuture
    {
        bool IsDone();
        bool IsCancelled();
        bool Cancel(bool mayInterruptingWhileRunning);

    }
}