using System.Collections.Generic;

namespace ScreenSaver.Options
{
    public class UserOptionsContext
    {
        private readonly UserOptionsFile _optionsFile;
        private UserOptionsProfileSet _optionsProfileSet;

        public UserOptionsContext()
        {
            _optionsFile = new UserOptionsFile();
        }

        public UserOptionsProfileSet OptionsProfileSet
        {
            get
            {
                if (_optionsProfileSet == null)
                {
                    _optionsProfileSet = _optionsFile.LoadOptionsProfileSet();
                }

                if (_optionsProfileSet == null)
                {
                    _optionsProfileSet = new UserOptionsProfileSet
                    {
                        SelectedProfileName = UserOptionsData.DefaultProfileName
                    };
                }

                if (!_optionsProfileSet.Profiles.Exists(p => p.Name.Equals(UserOptionsData.DefaultProfileName)))
                {
                    _optionsProfileSet.Profiles.Add(
                        new UserOptionsProfile
                        {
                            Name = UserOptionsData.DefaultProfileName,
                            Options = UserOptionsData.DefaultUserOptions
                        });
                }

                if (string.IsNullOrWhiteSpace(_optionsProfileSet.SelectedProfileName))
                {
                    _optionsProfileSet.SelectedProfileName = UserOptionsData.DefaultProfileName;
                }

                return _optionsProfileSet;
            }
        }

        public void SaveChanges()
        {
            _optionsFile.SaveOptionsProfileSet(_optionsProfileSet);
        }
    }
}
