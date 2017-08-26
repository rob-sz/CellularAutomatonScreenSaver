using System;
using System.Drawing;

namespace ScreenSaver
{
    public class CaWorldGraphics
    {
        private int _y;
        private Rectangle[] _cells;
        private readonly Graphics _graphics;

        public CaWorldGraphics(Graphics graphics)
        {
            _graphics = graphics;
        }

        public int CellCount
        {
            get { return _cells.Length; }
        }

        public bool IsNextScreen
        {
            get { return _y == 0; }
        }

        public void Reset(byte cellSize, Color backgroundColor)
        {
            _y = 0;

            _cells = new Rectangle[(int)Math.Ceiling((double)_graphics.VisibleClipBounds.Width / cellSize)];
            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i] = new Rectangle(i * cellSize, 0, cellSize, cellSize);
            }

            _graphics.Clear(backgroundColor);
        }

        public void DrawIteration(byte[] iteration, byte cellSize, Color[] stateColors)
        {
            using (var bitmap = new Bitmap((int)_graphics.VisibleClipBounds.Width, cellSize))
            {
                using (var brush = new SolidBrush(stateColors[0]))
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    for (int i = 0; i < iteration.Length; i++)
                    {
                        brush.Color = stateColors[iteration[i]];
                        graphics.FillRectangle(brush, _cells[i]);
                    }
                }

                _graphics.DrawImage(bitmap, 0, _y);
                _y += cellSize;
            }

            if (_y >= _graphics.VisibleClipBounds.Height) _y = 0;
        }
    }
}
