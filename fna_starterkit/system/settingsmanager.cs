using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using Newtonsoft.Json;

namespace fna_starterkit
{
    //Add or remove new settings here. They'll be added to the json settings file.
    public class SettingsProfile
    {
        [JsonProperty("screenwidth")]
        public int screenwidth { get; set; }

        [JsonProperty("screenheight")]
        public int screenheight { get; set; }

        [JsonProperty("fullscreen")]
        public bool fullscreen { get; set; }

        [JsonProperty("soundvolume")]
        public float soundvolume { get; set; }

        [JsonProperty("invertmouse")]
        public bool invertmouse { get; set; }
    }

    public class SettingsManager
    {
        static readonly string SETTINGS_FILEPATH = GetSettingsPath();

        SettingsProfile settings;
        public SettingsProfile GetSettings { get { return settings; } }

        static string GetSettingsPath() //Get the settings file, dependent on the platform.
        {
            string os = SDL2.SDL.SDL_GetPlatform();
            string osDir = string.Empty;
            if (os.Equals("Linux"))
            {
                osDir = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
                if (string.IsNullOrEmpty(osDir))
                {
                    osDir = Environment.GetEnvironmentVariable("HOME");
                    if (string.IsNullOrEmpty(osDir))
                    {
                        return "base/settings.json"; // Oh well.
                    }
                    osDir = Path.Combine(osDir, ".local/share");
                }
                osDir = Path.Combine(osDir, Globals.SETTINGSFOLDERNAME);
            }
            else if (os.Equals("Mac OS X"))
            {
                osDir = Environment.GetEnvironmentVariable("HOME");
                if (string.IsNullOrEmpty(osDir))
                {
                    return "base/settings.json"; // Oh well.
                }
                osDir = Path.Combine(osDir, "Library/Application Support", Globals.SETTINGSFOLDERNAME);
            }
            else if (os.Equals("Windows"))
            {
                return Path.Combine(Globals.baseFolder, "settings.json");
            }
            else
            {
                throw new NotSupportedException("Unhandled SDL2 platform!");
            }
            if (!Directory.Exists(osDir))
            {
                Directory.CreateDirectory(osDir);
            }
            return Path.Combine(osDir, "settings.json");
        }

        public SettingsManager()
        {
            settings = new SettingsProfile(); //Empty settings.
        }

        public static void OpenConfigFile()
        {
            string os = SDL2.SDL.SDL_GetPlatform();
            if (os.Equals("Windows") || os.Equals("Mac OS X"))
            {
                System.Diagnostics.Process.Start(SETTINGS_FILEPATH);
            }
            else
            {
                System.Diagnostics.Process.Start(
                    "xdg-open",
                    SETTINGS_FILEPATH
                );
            }
        }

        //Load settings from settings file.
        public bool ReadSettingsFromFile()
        {
            if (!File.Exists(SETTINGS_FILEPATH)) //Ensure settings file exists.
                return false;

            string rawStrings = Helpers.GetFileContents(SETTINGS_FILEPATH);

            if (string.IsNullOrEmpty(rawStrings)) //Ensure settings file has stuff in it.
                return false;

            //Attempt to load the settings json file.
            try
            {
                SettingsProfile profile = JsonConvert.DeserializeObject<SettingsProfile>(rawStrings);
                settings = profile; //Load it into the current settings.
            }
            catch
            {
                return false;
            }

            //yay everything worked.
            return true;
        }

        //Commit settings to the settings file.
        public bool WriteSettingsToFile()
        {
            try
            {
                using (StreamWriter file = File.CreateText(SETTINGS_FILEPATH))
                {
                    string output = JsonConvert.SerializeObject(settings, Formatting.Indented);
                    file.Write(output);
                }
            }
            catch (Exception err)
            {
                Helpers.ErrorPopup("Failed to write settings file:\n{0}\n\n{1}", SETTINGS_FILEPATH, err.Message);
                return false;
            }

            return true;
        }


        //Directly opens the json config file in the player's text editor.
        public static void OpenSettingsInEditor()
        {
            string os = SDL2.SDL.SDL_GetPlatform();
            if (os.Equals("Windows") || os.Equals("Mac OS X"))
            {
                System.Diagnostics.Process.Start(SETTINGS_FILEPATH);
            }
            else
            {
                System.Diagnostics.Process.Start(
                    "xdg-open",
                    SETTINGS_FILEPATH
                );
            }
        }
    }
}