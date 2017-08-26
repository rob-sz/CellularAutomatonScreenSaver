using System.Drawing;

namespace ScreenSaver.Options
{
    public static class UserOptionsData
    {
        public static int MaximumNumberOfStates = 32;
        public static byte DefaultState = 0;
        public static Color BackgroundColor = Color.Black;
        public static string DefaultProfileName = "[Custom]";

        public static byte[] NumberOfStatesRange = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };
        public static byte[] CellSizeRange = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
        public static string[] IsIsotropicOptions = { "Yes", "No" };

        private static readonly byte[] NumberOfNeighboursRange1 = { 3, 5, 7, 9, 11, 13, 15, 19 };
        private static readonly byte[] NumberOfNeighboursRange2 = { 3, 5, 7, 9, 11 };
        private static readonly byte[] NumberOfNeighboursRange3 = { 3, 5, 7, 9 };
        private static readonly byte[] NumberOfNeighboursRange4 = { 3, 5, 7 };
        private static readonly byte[] NumberOfNeighboursRange5 = { 3, 5 };
        private static readonly byte[] NumberOfNeighboursRange6 = { 3 };

        public static byte[] NumberOfNeighboursRange(int numberOfStates)
        {
            if (numberOfStates <= 2)
                return NumberOfNeighboursRange1;

            if (numberOfStates <= 3)
                return NumberOfNeighboursRange2;

            if (numberOfStates <= 4)
                return NumberOfNeighboursRange3;

            if (numberOfStates <= 7)
                return NumberOfNeighboursRange4;

            if (numberOfStates <= 15)
                return NumberOfNeighboursRange5;

            return NumberOfNeighboursRange6;
        }

        public static UserOptions DefaultUserOptions
        {
            get
            {
                return new UserOptions
                {
                    NumberOfStates = 10,
                    NumberOfStatesMinimum = 2,
                    NumberOfStatesMaximum = 15,
                    IsNumberOfStatesRandom = true,

                    NumberOfNeighbours = 5,
                    NumberOfNeighboursMinimum = 3,
                    NumberOfNeighboursMaximum = 5,
                    IsNumberOfNeighboursRandom = true,

                    Lambda = 33,
                    LambdaMinimum = 30,
                    LambdaMaximum = 70,
                    IsLambdaRandom = true,

                    IsIsotropic = true,
                    IsIsotropicRandom = true,

                    CellSize = 2,
                    CellSizeMinimum = 1,
                    CellSizeMaximum = 5,
                    IsCellSizeRandom = true,

                    RgbRangeMinimum = 0,
                    RgbRangeMaximum = 255,
                    IsRgbRangeRandom = true,

                    SleepAfter = new System.TimeSpan(0, 30, 0),
                    IsSleepAfter = true,

                    ChangeWorldFrequency = 3,
                    IsChangeWorldFrequency = true,

                    ChangeRulesFrequency = 1,
                    IsChangeRulesFrequency = true,

                    IterationInterval = 10,

                    StateColors = new[]
                    {
                        BackgroundColor,
                        Color.White,
                        Color.DarkRed,
                        Color.Sienna,
                        Color.Gold,
                        Color.DarkKhaki,
                        Color.Orange,
                        Color.YellowGreen,
                        Color.Turquoise,
                        Color.DeepSkyBlue,
                        Color.BlueViolet,
                        Color.CadetBlue,
                        Color.DarkOliveGreen,
                        Color.LightYellow,
                        Color.Green,
                        Color.Red,
                        Color.LightGray,
                        Color.HotPink,
                        Color.MediumPurple,
                        Color.Navy,
                        Color.Yellow,
                        Color.Pink,
                        Color.Olive,
                        Color.Tomato,
                        Color.PaleTurquoise,
                        Color.SlateGray,
                        Color.RoyalBlue,
                        Color.Fuchsia,
                        Color.AliceBlue,
                        Color.Chocolate,
                        Color.Cyan,
                        Color.Crimson
                    }
                };
            }
        }
    }
}
