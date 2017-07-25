using System;

namespace ScreenSaver
{
    public class BoolParameter
    {
        private bool _value;
        private Random _random;

        public BoolParameter()
        {
            _random = new Random();
        }

        public BoolParameter(bool value)
        {
            _value = value;
        }

        public bool Value
        {
            get
            {
                if (_random != null)
                    return _random.Next(0, 2) == 1;

                return _value;
            }
        }
    }
}
