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

            // Получаем все файлы из обеих директорий
            var files1 = Directory.GetFiles(dir1, "*", SearchOption.AllDirectories)
                .Select(f => new FileItem(f)).ToList();
            var files2 = Directory.GetFiles(dir2, "*", SearchOption.AllDirectories)
                .Select(f => new FileItem(f)).ToList();

            // Находим файлы, которые есть в dir1, но нет в dir2
            foreach (var file in files1)
            {
                var relativePath = GetRelativePath(dir1, file.FullPath);
                var correspondingFile = files2.FirstOrDefault(f =>
                    GetRelativePath(dir2, f.FullPath) == relativePath);

                if (correspondingFile == null)
                {
                    results.Add(new ComparisonResult
                    {
                        FileName = relativePath,
                        Change = ChangeType.Created,
                        SourceDir = dir1,
                        TargetDir = dir2
                    });
                }
                else if (!file.Equals(correspondingFile))
                {
                    results.Add(new ComparisonResult
                    {
                        FileName = relativePath,
                        Change = ChangeType.Updated,
                        SourceDir = dir1,
                        TargetDir = dir2
                    });
                }
            }

            // Находим файлы, которые есть в dir2, но нет в dir1
            foreach (var file in files2)
            {
                var relativePath = GetRelativePath(dir2, file.FullPath);
                if (!files1.Any(f => GetRelativePath(dir1, f.FullPath) == relativePath))
                {
                    results.Add(new ComparisonResult
                    {
                        FileName = relativePath,
                        Change = ChangeType.Deleted,
                        SourceDir = dir2,
                        TargetDir = dir1
                    });
                }
            }

            return results;
        }

        private static string GetRelativePath(string root, string fullPath)
        {
            return fullPath.Substring(root.Length).TrimStart(Path.DirectorySeparatorChar);
        }
    }
}