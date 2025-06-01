using System;

namespace DirectorySyncApp.Models
{
    public class FileItem
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public DateTime LastWriteTime { get; set; }
        public long Size { get; set; }

        public FileItem(string fullPath)
        {
            FullPath = fullPath;
            Name = System.IO.Path.GetFileName(fullPath);
            LastWriteTime = System.IO.File.GetLastWriteTime(fullPath);
            Size = new System.IO.FileInfo(fullPath).Length;
        }

        public override bool Equals(object obj)
        {
            if (obj is FileItem other)
                return FullPath.Equals(other.FullPath) &&
                       LastWriteTime.Equals(other.LastWriteTime) &&
                       Size == other.Size;
            return false;
        }

        public override int GetHashCode() => FullPath.GetHashCode();
    }
}