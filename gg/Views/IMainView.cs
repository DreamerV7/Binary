using System;

namespace DirectorySyncApp.Views
{
    public interface IMainView
    {
        string SourceDirectory { get; set; }
        string TargetDirectory { get; set; }
        string Log { get; set; }

        event EventHandler SelectSourceDirectory;
        event EventHandler SelectTargetDirectory;
        event EventHandler CompareDirectories;
        event EventHandler SynchronizeDirectories;
    }
}