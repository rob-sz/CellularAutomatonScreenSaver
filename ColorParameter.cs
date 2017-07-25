using System;
using System.Drawing;

namespace ScreenSaver
{
    public class ColorParameter
    {
        private Color _value;
        private byte _rangeFrom;
        private byte _rangeTo;
        private Random _random;

        public ColorParameter(Color value)
        {
            _value = value;
        }

        public ColorParameter(byte rangeFrom, byte rangeTo)
        {
            _rangeFrom = rangeFrom;
            _rangeTo = rangeTo;
            _random = new Random();
        }

        public Color Value
        {
            get
            {
                if (_random != null)
                {
                    return Color.FromArgb(
                        _random.Next(_rangeFrom, _rangeTo + 1),
                        _random.Next(_rangeFrom, _rangeTo + 1),
                        _random.Next(_rangeFrom, _rangeTo + 1));
                }

                return _value;
            }
        }
    }
}
