using DirectorySyncApp.Presenters;
using DirectorySyncApp.Views;
using System;
using System.Windows.Forms;

namespace DirectorySyncApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var view = new MainForm();
            var presenter = new MainPresenter(view);

            Application.Run(view);
        }
    }
}