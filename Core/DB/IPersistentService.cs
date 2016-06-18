using Core.Service;

namespace Core.DB
{

    public interface IPersistentService : IService
    {
        void Execute();
    }

}