using System;
using System.Windows.Forms;

namespace Tyuiu.RadchenkoAV.Sprint7.ProjectV10
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain_RAV());
        }
    }
}