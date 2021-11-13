using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace fna_starterkit
{
    public class OptionsPage : Screen
    {
        const int MARGIN_TOP = 100;
        const int MARGIN_LEFT = 50;

        const int RESOLUTION_COOLDOWNTIME = 300;        

        Button[] buttons;
        Button[] resolutionButtons;

        Point desktopResolution;
        
        int resolutionCooldown;

        Button volumeButton;

        public OptionsPage()
        {
            this.transitionOffTime = 100;
            this.transitionOnTime = 300;
            Globals.screenManager.IsMouseVisible = true;

            volumeButton = new Button("Sound volume: ????", HitButton_Volume);

            buttons = new Button[]
            {
                new Button("Fullscreen toggle", HitButton_Fullscreen),
                volumeButton,
                new Button("Done", HitButton_Resume)
            };

            int buttonStartY = MARGIN_TOP;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].SetPosition(new Vector2(MARGIN_LEFT, buttonStartY));
                buttonStartY += 70;
            }

            UpdateVolumeButtonText();

            //Create resolution buttons.
            resolutionCooldown = RESOLUTION_COOLDOWNTIME;
            Reinitialize();
        }

        public override void Reinitialize()
        {
            if (resolutionCooldown < RESOLUTION_COOLDOWNTIME) //Add a cooldown to resolution changes, to prevent misclicks or doubleclicks from causing multiple resolution changes.
                return;

            resolutionCooldown = 0;

            //Create all the resolution buttons.
            List<Button> rezButtons = new List<Button>();

            Point currentRez = new Point(Globals.screenManager.GetSettingsManager.GetSettings.screenwidth, Globals.screenManager.GetSettingsManager.GetSettings.screenheight);
            DisplayMode defaultMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if (mode.Width < 1024)
                    continue; //skip lower resolutions.

                string displayText = string.Format("{0}x{1}", mode.Width, mode.Height);

                //if ((mode.Width == defaultMode.Width) && (mode.Height == defaultMode.Height))
                if ((mode.Width == Globals.screenManager.Window.ClientBounds.Width) && (mode.Height == Globals.screenManager.Window.ClientBounds.Height))
                {
                    displayText = string.Format("[ {0} ]", displayText);
                    desktopResolution = new Point(defaultMode.Width, defaultMode.Height);
                }

                ButtonArgs args = new ButtonArgs(); //Stuff the resolution info into the button.
                args.var00 = mode.Width;
                args.var01 = mode.Height;
                Button rezButton = new Button(displayText, HitButton_Resolution, args);

                rezButtons.Add(rezButton);
            }

            int buttonStartX = 420;
            int buttonStartY = MARGIN_TOP;
            for (int i = 0; i < rezButtons.Count; i++)
            {
                rezButtons[i].SetPosition(new Vector2(buttonStartX, buttonStartY));
                buttonStartY += 70;

                if (buttonStartY >= Globals.screenManager.GetSettingsManager.GetSettings.screenheight - 100) //Start a new column.
                {
                    buttonStartY = MARGIN_TOP;
                    buttonStartX += 200;
                }
            }
            resolutionButtons = rezButtons.ToArray();
        }

        //Clicked on a resolution button.
        private void HitButton_Resolution(object sender, ButtonArgs data)
        {
            int width = data.var00;
            int height = data.var01;

            getGraphicsDevice.PreferredBackBufferWidth = width;
            getGraphicsDevice.PreferredBackBufferHeight = height;
            getGraphicsDevice.ApplyChanges();
            Globals.screenManager.ResetBloom();

            Globals.screenManager.GetSettingsManager.GetSettings.screenwidth = width;
            Globals.screenManager.GetSettingsManager.GetSettings.screenheight = height;
            Globals.screenManager.GetSettingsManager.WriteSettingsToFile();

            Globals.screenManager.InitializeAllScreens();
        }

        //Clicked on fullscreen button.
        private void HitButton_Fullscreen(object sender, ButtonArgs data)
        {
            Globals.screenManager.GetSettingsManager.GetSettings.fullscreen = !Globals.screenManager.GetSettingsManager.GetSettings.fullscreen;

            if (Globals.screenManager.GetSettingsManager.GetSettings.fullscreen)
            {
                getGraphicsDevice.PreferredBackBufferWidth = desktopResolution.X;
                getGraphicsDevice.PreferredBackBufferHeight = desktopResolution.Y;
            }
            else
            {
                //Use a relatively safe resolution.
                getGraphicsDevice.PreferredBackBufferWidth = 1280;
                getGraphicsDevice.PreferredBackBufferHeight = 720;
            }

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                Globals.screenManager.Window.IsBorderlessEXT = Globals.screenManager.GetSettingsManager.GetSettings.fullscreen;
            }
            else
            {
                getGraphicsDevice.IsFullScreen = Globals.screenManager.GetSettingsManager.GetSettings.fullscreen;
            }

            getGraphicsDevice.ApplyChanges();
            Globals.screenManager.ResetBloom();
            Globals.screenManager.GetSettingsManager.WriteSettingsToFile();
            Globals.screenManager.InitializeAllScreens();
        }

        //Clicked on volume button.
        private void HitButton_Volume(object sender, ButtonArgs data)
        {
            float newValue = 0;

            if (Globals.screenManager.GetSettingsManager.GetSettings.soundvolume <= 0)
                newValue = .3f;
            else if (Globals.screenManager.GetSettingsManager.GetSettings.soundvolume <= .3f)
                newValue = .6f;
            else if (Globals.screenManager.GetSettingsManager.GetSettings.soundvolume <= .6f)
                newValue = 1.0f;
            else
                newValue = 0;

            Globals.screenManager.GetSettingsManager.GetSettings.soundvolume = newValue;
            Globals.screenManager.GetSettingsManager.WriteSettingsToFile();
            UpdateVolumeButtonText();

            SoundEffect.MasterVolume = newValue; //This globally changes volume of all sound effects.
        }

        //Update text on volume button.
        private void UpdateVolumeButtonText()
        {
            float value = Globals.screenManager.GetSettingsManager.GetSettings.soundvolume;
            int displayvalue = (int)(value * 100.0f);
            volumeButton.SetDisplayText(string.Format("Sound volume: {0}%", displayvalue));
        }

        //Clicked on resume button.
        private void HitButton_Resume(object sender, ButtonArgs data)
        {
            //Hit done or esc.
            ExitScreen();
        }

        public override void Update(GameTime gameTime)
        {
            if (resolutionCooldown < RESOLUTION_COOLDOWNTIME)
            {
                resolutionCooldown += gameTime.ElapsedGameTime.Milliseconds;
            }

            base.Update(gameTime);
        }

        public override void UpdateInput(GameTime gameTime)
        {
            if (!Globals.screenManager.IsActive)
                return;

            if (InputManager.GetKeyboardClick(Keys.Escape))
            {
                HitButton_Resume(null,null);
            }

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Update(gameTime);
            }

            for (int i = 0; i < resolutionButtons.Length; i++)
            {
                resolutionButtons[i].Update(gameTime);
            }

            base.UpdateInput(gameTime);            
        }

        


        public override void Draw2D(GameTime gameTime)
        {
            Globals.screenManager.getSpriteBatch.Draw(Globals.white, new Rectangle(0, 0, Globals.screenManager.Window.ClientBounds.Width, Globals.screenManager.Window.ClientBounds.Height), (Color.PaleVioletRed) * this.getTransition);

            string headerText = "Options";
            Vector2 headerSize = Globals.fontNTR.MeasureString(headerText);
            Globals.screenManager.getSpriteBatch.DrawString(Globals.fontNTR, headerText, new Vector2(MARGIN_LEFT, MARGIN_TOP - 50), Color.White * this.getTransition);

            if (getTransition >= 1)
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].Draw2D(gameTime);
                }

                for (int i = 0; i < resolutionButtons.Length; i++)
                {
                    resolutionButtons[i].Draw2D(gameTime);
                }
            }

        }
    }
}