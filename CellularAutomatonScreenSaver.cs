using System;
using System.Collections.Generic;
using System.Drawing;

namespace ScreenSaver
{
    public class CellularAutomatonScreenSaver : IScreenSaver
    {
        private readonly Random _random = new Random();

        public Color[] XAvailableColors =
        {
            Color.Red,
            Color.Green,
            Color.Blue
            //Color.Brown,
            //Color.Orange,
            //Color.Purple,
            //Color.Yellow
        };

        internal class Rgb
        {
            internal int Red { get; set; }
            internal int Green { get; set; }
            internal int Blue { get; set; }
        }

        internal class Cell
        {
            //internal Color Color { get; set; }
            //internal int ColorIndex { get; set; }
            internal Rgb Rgb { get; set; }
            internal Rectangle Rectangle { get; set; }
        }

        private const int CellSize = 2;
        private const int NeighbourCount = 5;
        private int _cellsY;
        private Rgb[] _prevRgb;
        private Cell[] _cells;

        public void Iterate(Canvas canvas)
        {
            if (_cells == null)
            {
                _cellsY = 0;

                var cellCount = (int) Math.Ceiling((double) canvas.Size.Width / CellSize);

                _cells = new Cell[cellCount];
                for (int i = 0; i < _cells.Length; i++)
                {
                    //int colorIndex = _random.Next(AvailableColors.Length);
                    _cells[i] = new Cell
                    {
                        //Color = AvailableColors[colorIndex],
                        //ColorIndex = colorIndex,
                        Rgb = new Rgb
                        {
                            Red = _random.Next(256),
                            Green = _random.Next(256),
                            Blue = _random.Next(256)
                        },
                        Rectangle = new Rectangle(i * CellSize, 0, CellSize, CellSize)
                    };
                }

                _prevRgb = new Rgb[_cells.Length];
            }
            else
            {
                _cellsY += CellSize;
                if (_cellsY + CellSize >= canvas.Size.Height) _cellsY = 0;

                for (int i = 0; i < _cells.Length; i++)
                {
                    var neighbourRgb = new List<Rgb>();
                    neighbourRgb.Add(_prevRgb[i]);
                    for (int n = 1; n <= NeighbourCount; n++)
                    {
                        int prevCellIndex = i - n;
                        if (prevCellIndex < 0) prevCellIndex = _cells.Length + prevCellIndex;
                        neighbourRgb.Add(_prevRgb[prevCellIndex]);
                    }
                    for (int n = 1; n <= NeighbourCount; n++)
                    {
                        int nextCellIndex = i + n;
                        if (nextCellIndex >= _cells.Length) nextCellIndex = nextCellIndex - _cells.Length;
                        neighbourRgb.Add(_prevRgb[nextCellIndex]);
                    }

                    int red = 0;
                    int green = 0;
                    int blue = 0;
                    foreach (var rgb in neighbourRgb)
                    {
                        red += rgb.Red;
                        green += rgb.Green;
                        blue += rgb.Blue;
                    }
                    red /= neighbourRgb.Count;
                    green /= neighbourRgb.Count;
                    blue /= neighbourRgb.Count;

                    //int colorIndex = 0;
                    //foreach (int index in neighbourColorIndexes) colorIndex += index;
                    //colorIndex /= neighbourColorIndexes.Count;

                    _cells[i].Rgb.Red = red;
                    _cells[i].Rgb.Green = green;
                    _cells[i].Rgb.Blue = blue;
                    //_cells[i].ColorIndex = colorIndex;
                }
            }

            for (int i = 0; i < _cells.Length; i ++)
            {
                _prevRgb[i] = _cells[i].Rgb;
            }

            DrawCells(canvas);
        }

        private void DrawCells(Canvas canvas)
        {
            using (var bitmap = new Bitmap(canvas.Size.Width, CellSize))
            {
                using (var brush = new SolidBrush(Color.Black))
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    for (int i = 0; i < _cells.Length; i++)
                    {
                        brush.Color = Color.FromArgb(_cells[i].Rgb.Red, _cells[i].Rgb.Green, _cells[i].Rgb.Blue);
                        graphics.FillRectangle(brush, _cells[i].Rectangle);
                    }
                }

                canvas.Graphics.DrawImage(bitmap, 0, _cellsY);
            }
        }
    }
}
