using System;
using System.Drawing;

namespace ScreenSaver
{
    public class CaWorldOptions
    {
        public byte NumberOfStates { get; set; }
        public byte NumberOfNeighbours { get; set; }
        public int Lambda { get; set; }
        public bool IsIsotropic { get; set; }
        public byte CellSize { get; set; }
        //public int CellCount { get; set; }
        public Color[] StateColors { get; set; }
        public TimeSpan SleepAfter { get; set; }
        public bool IsSleepAfter { get; set; }
        public int ChangeWorldFrequency { get; set; }
        public bool IsChangeWorldFrequency { get; set; }
        public int ChangeRulesFrequency { get; set; }
        public bool IsChangeRulesFrequency { get; set; }
        public int IterationInterval { get; set; }
        //public byte[] SeedIteration { get; set; }
    }
}
