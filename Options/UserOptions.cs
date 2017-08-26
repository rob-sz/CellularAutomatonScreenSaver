using System;
using System.Drawing;

namespace ScreenSaver.Options
{
    public class UserOptions
    {
        //public bool IsSeedIteration { get; set; }
        //public byte[] SeedIteration { get; set; }

        public byte NumberOfStates { get; set; }
        public byte NumberOfStatesMinimum { get; set; }
        public byte NumberOfStatesMaximum { get; set; }
        public bool IsNumberOfStatesRandom { get; set; }

        public byte NumberOfNeighbours { get; set; }
        public byte NumberOfNeighboursMinimum { get; set; }
        public byte NumberOfNeighboursMaximum { get; set; }
        public bool IsNumberOfNeighboursRandom { get; set; }

        public int Lambda { get; set; }
        public int LambdaMinimum { get; set; }
        public int LambdaMaximum { get; set; }
        public bool IsLambdaRandom { get; set; }

        public bool IsIsotropic { get; set; }
        public bool IsIsotropicRandom { get; set; }

        public byte CellSize { get; set; }
        public byte CellSizeMinimum { get; set; }
        public byte CellSizeMaximum { get; set; }
        public bool IsCellSizeRandom { get; set; }

        public int RgbRangeMinimum { get; set; }
        public int RgbRangeMaximum { get; set; }
        public bool IsRgbRangeRandom { get; set; }

        public TimeSpan SleepAfter { get; set; }
        public bool IsSleepAfter { get; set; }

        public int ChangeWorldFrequency { get; set; }
        public bool IsChangeWorldFrequency { get; set; }

        public int ChangeRulesFrequency { get; set; }
        public bool IsChangeRulesFrequency { get; set; }

        public int IterationInterval { get; set; }

        public Color[] StateColors { get; set; }
    }
}
