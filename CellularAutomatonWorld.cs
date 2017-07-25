using System;
using System.Collections.Generic;
using System.Drawing;

namespace ScreenSaver
{
    public class CellularAutomatonWorld
    {
        private readonly Random _random = new Random();
        private byte[] _ruleValue;
        private bool[] _ruleIsUsed;
        private byte[] _currentWorld;
        private byte[] _nextWorld;
        private int[] _lambdaPath;

        // rectangle does not belong here: separate class for drawing
        //private Rectangle[] _rectangles;
        //private Color[] _cellValueColor;

        public bool IsIsotropic { get; set; }
        public byte NeighbourhoodSize { get; set; }
        public byte NumberOfStates { get; set; } // 2 >= N <=32
        public byte CellSize { get; set; }
        public int RulesUsed { get; set; }
        public int RulesTotal { get { return _ruleValue == null ? 0 : _ruleValue.Length; } }
        public bool IsReset { get; set; }

        //private long _startTicks;
        //private long _endTicks;

        // canvas does not belong here: separate class for drawing
        public void Iterate(Canvas canvas)
        {
            if (_currentWorld == null || IsReset)
            {
                _cellsY = 0;
                Initialize(canvas);

                //_startTicks = DateTime.Now.Ticks;
            }
            else
            {
                _cellsY += CellSize;
                if (_cellsY + CellSize >= canvas.Size.Height)
                {
                    //_endTicks = DateTime.Now.Ticks;

                    _cellsY = 0;
                }

                for (int i = 0; i < _nextWorld.Length; i++)
                {
                    int code = NeighbourhoodCode(i);
                    _nextWorld[i] = _ruleIsUsed[code]
                        ? _ruleValue[code]
                        : (byte)0;
                }

                Array.Copy(_nextWorld, _currentWorld, _nextWorld.Length);
            }

            DrawCells(canvas);
        }

        private void Initialize(Canvas canvas)
        {
            ClearScreen(canvas);

            _cellValueColor = new Color[NumberOfStates];
            _cellValueColor[0] = Color.Black;
            for (byte i = 1; i < NumberOfStates; i++)
            {
                int red = _random.Next(256);
                int green = _random.Next(256);
                int blue = _random.Next(256);
                _cellValueColor[i] = Color.FromArgb(red, green, blue);
            }

            var ruleCount = (int)Math.Pow(NumberOfStates, NeighbourhoodSize);

            _ruleValue = new byte[ruleCount];
            _ruleIsUsed = new bool[ruleCount];

            for (int i = 1; i < ruleCount; i++)
            {
                _ruleValue[i] = (byte)_random.Next(1, NumberOfStates);
                if (IsIsotropic)
                {
                    _ruleValue[IsotropicPartner(i)] = _ruleValue[i];
                }
            }

            var lambdaCount = IsIsotropic
                ? ((ruleCount + (int)Math.Pow(NumberOfStates, (NeighbourhoodSize + 1) / 2)) / 2) - 1
                : ruleCount - 1;

            _lambdaPath = new int[lambdaCount];

            int lambdaIndex = 0;
            for (int i = 1; i < ruleCount; i++)
            {
                if (!_ruleIsUsed[i])
                {
                    _lambdaPath[lambdaIndex++] = i;
                    _ruleIsUsed[i] = true;
                    if (IsIsotropic)
                    {
                        _ruleIsUsed[IsotropicPartner(i)] = true;
                    }
                }
            }

            for (int i = 0; i < lambdaCount; i++)
            {
                var randomLambdaIndex = _random.Next(lambdaCount);
                int currentLambdaValue = _lambdaPath[i];
                _lambdaPath[i] = _lambdaPath[randomLambdaIndex];
                _lambdaPath[randomLambdaIndex] = currentLambdaValue;
            }

            var cellCount = (int)Math.Ceiling((double)canvas.Size.Width / CellSize);

            _currentWorld = new byte[cellCount];
            _nextWorld = new byte[cellCount];
            _rectangles = new Rectangle[cellCount];
            for (int i = 0; i < cellCount; i++)
            {
                _currentWorld[i] = (byte)_random.Next(NumberOfStates);

                // rectangle does not belong here: separate class for drawing
                _rectangles[i] = new Rectangle(i * CellSize, 0, CellSize, CellSize);
            }

            InitializeRulesUsed(RulesUsed);
        }

        private int IsotropicPartner(int n)
        {
            int isotropicPartner = 0;
            for (int i = 0; i < NeighbourhoodSize; i++)
            {
                isotropicPartner = isotropicPartner * NumberOfStates + (n % NumberOfStates);
                n /= NumberOfStates;
            }
            return isotropicPartner;
        }

        private void InitializeRulesUsed(int n)
        {
            if (n < 0)
            {
                n = 0;
            }
            else if (n > _lambdaPath.Length)
            {
                n = _lambdaPath.Length;
            }
            if (IsIsotropic)
            {
                for (int i = 0; i < n; i++)
                {
                    int r = _lambdaPath[i];
                    _ruleIsUsed[r] = _ruleIsUsed[IsotropicPartner(r)] = true;
                }
                for (int i = n; i < _lambdaPath.Length; i++)
                {
                    int r = _lambdaPath[i];
                    _ruleIsUsed[r] = _ruleIsUsed[IsotropicPartner(r)] = false;
                }
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    _ruleIsUsed[_lambdaPath[i]] = true;
                }
                for (int i = n; i < _lambdaPath.Length; i++)
                {
                    _ruleIsUsed[_lambdaPath[i]] = false;
                }
            }
        }

        private byte GetState(int n)
        {
            if (n < 0)
            {
                n += _currentWorld.Length;
            }
            else if (n >= _currentWorld.Length)
            {
                n -= _currentWorld.Length;
            }

            return _currentWorld[n];
        }

        private int NeighbourhoodCode(int n)
        {
            int code = 0;
            int offset = n - (NeighbourhoodSize / 2);
            for (int i = 0; i < NeighbourhoodSize; i++)
            {
                code = code * NumberOfStates + GetState(i + offset);
            }

            return code;
        }

        // draw cells does not belong here: separate class for drawing
        private void DrawCells(Canvas canvas)
        {
            using (var bitmap = new Bitmap(canvas.Size.Width, CellSize))
            {
                using (var brush = new SolidBrush(Color.Black))
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    for (int i = 0; i < _currentWorld.Length; i++)
                    {
                        brush.Color = _cellValueColor[_currentWorld[i]];
                        graphics.FillRectangle(brush, _rectangles[i]);
                    }
                }

                canvas.Graphics.DrawImage(bitmap, 0, _cellsY);
            }
        }
    }
}
