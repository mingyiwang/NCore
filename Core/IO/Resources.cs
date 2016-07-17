using System;
using System.IO;
using System.Linq;
using Core.Primitive;

namespace Core.IO
{

    public sealed class Resources
    {
        /// <summary>
        /// Retrieve Resource Binary Data based on FullPath e.g A.B.C.filename.txt
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string fullPath)
        {
            return Streams.GetBytes(GetStream(fullPath));
        }


        /// <summary>
        /// Retrieve String based on FullPath e.g A.B.C.filename.txt
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetString(string fullPath)
        {
            return Streams.GetString(GetStream(fullPath));
        }

        /// <summary>
        /// Retrieve Resource Stream on FullPath e.g A.B.C.filename.txt
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        private static Stream GetStream(string fullPath)
        {
            var assemblyName = Strings.SubString(fullPath, 0, fullPath.IndexOf(".", StringComparison.Ordinal));
            var assembly = AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name.Equals(assemblyName));
            if(assembly == null)
            {
                throw new FileNotFoundException("Can not found resource [" + fullPath + "]");
            }

            return assembly.GetManifestResourceStream(fullPath);
        }

        /// <summary>
        /// Retrieve Resource Binary Data Based on Resource Name and Type lives in the same location as this Resource
        /// </summary>
        /// <param name="fileName">Resource Name</param>
        /// <param name="type">Type locates in the same as Resource</param>
        /// <returns></returns>
        public static byte[] GetBytes(string fileName, Type type)
        {
            return Streams.GetBytes(GetStream(fileName, type));
        }

        /// <summary>
        /// Retrieve String based on Resource Name and Type lives in the same location as this Resource
        /// </summary>
        /// <param name="fileName">Resource Name to be retrieved</param>
        /// <param name="type">Type locates in the same location as resource name</param>
        /// <returns>Resource</returns>
        public static string GetString(string fileName, Type type)
        {
            return Streams.GetString(GetStream(fileName, type));
        }

        /// <summary>
        /// Retrieve Resource Stream based on Resource Name and Type lives in the same location as this Resource
        /// </summary>
        /// <param name="fileName">Resource Name to be retrieved</param>
        /// <param name="type">Type locates in the same location as resource name</param>
        /// <returns>Resource</returns>
        private static Stream GetStream(string fileName, Type type)
        {
            return type.Assembly.GetManifestResourceStream(type, fileName);
        }

    }
}