using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Service;
using Core.Transfer;

namespace Core.Aws {

    public class AmazonTransferService : AbstractService, IAmazonTransferService {

        public string BucketName { get; set; }
        public AmazonCredential Credential { get; set; }

        protected override void StartUp() {
            throw new System.NotImplementedException();
        }

        protected override void ShutDown() {
            throw new System.NotImplementedException();
        }

        public bool Exist(string key) {
            throw new System.NotImplementedException();
        }

        public Task<bool> ExistAsync(string key) {
            throw new System.NotImplementedException();
        }

        public List<AmazonS3Object> GetAll() {
            throw new System.NotImplementedException();
        }

        public Task<List<AmazonS3Object>> GetAllAsync() {
            throw new System.NotImplementedException();
        }

        public PagingResult<AmazonS3Object> List(PagingRequest request) {
            throw new System.NotImplementedException();
        }

        public Task<PagingResult<AmazonS3Object>> ListAsync(PagingRequest request) {
            throw new System.NotImplementedException();
        }

        public AmazonS3Object Get(string key) {
            throw new System.NotImplementedException();
        }

        public Task<AmazonS3Object> GetAsync(string key) {
            throw new System.NotImplementedException();
        }

        public void Transfer(AmazonS3Object s3Object) {
            throw new System.NotImplementedException();
        }

        public Task TransferAsync(AmazonS3Object s3Object) {
            throw new System.NotImplementedException();
        }

        public void TransferMany(params AmazonS3Object[] s3Objects) {
            throw new System.NotImplementedException();
        }

        public Task TransferManyAsync(params AmazonS3Object[] s3Objects) {
            throw new System.NotImplementedException();
        }

        public void Remove(string key) {
            throw new System.NotImplementedException();
        }

        public Task RemoveAsync(string key) {
            throw new System.NotImplementedException();
        }

        public void RemoveMany(params string[] objectKeys) {
            throw new System.NotImplementedException();
        }

        public Task RemoveManyAsync(params string[] objectKeys) {
            throw new System.NotImplementedException();
        }

        public void ReplaceWith(string key, AmazonS3Object s3Object) {
            throw new System.NotImplementedException();
        }

        public Task ReplaceWithAsync(string key, AmazonS3Object s3Object) {
            throw new System.NotImplementedException();
        }

        public void Copy(AmazonS3Object s3Object, string key) {
            throw new System.NotImplementedException();
        }

        public Task CopyAsync(AmazonS3Object s3Object, string key) {
            throw new System.NotImplementedException();
        }

        public void Move(AmazonS3Object s3Object, string key) {
            throw new System.NotImplementedException();
        }

        public Task MoveAsync(AmazonS3Object s3Object, string key) {
            throw new System.NotImplementedException();
        }
        
    }
}