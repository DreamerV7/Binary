using System;
using System.Windows.Forms;

namespace DirectorySyncApp.Views
{
    public partial class MainForm : Form, IMainView
    {
        public MainForm()
        {
            InitializeComponent();
            btnSelectSource.Click += (s, e) => SelectSourceDirectory?.Invoke(this, EventArgs.Empty);
            btnSelectTarget.Click += (s, e) => SelectTargetDirectory?.Invoke(this, EventArgs.Empty);
            btnCompare.Click += (s, e) => CompareDirectories?.Invoke(this, EventArgs.Empty);
            btnSync.Click += (s, e) => SynchronizeDirectories?.Invoke(this, EventArgs.Empty);
        }

        public string SourceDirectory
        {
            get => txtSource.Text;
            set => txtSource.Text = value;
        }

        public string TargetDirectory
        {
            get => txtTarget.Text;
            set => txtTarget.Text = value;
        }

        public string Log
        {
            get => txtLog.Text;
            set => txtLog.Text = value;
        }

        public event EventHandler SelectSourceDirectory;
        public event EventHandler SelectTargetDirectory;
        public event EventHandler CompareDirectories;
        public event EventHandler SynchronizeDirectories;

        private void InitializeComponent()
        {
            // Инициализация компонентов формы
            this.txtSource = new TextBox();
            this.txtTarget = new TextBox();
            this.btnSelectSource = new Button();
            this.btnSelectTarget = new Button();
            this.btnCompare = new Button();
            this.btnSync = new Button();
            this.txtLog = new RichTextBox();
            this.SuspendLayout();

            // txtSource
            this.txtSource.Location = new System.Drawing.Point(12, 12);
            this.txtSource.Size = new System.Drawing.Size(300, 20);

            // txtTarget
            this.txtTarget.Location = new System.Drawing.Point(12, 38);
            this.txtTarget.Size = new System.Drawing.Size(300, 20);

            // btnSelectSource
            this.btnSelectSource.Location = new System.Drawing.Point(318, 10);
            this.btnSelectSource.Text = "Выбрать...";
            this.btnSelectSource.Size = new System.Drawing.Size(75, 23);

            // btnSelectTarget
            this.btnSelectTarget.Location = new System.Drawing.Point(318, 36);
            this.btnSelectTarget.Text = "Выбрать...";
            this.btnSelectTarget.Size = new System.Drawing.Size(75, 23);

            // btnCompare
            this.btnCompare.Location = new System.Drawing.Point(12, 64);
            this.btnCompare.Text = "Сравнить";
            this.btnCompare.Size = new System.Drawing.Size(75, 23);

            // btnSync
            this.btnSync.Location = new System.Drawing.Point(93, 64);
            this.btnSync.Text = "Синхронизировать";
            this.btnSync.Size = new System.Drawing.Size(120, 23);

            // txtLog
            this.txtLog.Location = new System.Drawing.Point(12, 93);
            this.txtLog.Size = new System.Drawing.Size(381, 246);
            this.txtLog.ReadOnly = true;

            // MainForm
            this.ClientSize = new System.Drawing.Size(405, 351);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnSync);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.btnSelectTarget);
            this.Controls.Add(this.btnSelectSource);
            this.Controls.Add(this.txtTarget);
            this.Controls.Add(this.txtSource);
            this.Text = "Синхронизация директорий";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private TextBox txtSource;
        private TextBox txtTarget;
        private Button btnSelectSource;
        private Button btnSelectTarget;
        private Button btnCompare;
        private Button btnSync;
        private RichTextBox txtLog;
    }
}