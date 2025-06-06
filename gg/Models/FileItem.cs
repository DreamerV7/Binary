using System;
using System.IO;

namespace DirectorySyncApp.Models
{
    public class FileItem
    {
        public string Name { get; }
        public string FullPath { get; }
        public DateTime LastWriteTime { get; }
        public long Size { get; }

        public FileItem(string fullPath)
        {
            FullPath = fullPath;
            Name = Path.GetFileName(fullPath);
            LastWriteTime = File.GetLastWriteTime(fullPath);
            Size = new FileInfo(fullPath).Length;
        }

        public override bool Equals(object obj)
        {
            return obj is FileItem other &&
                   FullPath.Equals(other.FullPath) &&
                   LastWriteTime.Equals(other.LastWriteTime) &&
                   Size == other.Size;
        }

        public override int GetHashCode() => FullPath.GetHashCode();
    }
}