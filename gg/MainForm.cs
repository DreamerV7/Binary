using System;
using System.Windows.Forms;

namespace DirectorySyncApp.Views
{
    public partial class MainForm : Form, IMainView
    {
        public MainForm()
        {
            // Удаляем вызов InitializeComponent() - будем создавать компоненты вручную
            SetupControls();
            WireUpEvents();
        }

        // Остальной код остается без изменений
        public string SourceDirectory { get => txtSource.Text; set => txtSource.Text = value; }
        public string TargetDirectory { get => txtTarget.Text; set => txtTarget.Text = value; }
        public string Log { get => txtLog.Text; set => txtLog.Text = value; }

        public event EventHandler SelectSourceDirectory;
        public event EventHandler SelectTargetDirectory;
        public event EventHandler CompareDirectories;
        public event EventHandler SynchronizeDirectories;

        private TextBox txtSource;
        private TextBox txtTarget;
        private Button btnSelectSource;
        private Button btnSelectTarget;
        private Button btnCompare;
        private Button btnSync;
        private RichTextBox txtLog;

        private void SetupControls()
        {
            // Убедимся, что все компоненты инициализированы
            this.txtSource = new TextBox();
            this.txtTarget = new TextBox();
            this.btnSelectSource = new Button();
            this.btnSelectTarget = new Button();
            this.btnCompare = new Button();
            this.btnSync = new Button();
            this.txtLog = new RichTextBox();

            // Настройка свойств формы
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(500, 400);
            this.Text = "Синхронизация директорий";
            this.MinimumSize = new System.Drawing.Size(500, 400);

            // Настройка txtSource
            this.txtSource.Location = new System.Drawing.Point(12, 12);
            this.txtSource.Size = new System.Drawing.Size(350, 20);
            this.txtSource.Name = "txtSource";
            this.txtSource.TabIndex = 0;

            // Настройка txtTarget
            this.txtTarget.Location = new System.Drawing.Point(12, 42);
            this.txtTarget.Size = new System.Drawing.Size(350, 20);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.TabIndex = 1;

            // Настройка btnSelectSource
            this.btnSelectSource.Location = new System.Drawing.Point(370, 10);
            this.btnSelectSource.Size = new System.Drawing.Size(100, 23);
            this.btnSelectSource.Text = "Выбрать...";
            this.btnSelectSource.Name = "btnSelectSource";
            this.btnSelectSource.TabIndex = 2;

            // Настройка btnSelectTarget
            this.btnSelectTarget.Location = new System.Drawing.Point(370, 40);
            this.btnSelectTarget.Size = new System.Drawing.Size(100, 23);
            this.btnSelectTarget.Text = "Выбрать...";
            this.btnSelectTarget.Name = "btnSelectTarget";
            this.btnSelectTarget.TabIndex = 3;

            // Настройка btnCompare
            this.btnCompare.Location = new System.Drawing.Point(12, 72);
            this.btnCompare.Size = new System.Drawing.Size(100, 30);
            this.btnCompare.Text = "Сравнить";
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.TabIndex = 4;

            // Настройка btnSync
            this.btnSync.Location = new System.Drawing.Point(120, 72);
            this.btnSync.Size = new System.Drawing.Size(150, 30);
            this.btnSync.Text = "Синхронизировать";
            this.btnSync.Name = "btnSync";
            this.btnSync.TabIndex = 5;

            // Настройка txtLog
            this.txtLog.Location = new System.Drawing.Point(12, 110);
            this.txtLog.Size = new System.Drawing.Size(460, 280);
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.txtLog.Name = "txtLog";
            this.txtLog.TabIndex = 6;

            // Добавление компонентов на форму
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.txtTarget);
            this.Controls.Add(this.btnSelectSource);
            this.Controls.Add(this.btnSelectTarget);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.btnSync);
            this.Controls.Add(this.txtLog);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void WireUpEvents()
        {
            this.btnSelectSource.Click += (s, e) => SelectSourceDirectory?.Invoke(this, EventArgs.Empty);
            this.btnSelectTarget.Click += (s, e) => SelectTargetDirectory?.Invoke(this, EventArgs.Empty);
            this.btnCompare.Click += (s, e) => CompareDirectories?.Invoke(this, EventArgs.Empty);
            this.btnSync.Click += (s, e) => SynchronizeDirectories?.Invoke(this, EventArgs.Empty);
        }
    }
}