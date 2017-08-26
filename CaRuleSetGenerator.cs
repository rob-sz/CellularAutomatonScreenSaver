using System;
using System.Linq;

namespace ScreenSaver
{
    public class CaRuleSetGenerator
    {
        private readonly Random _random;
        private byte _numberOfStates;
        private byte _numberOfneighbours;

        public CaRuleSetGenerator()
        {
            _random = new Random();
        }

        public CaRuleSet Generate(byte numberOfStates, byte numberOfNeighbours, bool isIsotropic, int lambda)
        {
            _numberOfStates = numberOfStates;
            _numberOfneighbours = numberOfNeighbours;

            int ruleCount = (int)Math.Pow(numberOfStates, numberOfNeighbours);
            var rules = new CaRule[ruleCount];

            for (int i = 0; i < ruleCount; i++)
            {
                rules[i] = new CaRule();
            }

            for (int i = 1; i < ruleCount; i++)
            {
                rules[i].State = (byte)_random.Next(1, numberOfStates);
                if (isIsotropic)
                {
                    rules[IsotropicIndex(i)].State = rules[i].State;
                }
            }

            int lambdaCount = isIsotropic
                ? ((ruleCount + (int) Math.Pow(numberOfStates, (numberOfNeighbours + 1) / 2)) / 2) - 1
                : ruleCount - 1;

            var lambdaPath = new int[lambdaCount];

            int lambdaIndex = 0;
            for (int i = 1; i < ruleCount; i++)
            {
                if (!rules[i].IsUsed)
                {
                    lambdaPath[lambdaIndex++] = i;
                    rules[i].IsUsed = true;
                    if (isIsotropic)
                    {
                        rules[IsotropicIndex(i)].IsUsed = true;
                    }
                }
            }

            for (int i = 0; i < lambdaCount; i++)
            {
                var randomLambdaIndex = _random.Next(lambdaCount);
                int currentLambdaValue = lambdaPath[i];
                lambdaPath[i] = lambdaPath[randomLambdaIndex];
                lambdaPath[randomLambdaIndex] = currentLambdaValue;
            }

            int rulesUsed = (int)Math.Ceiling(lambda / 100d * lambdaCount);

            if (rulesUsed < 0)
            {
                rulesUsed = 0;
            }
            else if (rulesUsed > lambdaPath.Length)
            {
                rulesUsed = lambdaPath.Length;
            }

            if (isIsotropic)
            {
                for (int i = 0; i < rulesUsed; i++)
                {
                    int r = lambdaPath[i];
                    rules[r].IsUsed = rules[IsotropicIndex(r)].IsUsed = true;
                }
                for (int i = rulesUsed; i < lambdaPath.Length; i++)
                {
                    int r = lambdaPath[i];
                    rules[r].IsUsed = rules[IsotropicIndex(r)].IsUsed = false;
                }
            }
            else
            {
                for (int i = 0; i < rulesUsed; i++)
                {
                    int r = lambdaPath[i];
                    rules[r].IsUsed = true;
                }
                for (int i = rulesUsed; i < lambdaPath.Length; i++)
                {
                    int r = lambdaPath[i];
                    rules[r].IsUsed = false;
                }
            }

            return new CaRuleSet(numberOfStates, numberOfNeighbours, rules);
        }

        private int IsotropicIndex(int ruleIndex)
        {
            int isotropicIndex = 0;

            for (int i = 0; i < _numberOfneighbours; i++)
            {
                isotropicIndex = isotropicIndex * _numberOfStates + (ruleIndex % _numberOfStates);
                ruleIndex /= _numberOfStates;
            }

            return isotropicIndex;
        }
    }
}
