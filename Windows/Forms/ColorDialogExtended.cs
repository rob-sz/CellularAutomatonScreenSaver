using System;
using System.Drawing;
using System.Windows.Forms;
using ScreenSaver.Windows.OS;

namespace ScreenSaver.Windows.Forms
{
    public class ColorDialogExtended : ColorDialog
    {
        public Point Location { get; set; }
        public string Text { get; set; }

        protected override IntPtr HookProc(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
        {
            IntPtr hookProc = base.HookProc(hWnd, msg, wparam, lparam);

            if (msg == WinApi.WM_INITDIALOG)
            {
                if (!String.IsNullOrEmpty(Text))
                {
                    WinApi.SetWindowText(hWnd, Text);
                }

                WinApi.SetWindowPos(hWnd, WinApi.HWND_TOP, Location.X, Location.Y, 0, 0, WinApi.UFLAGS);
            }

            return hookProc;
        }
    }
}
