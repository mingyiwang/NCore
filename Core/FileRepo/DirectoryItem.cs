using System.Collections.Generic;

namespace Core.FileRepo {

    public class DirectoryItem : FileSystemItem {

        public override bool IsFile => false;
        public override bool IsDirectory => true;
        public override FileSystemItem Parent { get; set; }

        public override List<FileSystemItem> Children {
            get {
                throw new System.NotImplementedException();
            }
        }

        public override List<FileSystemItem> Decendant {
            get {
                throw new System.NotImplementedException();
            }
        }
        
    }

}
