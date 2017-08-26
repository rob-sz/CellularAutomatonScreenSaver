using System;
using System.Drawing;
using System.Windows.Forms;
using ScreenSaver.Windows.OS;

namespace ScreenSaver
{
    public partial class ScreenSaverForm : Form
    {
        private bool _fullScreen;
        private bool _changeWorld;
        private bool _changeRules;
        private int _changeWorldCount;
        private int _changeRulesCount;
        private DateTime _timerStart;
        private Timer _timer;
        private Point _mouseLocation;
        private CaWorld _world;
        private CaWorldOptions _worldOptions;
        private CaWorldGraphics _worldGraphics;
        private readonly CaWorldOptionsRepository _optionsRepository;

        public ScreenSaverForm(IntPtr previewWindowHandle)
        {
            InitializeComponent();

            _changeWorld = _changeRules = true;
            _optionsRepository = new CaWorldOptionsRepository();
            _fullScreen = previewWindowHandle == IntPtr.Zero;

            if (_fullScreen)
            {
#if (DEBUG)
                StartPosition = FormStartPosition.Manual;
                Location = new Point(Screen.PrimaryScreen.Bounds.Width - this.Width, 0);
#else
                Cursor.Hide();
                WindowState = FormWindowState.Maximized;
                TopMost = true;
#endif
            }
            else
            {
                // set the (new) parent window
                WinApi.SetParent(this.Handle, previewWindowHandle);

                // make this a child window, so it will properly receive a close message
                // from the systems screen saver properties dialog when a new screen saver 
                // is selected for the preview window
                WinApi.SetWindowLong(this.Handle, WinApi.GWL_STYLE,
                    WinApi.GetWindowLong(this.Handle, WinApi.GWL_STYLE) | WinApi.WS_CHILD);

                // get the parent window size and set it for this form
                Rectangle rect;
                WinApi.GetClientRect(previewWindowHandle, out rect);
                Size = rect.Size;
                Location = new Point(0, 0);
            }
        }

        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
            _mouseLocation = MousePosition;
            _worldGraphics = new CaWorldGraphics(CreateGraphics());

            _timer = new Timer { Interval = 1 };
            _timer.Tick += _timer_Tick;
            _timerStart = DateTime.Now;
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (_changeWorld)
            {
                _worldOptions = _optionsRepository.GetWorldOptions();

                _worldGraphics.Reset(
                    _worldOptions.CellSize,
                    _worldOptions.StateColors[0]);

                _world = new CaWorld(
                    _worldGraphics.CellCount,
                    _worldOptions.NumberOfStates,
                    _worldOptions.NumberOfNeighbours,
                    _worldOptions.IsIsotropic,
                    _worldOptions.Lambda);
                _world.ChangeWorld = true;

                _timer.Interval = _worldOptions.IterationInterval;

                _changeWorld = false;
            }

            if (_changeRules)
            {
                _world.ChangeRules = true;

                _changeRules = false;
            }

            if (_fullScreen && _worldOptions.IsSleepAfter &&
                DateTime.Now >= _timerStart.Add(_worldOptions.SleepAfter))
            {
                _worldGraphics.Reset(_worldOptions.CellSize, Color.Black);
                _timer.Stop();
            }
            else
            {
                _worldGraphics.DrawIteration(
                    _world.NextIteration(),
                    _worldOptions.CellSize,
                    _worldOptions.StateColors);

                if (_worldGraphics.IsNextScreen)
                {
                    _changeWorldCount++;
                    _changeRulesCount++;

                    if (_worldOptions.IsChangeWorldFrequency &&
                        _worldOptions.ChangeWorldFrequency == _changeWorldCount)
                    {
                        _changeWorld = true;
                        _changeWorldCount = 0;
                    }
                    if (_worldOptions.IsChangeRulesFrequency &&
                        _worldOptions.ChangeRulesFrequency == _changeRulesCount)
                    {
                        _changeRules = true;
                        _changeRulesCount = 0;
                    }
                }
            }
        }

        private void ScreenSaverForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_fullScreen) return;

            if (e.KeyCode == Keys.F5)
            {
                _changeWorld = true;
                return;
            }

            if (e.KeyCode == Keys.F6)
            {
                _changeRules = true;
                return;
            }

            if (e.KeyCode == Keys.F9)
            {
                //StopTimer();
                // ask user for profile name
                //   pass _optionsRepository to save dialog
                //   make sure profile does not exist
                //  _optionsRepository.SaveChangesAs(profileName);
                //StartTimer();
                return;
            }

            Exit();
        }

        private void ScreenSaverForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_fullScreen) return;

            Exit();
        }

        private void ScreenSaverForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_fullScreen) return;

            if ((Math.Abs(MousePosition.X - _mouseLocation.X) > 5) ||
                (Math.Abs(MousePosition.Y - _mouseLocation.Y) > 5))
            {
                Exit();
            }
        }

        private void Exit()
        {
            StopTimer();
            Application.Exit();
        }

        private void StopTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
        }

        private void StartTimer()
        {
            if (_timer != null)
            {
                _timer.Start();
            }
        }
    }
}
