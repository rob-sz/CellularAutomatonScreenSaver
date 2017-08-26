using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenSaver.Windows.Forms
{
    public partial class MessageDialog : Form
    {
        private MessageDialog(Form parent, string text, string caption, MessageBoxButtons buttons)
        {
            InitializeComponent();
            InitializeButtons(buttons);

            Text = caption;
            Message.Text = text;
        }

        public static DialogResult Show(Form parent, string text, string caption)
        {
            return Show(parent, text, caption, MessageBoxButtons.OK);
        }

        public static DialogResult Show(Form parent, string text, string caption, MessageBoxButtons buttons)
        {
            return new MessageDialog(parent, text, caption, buttons).ShowDialog(parent);
        }

        private void InitializeButtons(MessageBoxButtons buttons)
        {
            switch (buttons)
            {
                case MessageBoxButtons.OKCancel:
                    Button1.Enabled = false;
                    Button1.Visible = false;
                    Button2.Text = "&OK";
                    Button2.DialogResult = DialogResult.OK;
                    Button3.Text = "&Cancel";
                    Button3.DialogResult = DialogResult.Cancel;
                    AcceptButton = Button2;
                    CancelButton = Button3;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    Button1.Text = "&Yes";
                    Button1.DialogResult = DialogResult.Yes;
                    Button2.Text = "&No";
                    Button2.DialogResult = DialogResult.No;
                    Button3.Text = "&Cancel";
                    Button3.DialogResult = DialogResult.Cancel;
                    AcceptButton = Button1;
                    CancelButton = Button3;
                    break;
                default:
                    Button1.Enabled = false;
                    Button1.Visible = false;
                    Button2.Enabled = false;
                    Button2.Visible = false;
                    Button3.Text = "&OK";
                    Button3.DialogResult = DialogResult.OK;
                    AcceptButton = Button3;
                    break;
            }
        }
    }
}
