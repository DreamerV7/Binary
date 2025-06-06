using System;
using System.Collections.Generic;
using System.IO;

namespace DirectorySyncApp.Models
{
    public class DirectorySynchronizer
    {
        public event Action<string> SyncActionPerformed;

        public void Synchronize(List<DirectoryComparer.ComparisonResult> changes)
        {
            foreach (var change in changes)
            {
                try
                {
                    ProcessChange(change);
                }
                catch (Exception ex)
                {
                    SyncActionPerformed?.Invoke($"Ошибка при обработке файла {change.FileName}: {ex.Message}");
                }
            }
        }

        private void ProcessChange(DirectoryComparer.ComparisonResult change)
        {
            var sourcePath = Path.Combine(change.SourceDir, change.FileName);
            var targetPath = Path.Combine(change.TargetDir, change.FileName);

            switch (change.Change)
            {
                case DirectoryComparer.ChangeType.Created:
                case DirectoryComparer.ChangeType.Updated:
                    EnsureDirectoryExists(Path.GetDirectoryName(targetPath));
                    File.Copy(sourcePath, targetPath, true);
                    SyncActionPerformed?.Invoke($"Файл \"{change.FileName}\" скопирован");
                    break;

                case DirectoryComparer.ChangeType.Deleted:
                    if (File.Exists(targetPath))
                    {
                        File.Delete(targetPath);
                        SyncActionPerformed?.Invoke($"Файл \"{change.FileName}\" удален");
                    }
                    break;
            }
        }

        private void EnsureDirectoryExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}