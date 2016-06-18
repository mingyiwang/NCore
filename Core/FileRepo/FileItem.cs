using System.Collections.Generic;

namespace Core.FileRepo {

    public class FileItem : FileSystemItem {

        public override FileSystemItem Parent { get; set; }
        public override bool IsFile => true;
        public override bool IsDirectory => false;
        public override List<FileSystemItem> Children  => new List<FileSystemItem>();
        public override List<FileSystemItem> Decendant => new List<FileSystemItem>();



    }

}
