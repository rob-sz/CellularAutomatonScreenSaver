using System;

namespace ScreenSaver
{
    public class CaWorld
    {
        private readonly byte[] _currentIteration;
        private readonly byte[] _nextIteration;

        private readonly int _cellCount;
        private readonly byte _numberOfStates;
        private readonly byte _numberOfNeighbours;
        private readonly bool _isIsotropic;
        private readonly int _lambda;

        private readonly Random _random;
        private readonly CaRuleSetGenerator _ruleSetGenerator;
        private CaRuleSet _ruleSet;

        public bool ChangeWorld { get; set; }
        public bool ChangeRules { get; set; }

        public CaWorld(int cellCount, byte numberOfStates, byte numberOfNeighbours, bool isIsotropic, int lambda)
        {
            _cellCount = cellCount;
            _currentIteration = new byte[_cellCount];
            _nextIteration = new byte[_cellCount];

            _numberOfStates = numberOfStates;
            _numberOfNeighbours = numberOfNeighbours;
            _isIsotropic = isIsotropic;
            _lambda = lambda;

            _random = new Random();
            _ruleSetGenerator = new CaRuleSetGenerator();

            ChangeWorld = ChangeRules = true;
        }

        public byte[] NextIteration()
        {
            if (ChangeWorld || ChangeRules)
            {
                _ruleSet = _ruleSetGenerator.Generate(_numberOfStates, _numberOfNeighbours, _isIsotropic, _lambda);
            }

            if (ChangeWorld)
            {
                for (int i = 0; i < _cellCount; i++)
                {
                    _currentIteration[i] = (byte) _random.Next(_numberOfStates);
                }
            }
            else
            {
                for (int i = 0; i < _cellCount; i++)
                {
                    _nextIteration[i] = _ruleSet.GetRuleState(i, _currentIteration);
                }

                Array.Copy(_nextIteration, _currentIteration, _cellCount);
            }

            ChangeWorld = ChangeRules = false;

            return _currentIteration;
        }
    }
}
