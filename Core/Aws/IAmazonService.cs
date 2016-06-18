using Core.Service;

namespace Core.Aws {

    public interface IAmazonService : IService {

        AmazonCredential Credential { get; set; }
        
    }

}