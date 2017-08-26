using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ScreenSaver.Windows.Forms
{
    public class ComboBoxExtended : ComboBox
    {
        public int LastSelectedIndex { get; set; }

        public ComboBoxExtended()
        {
            LastSelectedIndex = -1;
        }

        public event CancelEventHandler SelectedIndexChanging;

        protected void OnSelectedIndexChanging(CancelEventArgs e)
        {
            if (SelectedIndexChanging != null)
            {
                SelectedIndexChanging(this, e);
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (LastSelectedIndex != SelectedIndex)
            {
                var cancelEventArgs = new CancelEventArgs();
                OnSelectedIndexChanging(cancelEventArgs);

                if (cancelEventArgs.Cancel)
                {
                    SelectedIndex = LastSelectedIndex;
                }
                else
                {
                    LastSelectedIndex = SelectedIndex;
                    base.OnSelectedIndexChanged(e);
                }
            }
        }
    }
}
