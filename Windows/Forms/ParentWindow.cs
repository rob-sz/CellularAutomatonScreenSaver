using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScreenSaver.Windows.OS;

namespace ScreenSaver.Windows.Forms
{
    public class ParentWindow : IWin32Window
    {
        private IntPtr _hwnd;

        public ParentWindow(IntPtr hwnd)
        {
            _hwnd = hwnd;
        }

        public IntPtr Handle
        {
            get { return WinApi.GetParent(_hwnd); }
        }

        public Rectangle Bounds
        {
            get
            {
                WinApi.Rect rect = new WinApi.Rect();
                if (WinApi.GetWindowRect(Handle, ref rect))
                {
                    return new Rectangle(
                        rect.Left, rect.Top,
                        rect.Right - rect.Left, rect.Bottom - rect.Top);
                }

                return Rectangle.Empty;
            }
        }
    }
}
