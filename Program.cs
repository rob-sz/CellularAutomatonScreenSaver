using System;
using System.Windows.Forms;

namespace ScreenSaver
{
    public class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {
                if (args[0].ToLower().Trim().Substring(0, 2) == "/c")
                {
                    MessageBox.Show(
                        "This Screen Saver has no options you can set.",
                        ".NET Screen Saver",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
                else if (args[0].ToLower() == "/s")
                {
                    for (int i = Screen.AllScreens.GetLowerBound(0); i <= Screen.AllScreens.GetUpperBound(0); i++)
                        Application.Run(new ScreenSaverForm(i));
                }
            }
            else
            {
                for (int i = Screen.AllScreens.GetLowerBound(0); i <= Screen.AllScreens.GetUpperBound(0); i++)
                    Application.Run(new ScreenSaverForm(i));
            }
        }
    }
}
