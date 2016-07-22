using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Transfer;

namespace Core.Aws {

    public interface IAmazonTransferService : IAmazonService {

        /// <summary>
        /// The Toppest Bucket
        /// </summary>
        string BucketName { get; }

        /// <summary>
        /// Checks an s3 object exist or not
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exist(string key);
        Task<bool> ExistAsync(string key);

        /// <summary>
        /// Retrieve All S3 Objects
        /// </summary>
        /// <returns></returns>
        List<AmazonS3Object> GetAll();
        Task<List<AmazonS3Object>> GetAllAsync();

        /// <summary>
        /// Retrieve S3 Objects By Paging
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PagingResult<AmazonS3Object> List(PagingRequest request);
        Task<PagingResult<AmazonS3Object>> ListAsync(PagingRequest request);

        /// <summary>
        /// Retrieve Single S3 Object
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        AmazonS3Object Get(string key);
        Task<AmazonS3Object> GetAsync(string key);

        /// <summary>
        /// Upload Object to S3 Bucket
        /// </summary>
        /// <param name="s3Object"></param>
        void Transfer(AmazonS3Object s3Object);
        Task TransferAsync(AmazonS3Object s3Object);
        
        /// <summary>
        /// Upload Multiple S3 Objects
        /// </summary>
        /// <param name="s3Objects"></param>
        void TransferMany(params AmazonS3Object[] s3Objects);
        Task TransferManyAsync(params AmazonS3Object[] s3Objects);

        /// <summary>
        /// Delete S3 Object
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
        Task RemoveAsync(string key);

        /// <summary>
        /// Delete Mutliple S3 Objects
        /// </summary>
        /// <param name="objectKeys"></param>
        void RemoveMany(params string[] objectKeys);
        Task RemoveManyAsync(params string[] objectKeys);

        /// <summary>
        /// Replace Current S3 Object with New Object
        /// </summary>
        /// <param name="key"></param>
        /// <param name="s3Object"></param>
        void ReplaceWith(string key, AmazonS3Object s3Object);
        Task ReplaceWithAsync(string key, AmazonS3Object s3Object);

        /// <summary>
        /// Copy Current Object to New Place
        /// </summary>
        /// <param name="s3Object"></param>
        /// <param name="key"></param>
        void Copy(AmazonS3Object s3Object, string key);
        Task CopyAsync(AmazonS3Object s3Object, string key);

        /// <summary>
        /// Move Current Object to New Location, Delete Old Location afterwards
        /// </summary>
        /// <param name="s3Object"></param>
        /// <param name="key"></param>
        void Move(AmazonS3Object s3Object, string key);
        Task MoveAsync(AmazonS3Object s3Object, string key);

    }

}
