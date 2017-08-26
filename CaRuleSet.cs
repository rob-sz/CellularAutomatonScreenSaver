using ScreenSaver.Options;

namespace ScreenSaver
{
    public class CaRuleSet
    {
        private readonly byte _numberOfStates;
        private readonly byte _neighbourhoodSize;
        private readonly CaRule[] _rules;

        public CaRuleSet(byte numberOfStates, byte neighbourhoodSize, CaRule[] rules)
        {
            _numberOfStates = numberOfStates;
            _neighbourhoodSize = neighbourhoodSize;
            _rules = rules;
        }

        public byte GetRuleState(int cellIndex, byte[] cellStates)
        {
            int ruleIndex = 0;
            int cellCount = cellStates.Length;
            int offset = cellIndex - (_neighbourhoodSize / 2);

            for (int i = 0; i < _neighbourhoodSize; i++)
            {
                int neighbourCellIndex = i + offset;

                if (neighbourCellIndex < 0)
                {
                    neighbourCellIndex += cellCount;
                }
                else if (neighbourCellIndex >= cellCount)
                {
                    neighbourCellIndex -= cellCount;
                }

                ruleIndex = ruleIndex * _numberOfStates + cellStates[neighbourCellIndex];
            }

            var rule = _rules[ruleIndex];
            return rule.IsUsed ? rule.State : UserOptionsData.DefaultState;
        }
    }
}
