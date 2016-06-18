using System.Collections.Generic;
using Core.Service;

namespace Core.FileRepo {

    public interface IFileRepository : IService {

        List<FileSystemItem> GetFiles(bool recursive);
        List<FileSystemItem> GetDirectories(bool recursive);

        List<FileSystemItem> GetFiles(DirectoryItem direcotry, bool recursive);
        List<FileSystemItem> GetDirectories(DirectoryItem directory, bool recursive);

        List<FileSystemItem> GetFiles(string path, bool recursive);
        List<FileSystemItem> GetDirectories(string path, bool recursive);

        bool DeleteFile(FileItem file);
        bool DeleteDirectory(DirectoryItem directory, bool recursive);

        void CreateFile(FileItem file);
        void CreateDirectory(DirectoryItem directory);

    }
}
