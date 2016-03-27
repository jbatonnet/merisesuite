using System;
using System.Windows.Forms;

namespace MeriseSuite
{
    static class Program
    {
        public static int Id = 0;

        [STAThread]
        static void Main(params string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
