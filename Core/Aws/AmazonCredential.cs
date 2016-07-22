namespace Core.Aws
{

    public class AmazonCredential
    {

        public string AccessKey
        {
            get; private set;
        }

        public string SecretKey
        {
            get; private set;
        }

        public AmazonCredential(string accessKey, string secretKey)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;

        }

    }

}

