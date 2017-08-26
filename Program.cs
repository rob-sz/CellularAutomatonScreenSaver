using System;
using System.Drawing;
using System.Windows.Forms;
using ScreenSaver.Options;
using ScreenSaver.Windows.Forms;

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
                string arg1 = args[0].ToLower().Trim();
                string arg2 = null;

                // Arguments can be separated by colon, eg. /c:1234567
                if (arg1.Length > 2)
                {
                    arg2 = arg1.Substring(3).Trim();
                    arg1 = arg1.Substring(0, 2);
                }
                else if (args.Length > 1)
                {
                    arg2 = args[1];
                }

                IntPtr previewWindowHandle = IntPtr.Zero;
                if (arg2 != null)
                {
                    long pointerValue;
                    if (long.TryParse(arg2, out pointerValue))
                    {
                        previewWindowHandle = new IntPtr(pointerValue);
                    }
                }

                switch (arg1)
                {
                    case "/c": // configure
#if (DEBUG)
                        new UserOptionsForm(Point.Empty).ShowDialog();
#else
                        if (previewWindowHandle != IntPtr.Zero)
                        {
                            var screenSaverSettingsWindow = new ParentWindow(previewWindowHandle);
                            var screenSaverSettingsWindowBounds = screenSaverSettingsWindow.Bounds;
                            if (screenSaverSettingsWindowBounds != Rectangle.Empty)
                            {
                                var userOptionsForm = new UserOptionsForm(
                                    new Point(
                                        screenSaverSettingsWindowBounds.X + 10,
                                        screenSaverSettingsWindowBounds.Y + 10));

                                userOptionsForm.ShowDialog(screenSaverSettingsWindow);
                            }
                        }
                        else
                        {
                            new UserOptionsForm(Point.Empty).ShowDialog();
                        }
#endif
                        return;

                    case "/p": // preview
                        if (previewWindowHandle != IntPtr.Zero)
                        {
                            Application.Run(new ScreenSaverForm(previewWindowHandle));
                        }
                        break;

                    case "/s": // show
                    default:
                        foreach (Screen screen in Screen.AllScreens)
                        {
                            Application.Run(new ScreenSaverForm(IntPtr.Zero));
                        }
                        break;
                }
            }
        }
    }
}
