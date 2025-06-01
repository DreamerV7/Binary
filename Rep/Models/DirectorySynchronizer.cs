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
                var sourcePath = Path.Combine(change.SourceDir, change.FileName);
                var targetPath = Path.Combine(change.TargetDir, change.FileName);

                try
                {
                    switch (change.Change)
                    {
                        case DirectoryComparer.ChangeType.Created:
                        case DirectoryComparer.ChangeType.Updated:
                            EnsureDirectoryExists(targetPath);
                            File.Copy(sourcePath, targetPath, true);
                            SyncActionPerformed?.Invoke($"Файл \"{change.FileName}\" скопирован из {change.SourceDir} в {change.TargetDir}");
                            break;
                        case DirectoryComparer.ChangeType.Deleted:
                            if (File.Exists(targetPath))
                            {
                                File.Delete(targetPath);
                                SyncActionPerformed?.Invoke($"Файл \"{change.FileName}\" удален из {change.TargetDir}");
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    SyncActionPerformed?.Invoke($"Ошибка при синхронизации файла \"{change.FileName}\": {ex.Message}");
                }
            }
        }

        private void EnsureDirectoryExists(string filePath)
        {
            var dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}