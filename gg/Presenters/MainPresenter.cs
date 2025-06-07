using DirectorySyncApp.Models;
using DirectorySyncApp.Views;
using System;
using System.Windows.Forms;

namespace DirectorySyncApp.Presenters
{
    public class MainPresenter
    {
        private readonly IMainView _view;
        private readonly DirectorySynchronizer _synchronizer;

        public MainPresenter(IMainView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _synchronizer = new DirectorySynchronizer();
            _synchronizer.SyncActionPerformed += OnSyncActionPerformed;

            SubscribeToViewEvents();
        }

        private void SubscribeToViewEvents()
        {
            _view.SelectSourceDirectory += OnSelectSourceDirectory;
            _view.SelectTargetDirectory += OnSelectTargetDirectory;
            _view.CompareDirectories += OnCompareDirectories;
            _view.SynchronizeDirectories += OnSynchronizeDirectories;
        }

        private void OnSelectSourceDirectory(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _view.SourceDirectory = dialog.SelectedPath;
                }
            }
        }

        private void OnSelectTargetDirectory(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _view.TargetDirectory = dialog.SelectedPath;
                }
            }
        }

        private void OnCompareDirectories(object sender, EventArgs e)
        {
            if (!ValidateDirectories()) return;

            try
            {
                _view.Log = "=== Результаты сравнения ===\r\n";
                var changes = DirectoryComparer.Compare(_view.SourceDirectory, _view.TargetDirectory);

                if (changes.Count == 0)
                {
                    _view.Log += "Директории идентичны\r\n";
                    return;
                }

                foreach (var change in changes)
                {
                    _view.Log += FormatChangeMessage(change);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сравнении: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnSynchronizeDirectories(object sender, EventArgs e)
        {
            if (!ValidateDirectories()) return;

            try
            {
                _view.Log = "=== Начало синхронизации ===\r\n";
                var changes = DirectoryComparer.Compare(_view.SourceDirectory, _view.TargetDirectory);
                _synchronizer.Synchronize(changes);
                _view.Log += "=== Синхронизация завершена ===\r\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при синхронизации: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateDirectories()
        {
            if (string.IsNullOrEmpty(_view.SourceDirectory) || string.IsNullOrEmpty(_view.TargetDirectory))
            {
                MessageBox.Show("Пожалуйста, выберите обе директории", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private string FormatChangeMessage(DirectoryComparer.ComparisonResult change)
        {
            switch (change.Change)
            {
                case DirectoryComparer.ChangeType.Created:
                    return $"• Файл \"{change.FileName}\" создан\r\n";
                case DirectoryComparer.ChangeType.Updated:
                    return $"• Файл \"{change.FileName}\" изменен\r\n";
                case DirectoryComparer.ChangeType.Deleted:
                    return $"• Файл \"{change.FileName}\" удален\r\n";
                default:
                    return $"• Файл \"{change.FileName}\" - неизвестное изменение\r\n";
            }
        }

        private void OnSyncActionPerformed(string message)
        {
            _view.Log += message + "\r\n";
        }
    }
}