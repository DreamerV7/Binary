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
            _view = view;
            _synchronizer = new DirectorySynchronizer();
            _synchronizer.SyncActionPerformed += OnSyncActionPerformed;

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
            if (string.IsNullOrEmpty(_view.SourceDirectory) || string.IsNullOrEmpty(_view.TargetDirectory))
            {
                MessageBox.Show("Выберите обе директории для сравнения");
                return;
            }

            var changes = DirectoryComparer.Compare(_view.SourceDirectory, _view.TargetDirectory);
            _view.Log = "Результаты сравнения:\r\n";

            foreach (var change in changes)
            {
                string action = change.Change switch
                {
                    DirectoryComparer.ChangeType.Created => "новый",
                    DirectoryComparer.ChangeType.Updated => "изменен",
                    DirectoryComparer.ChangeType.Deleted => "удален",
                    _ => "неизвестное действие"
                };

                _view.Log += $"• Файл \"{change.FileName}\" {action}\r\n";
            }

            if (changes.Count == 0)
            {
                _view.Log += "Директории идентичны\r\n";
            }
        }

        private void OnSynchronizeDirectories(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_view.SourceDirectory) || string.IsNullOrEmpty(_view.TargetDirectory))
            {
                MessageBox.Show("Выберите обе директории для синхронизации");
                return;
            }

            _view.Log = "Начало синхронизации...\r\n";

            // Синхронизация в обе стороны
            var changes1 = DirectoryComparer.Compare(_view.SourceDirectory, _view.TargetDirectory);
            var changes2 = DirectoryComparer.Compare(_view.TargetDirectory, _view.SourceDirectory);

            _synchronizer.Synchronize(changes1);
            _synchronizer.Synchronize(changes2);

            _view.Log += "Синхронизация завершена\r\n";
        }

        private void OnSyncActionPerformed(string message)
        {
            _view.Log += message + "\r\n";
        }
    }
}