using System.Linq;

namespace ScreenSaver.Options
{
    public class UserOptionsProfileRepository
    {
        private readonly UserOptionsContext _userOptionsContext;

        public UserOptionsProfileRepository()
        {
            _userOptionsContext = new UserOptionsContext();
        }

        public string[] GetAllProfileNames()
        {
            return _userOptionsContext.OptionsProfileSet.Profiles.Select(o => o.Name).ToArray();
        }

        public string GetSelectedProfileName()
        {
            return _userOptionsContext.OptionsProfileSet.SelectedProfileName;
        }

        public UserOptionsProfile GetSelectedProfile()
        {
            return GetProfile(_userOptionsContext.OptionsProfileSet.SelectedProfileName);
        }

        public UserOptionsProfile GetProfile(string profileName)
        {
            return _userOptionsContext.OptionsProfileSet[profileName];
        }

        public void Add(UserOptionsProfile profile)
        {
            _userOptionsContext.OptionsProfileSet.Profiles.Add(profile);

            if (_userOptionsContext.OptionsProfileSet.Profiles.Count == 1)
            {
                _userOptionsContext.OptionsProfileSet.SelectedProfileName = profile.Name;
            }
        }

        public bool ProfileExists(string profileName)
        {
            return _userOptionsContext.OptionsProfileSet[profileName] != null;
        }

        public void SaveChanges()
        {
            // note: technically this should be in unit of work

            _userOptionsContext.SaveChanges();
        }
    }
}
