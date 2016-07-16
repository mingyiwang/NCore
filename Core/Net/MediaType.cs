using System.Collections.Generic;
using Core.Collection;

namespace Core.Net
{
    public class MediaType
    {

        public string Type { get; private set; }
        private readonly HashSet<string> _extensions;

        public MediaType(string type) : this(type, new[]{ MimeTypes.Instance.ExtensionOf(type)})
        {
              
        }

        public MediaType(string type, IEnumerable<string> extensions)
        {
            Type = type;
            _extensions = extensions == null ? new HashSet<string>() : extensions.ToHashSet();
        }

        public List<string> GetExtensions()
        {
            return new List<string>(_extensions);
        }

        public bool Contains(string fileExtension)
        {
            return fileExtension != null && _extensions.Contains(fileExtension);
        }


    }
}