using System.Collections.Generic;

namespace ScreenSaver.Options
{
    public class UserOptionsProfileSet
    {
        public string SelectedProfileName { get; set; }
        public List<UserOptionsProfile> Profiles { get; set; }

        public UserOptionsProfileSet()
        {
            Profiles = new List<UserOptionsProfile>();
        }

        public UserOptionsProfile this[string profileName]
        {
            get
            {
                return Profiles.Find(p => p.Name.Equals(profileName));
            }
        }
    }
}
