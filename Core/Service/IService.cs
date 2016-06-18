
namespace Core.Service
{

    public interface IService
    {

        ServiceState State { get; }
        string Id { get; }
        string Name { get; }
        bool IsStarted { get; }

        IService Start();
        void AwaitStarted();

        IService Stop();
        void AwaitStopped();

    }

}
