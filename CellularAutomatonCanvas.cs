using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSaver
{
    public class CellularAutomatonCanvas
    {
        private int _y;
        private Rectangle[] _cells;
        private Graphics _graphics;

        public CellularAutomatonCanvas(Graphics graphics, byte cellSize)
        {
            var cellCount = (int)Math.Ceiling((double)graphics.VisibleClipBounds.Width / cellSize);

            _cells = new Rectangle[cellCount];
            for (int i = 0; i < cellCount; i++)
            {
                _cells[i] = new Rectangle(i * cellSize, 0, cellSize, cellSize);
            }

            _graphics = graphics;
        }

        public void Reset()
        {
            _y = 0;
            _graphics.Clear(Color.Black);
        }
    }
}
