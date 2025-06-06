using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DirectorySyncApp.Models
{
    public class DirectoryComparer
    {
        public enum ChangeType { Created, Updated, Deleted }

        public class ComparisonResult
        {
            public string FileName { get; set; }
            public ChangeType Change { get; set; }
            public string SourceDir { get; set; }
            public string TargetDir { get; set; }
        }

        public static List<ComparisonResult> Compare(string dir1, string dir2)
        {
            var results = new List<ComparisonResult>();

            if (!Directory.Exists(dir1) || !Directory.Exists(dir2))
                return results;

            var files1 = GetFilesRecursive(dir1);
            var files2 = GetFilesRecursive(dir2);

            CompareFiles(dir1, dir2, files1, files2, results);
            CompareFiles(dir2, dir1, files2, files1, results);

            return results;
        }

        private static void CompareFiles(string sourceDir, string targetDir,
            List<FileItem> sourceFiles, List<FileItem> targetFiles, List<ComparisonResult> results)
        {
            foreach (var file in sourceFiles)
            {
                var relativePath = GetRelativePath(sourceDir, file.FullPath);
                var correspondingFile = targetFiles.FirstOrDefault(f =>
                    GetRelativePath(targetDir, f.FullPath) == relativePath);

                if (correspondingFile == null)
                {
                    results.Add(new ComparisonResult
                    {
                        FileName = relativePath,
                        Change = ChangeType.Created,
                        SourceDir = sourceDir,
                        TargetDir = targetDir
                    });
                }
                else if (!file.Equals(correspondingFile))
                {
                    results.Add(new ComparisonResult
                    {
                        FileName = relativePath,
                        Change = ChangeType.Updated,
                        SourceDir = sourceDir,
                        TargetDir = targetDir
                    });
                }
            }
        }

        private static List<FileItem> GetFilesRecursive(string directory)
        {
            try
            {
                return Directory.GetFiles(directory, "*", SearchOption.AllDirectories)
                    .Select(f => new FileItem(f)).ToList();
            }
            catch
            {
                return new List<FileItem>();
            }
        }

        private static string GetRelativePath(string root, string fullPath)
        {
            return fullPath.Substring(root.Length).TrimStart(Path.DirectorySeparatorChar);
        }
    }
}