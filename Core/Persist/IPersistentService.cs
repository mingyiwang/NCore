using Core.Service;

namespace Core.Persist
{

    public interface IPersistentService : IService
    {
        void Execute();
    }

}