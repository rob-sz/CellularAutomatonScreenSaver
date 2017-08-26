using System;
using System.Drawing;
using ScreenSaver.Options;

namespace ScreenSaver
{
    public class CaWorldOptionsRepository
    {
        private readonly Random _random;
        private readonly UserOptionsProfileRepository _userOptionsRepository;

        public CaWorldOptionsRepository()
        {
            _random = new Random();
            _userOptionsRepository = new UserOptionsProfileRepository();
        }

        public CaWorldOptions GetWorldOptions()
        {
            var userOptions = _userOptionsRepository.GetSelectedProfile().Options;

            if (userOptions.IsCellSizeRandom)
            {
                userOptions.CellSize = (byte)_random.Next(userOptions.CellSizeMinimum, userOptions.CellSizeMaximum);
            }

            if (userOptions.IsIsotropicRandom)
            {
                userOptions.IsIsotropic = _random.Next(0, 2) == 1;
            }

            if (userOptions.IsLambdaRandom)
            {
                userOptions.Lambda = _random.Next(userOptions.LambdaMinimum, userOptions.LambdaMaximum + 1);
            }

            if (userOptions.IsNumberOfNeighboursRandom)
            {
                userOptions.NumberOfNeighbours =
                    (byte)_random.Next(userOptions.NumberOfNeighboursMinimum, userOptions.NumberOfNeighboursMaximum + 1);
            }

            if (userOptions.IsNumberOfStatesRandom)
            {
                userOptions.NumberOfStates =
                    (byte)_random.Next(userOptions.NumberOfStatesMinimum, userOptions.NumberOfStatesMaximum + 1);
            }

            if (userOptions.IsRgbRangeRandom || userOptions.StateColors == null || userOptions.StateColors.Length == 0)
            {
                userOptions.StateColors = new Color[userOptions.NumberOfStates];
                userOptions.StateColors[0] = UserOptionsData.BackgroundColor;
                for (int i = 1; i < userOptions.StateColors.Length; i++)
                {
                    userOptions.StateColors[i] = Color.FromArgb(
                        _random.Next(userOptions.RgbRangeMinimum, userOptions.RgbRangeMaximum),
                        _random.Next(userOptions.RgbRangeMinimum, userOptions.RgbRangeMaximum),
                        _random.Next(userOptions.RgbRangeMinimum, userOptions.RgbRangeMaximum));
                }
            }
            else if (userOptions.StateColors.Length < userOptions.NumberOfStates)
            {
                var stateColors = new Color[userOptions.NumberOfStates];
                for (int i = 0; i < userOptions.StateColors.Length; i++)
                {
                    stateColors[i] = i < userOptions.StateColors.Length
                        ? userOptions.StateColors[i] : UserOptionsData.BackgroundColor;
                }
                userOptions.StateColors = stateColors;
            }

            //if (!userOptions.IsSeedIteration || userOptions.SeedIteration == null || userOptions.SeedIteration.Length == 0)
            //{
            //    userOptions.SeedIteration = new byte[cellCount];
            //    for (int i = 0; i < userOptions.SeedIteration.Length; i++)
            //    {
            //        userOptions.SeedIteration[i] = (byte)_random.Next(userOptions.NumberOfStates);
            //    }
            //}

            var worldOptions = new CaWorldOptions
            {
                CellSize = userOptions.CellSize,
                ChangeRulesFrequency = userOptions.ChangeRulesFrequency,
                ChangeWorldFrequency = userOptions.ChangeWorldFrequency,
                IsChangeRulesFrequency = userOptions.IsChangeRulesFrequency,
                IsChangeWorldFrequency = userOptions.IsChangeWorldFrequency,
                IsIsotropic = userOptions.IsIsotropic,
                IsSleepAfter = userOptions.IsSleepAfter,
                IterationInterval = userOptions.IterationInterval,
                Lambda = userOptions.Lambda,
                NumberOfNeighbours = userOptions.NumberOfNeighbours,
                NumberOfStates = userOptions.NumberOfStates,
                //SeedIteration = userOptions.SeedIteration,
                SleepAfter = userOptions.SleepAfter,
                StateColors = userOptions.StateColors
            };

            return worldOptions;
        }

        public bool ProfileExists(string profileName)
        {
            return _userOptionsRepository.ProfileExists(profileName);
        }

        public void SaveChangesAs(string profileName)
        {
            // note: technically this should be in unit of work

            _userOptionsRepository.Add(
                new UserOptionsProfile
                {
                    Name = profileName,
                    Options = _userOptionsRepository.GetSelectedProfile().Options
                });

            _userOptionsRepository.SaveChanges();
        }
    }
}
