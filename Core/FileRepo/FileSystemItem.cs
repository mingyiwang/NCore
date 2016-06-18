using System;
using System.Collections.Generic;

namespace Core.FileRepo {
    
    public abstract class FileSystemItem {

        public abstract FileSystemItem Parent { get; set; }
        public abstract List<FileSystemItem> Children   { get; }
        public abstract List<FileSystemItem> Decendant  { get; }

        public abstract bool IsFile { get; }
        public abstract bool IsDirectory { get; }

        public string Name     { get; set; }
        public string Path     { get; set; }
        public string Keywords { get; set; }
        public string Author   { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public WeakReference<byte[]> Data { get; set; }

        public void SetData(byte[] data) {
            Data = new WeakReference<byte[]>(data);
        }

        public byte[] GetData() {
            byte[] data;
            Data.TryGetTarget(out data);
            return data;
        }

    }

}
