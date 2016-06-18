using System.Collections.Generic;
using Core.IO;

namespace Core.Net
{

    public sealed class MimeTypes
    {
        private readonly IDictionary<string,string> _extensionToMimeType = new Dictionary<string, string>();

        private static void Load()
        {
            Resources.GetString("mime.types.config", typeof(MimeTypes));
        }

    }

}