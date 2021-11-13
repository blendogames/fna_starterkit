using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace fna_starterkit
{
    public class SoundManager
    {
        Dictionary<string, SoundEntry> soundDictionary;

        public SoundManager()
        {
            string dirPath = Path.Combine(Globals.baseFolder, "sound");

            if (!Directory.Exists(dirPath))
            {
                Helpers.ErrorPopup("SoundManager failed to find directory:\n{0}", Path.Combine(Environment.CurrentDirectory, dirPath));
                return;
            }

            //Get all the .wav files in the sounds folder.
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            FileInfo[] fileArray = dirInfo.GetFiles("*.wav");

            //Generate a dictionary of all .wav sound files. This creates a library of sounds for the game. We bypass the XNB content pipeline, and just load .wav files directly.
            soundDictionary = new Dictionary<string, SoundEntry>();


            for (int i = 0; i < fileArray.Length; i++)
            {
                if (!File.Exists(fileArray[i].FullName))
                {
                    Helpers.ErrorPopup(string.Format("SoundManager can't find file:\n{0}", fileArray[i].FullName));
                    continue;
                }

                SoundEntry newEntry = new SoundEntry();
                if (!newEntry.Initialize(fileArray[i].FullName))
                    continue;

                string cueName = Path.GetFileNameWithoutExtension(fileArray[i].FullName);
                soundDictionary.Add(cueName, newEntry); //Add the entry to the sound library.
            }
        }

        

        public void Play(string cuename)
        {
            SoundEntry entry;
            if (soundDictionary.TryGetValue(cuename, out entry))
            {
                entry.Play();
            }
            else
            {
                Helpers.FatalPopup("Failed to find sound: {0}", cuename);
            }
        }

        public void SetLoop(string cuename, bool value)
        {
            SoundEntry entry;
            if (soundDictionary.TryGetValue(cuename, out entry))
            {
                entry.SetLoop(value);                
            }
        }

        public void SetPitch(string cuename, float value)
        {
            SoundEntry entry;
            if (soundDictionary.TryGetValue(cuename, out entry))
            {
                entry.SetPitch(value);
            }
        }

        public void SetVolume(string cuename, float value)
        {
            SoundEntry entry;
            if (soundDictionary.TryGetValue(cuename, out entry))
            {
                entry.SetVolume(value);
            }
        }

        public void StopAllSound()
        {
            foreach (KeyValuePair<string, SoundEntry> entry in soundDictionary)
            {
                entry.Value.Stop();
            }
        }

        public void StopSound(string cuename)
        {
            SoundEntry entry;
            if (soundDictionary.TryGetValue(cuename, out entry))
            {
                entry.Stop();
            }
        }

        
        public void Update(GameTime gameTime)
        {
        }
    }

    public class SoundEntry
    {
        SoundEffect effect;
        SoundEffectInstance instance;

        public SoundEntry()
        {
        }

        public bool Initialize(string filepath)
        {
            try
            {
                //Load .wav files directly. Bypass the content pipeline.
                System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.Open, FileAccess.Read);
                effect = SoundEffect.FromStream(fs);
                fs.Dispose();

                instance = effect.CreateInstance();
            }
            catch (Exception e)
            {
                Helpers.ErrorPopup(string.Format("Failed to load sound:\n{0}\n\n{1}", filepath, e.Message));
                return false;
            }

            return true;
        }

        public void Stop()
        {
            if (instance.State == SoundState.Stopped || instance.State == SoundState.Paused)
                return;

            instance.Stop();
        }

        public void Pause()
        {
            if (instance.State == SoundState.Paused || instance.State == SoundState.Stopped)
                return;

            instance.Pause();
        }

        public void SetVolume(float value)
        {
            instance.Volume = value;
        }

        public void SetPitch(float value)
        {
            instance.Pitch = value;
        }

        public void SetLoop(bool value)
        {
            if (instance.IsLooped == value)
            {
                return;
            }

            if (instance.State != SoundState.Stopped)
            {
                Console.WriteLine("SPEC VIOLATION, TELL FLIBITIJIBIBO:\n\n" + Environment.StackTrace);
                return;
            }

            instance.IsLooped = value;
        }

        public void Play()
        {
            if (instance.State == SoundState.Stopped)
            {
                instance.Play();
            }
            else if (instance.State == SoundState.Paused)
            {
                instance.Resume();
            }
            else
            {
                instance.Stop();
                instance.Play();
            }
        }
    }
}