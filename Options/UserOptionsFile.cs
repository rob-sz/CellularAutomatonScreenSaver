using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace ScreenSaver.Options
{
    public class UserOptionsFile
    {
        public UserOptionsProfileSet LoadOptionsProfileSet()
        {
            string optionsFileName = OptionsFileName;
            if (!File.Exists(optionsFileName))
            {
                return null;
            }

            using (var streamReader = new StreamReader(optionsFileName))
            using (var textReader = new JsonTextReader(streamReader))
            {
                return new JsonSerializer().Deserialize<UserOptionsProfileSet>(textReader);
            }
        }

        public void SaveOptionsProfileSet(UserOptionsProfileSet optionsProfileSet)
        {
            using (var streamWriter = new StreamWriter(OptionsFileName, false))
            using (var textWriter = new JsonTextWriter(streamWriter))
            {
                new JsonSerializer { Formatting = Formatting.Indented }.Serialize(textWriter, optionsProfileSet);
            }
        }

        private string OptionsFileName
        {
            get
            {
                var applicationName = Assembly.GetEntryAssembly().GetName().Name;
                var applicationDataFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    applicationName);

                Directory.CreateDirectory(applicationDataFolder);

                return string.Format("{0}Options.json",
                    Path.Combine(applicationDataFolder, applicationName));
            }
        }
    }
}
