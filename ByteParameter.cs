using System;

namespace ScreenSaver
{
    public class ByteParameter
    {
        private byte _value;
        private byte _rangeFrom;
        private byte _rangeTo;
        private Random _random;

        public ByteParameter(byte value)
        {
            _value = value;
        }

        public ByteParameter(byte rangeFrom, byte rangeTo)
        {
            _rangeFrom = rangeFrom;
            _rangeTo = rangeTo;
            _random = new Random();
        }

        public byte Value
        {
            get
            {
                if (_random != null)
                    return (byte)_random.Next(_rangeFrom, _rangeTo + 1);

                return _value;
            }
        }
    }
}
