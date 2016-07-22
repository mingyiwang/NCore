using Core.Aws;
using Moq;
using NUnit.Framework;

namespace Core.Test.Aws
{

    [TestFixture]
    public class AmazonTransferServiceTest
    {
        
        [Test]
        public void TestTransfer()
        {

            var mock = new Mock<AmazonCredential>("123", "123");
            mock.VerifyGet(c => c.AccessKey, Times.Exactly(1));
            mock.VerifyGet(c => c.SecretKey, Times.Exactly(1));
            
        }

    }

}