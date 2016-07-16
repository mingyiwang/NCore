namespace Core.Net
{

    public sealed class MimeTypes
    {

        private static MimeTypes _instance;
        private static readonly object SyncObject = new object();

        public static MimeTypes Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new MimeTypes();
                            _instance.TypeOf("");
                            return _instance;
                        }
                    }

                }
                return _instance;
            }
        }

        public string TypeOf(string fileExtension)
        {
            // If doesn't found then returns an octect stream mime type
            return MimeType.ResourceManager.GetString(fileExtension.TrimStart('.'));

        }

        public string ExtensionOf(string mimeType)
        {
            return MimeType.ResourceManager.GetString(mimeType);
        }

        private MimeTypes()
        {
        }

    }

}