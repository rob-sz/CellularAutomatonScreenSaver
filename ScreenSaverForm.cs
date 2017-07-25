using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScreenSaver
{
    public partial class ScreenSaverForm : Form
    {
        private Timer _timer;
        private CellularAutomatonWorld _screenSaver;
        private Point _mouseLocation;
        private Canvas _canvas;
        private bool _isReset;

        public ScreenSaverForm(int screenIndex)
        {
            InitializeComponent();
        }

        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
#if (!DEBUG)
            Cursor.Hide();
            WindowState = FormWindowState.Maximized;
#else
            StartPosition = FormStartPosition.Manual;
            Location = new Point(Screen.PrimaryScreen.Bounds.Width - this.Width, 0);
#endif
            _screenSaver = new CellularAutomatonWorld
            {
                CellSize = 2,
                IsIsotropic = true,
                NeighbourhoodSize = 5,
                NumberOfStates = 5,
                RulesUsed = 537
            };
            _mouseLocation = MousePosition;
            _canvas = new Canvas
            {
                Graphics = CreateGraphics(),
                Size = Size
            };

            _timer = new Timer { Interval = 1 };
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _screenSaver.IsReset = _isReset;
            _isReset = false;
            _screenSaver.Iterate(_canvas);
        }

        private void ScreenSaverForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.N)
            {
                _isReset = true;
                return;
            }

            Stop();
        }

        private void ScreenSaverForm_MouseDown(object sender, MouseEventArgs e)
        {
            Stop();
        }

        private void ScreenSaverForm_MouseMove(object sender, MouseEventArgs e)
        {
            if ((Math.Abs(MousePosition.X - _mouseLocation.X) > 5) ||
                (Math.Abs(MousePosition.Y - _mouseLocation.Y) > 5))
            {
                Stop();
            }
        }

        private void Stop()
        {
            if (_timer != null)
                _timer.Stop();
            Close();
        }
    }
}
