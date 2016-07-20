using System.Resources;
using System.Threading;

namespace Core.Net
{

    public sealed class MimeTypes
    {

        public const string ApplicationOctetStream = "application/octet-stream";

        private static readonly object Lock = new object();
        private ResourceManager _resourceManager;

        public string TypeOf(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension))
            {
                return ApplicationOctetStream;
            }

            var ext = fileExtension.TrimStart('.').ToLowerInvariant();
            return _resourceManager.GetString(ext) ?? ApplicationOctetStream;
        }

        private static MimeTypes _instance;
        public  static MimeTypes Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                var lockTaken = false;
                try
                {
                    Monitor.Enter(Lock, ref lockTaken);
                    return _instance ?? (_instance = new MimeTypes
                    {
                        _resourceManager = new ResourceManager(typeof(MimeTypes).FullName, typeof(MimeTypes).Assembly)
                    });
                }
                finally
                {
                    if (lockTaken)
                    {
                        Monitor.Exit(Lock);
                    }
                }
                
            }
        }

        private MimeTypes(){}

    }

}