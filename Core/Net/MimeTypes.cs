using System.Resources;

namespace Core.Net
{

    public sealed class MimeTypes
    {

        public const string ApplicationOctetStream = "application/octet-stream";

        private static readonly object SyncObject = new object();
        private ResourceManager _resourceManager;

        public string Of(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension))
            {
                return ApplicationOctetStream;
            }

            return _resourceManager.GetString(fileExtension.TrimStart('.').ToLowerInvariant())
                ?? ApplicationOctetStream;
        }

        private static MimeTypes _instance;
        public  static MimeTypes Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new MimeTypes
                            {
                                _resourceManager = new ResourceManager(
                                    typeof(MimeTypes).FullName,
                                    typeof(MimeTypes).Assembly
                                )

                            };
                            return _instance;
                        }
                    }
                }
                return _instance;
            }
        }

        private MimeTypes(){}

    }

}