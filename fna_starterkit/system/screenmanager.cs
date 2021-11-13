using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;



namespace fna_starterkit
{
    //ScreenManager is the backbone of the game. It handles the stack of all the different screens.
    //Screens can be added and removed from the stack. The top screen in the stack is the one the player interacts with.
    //We also handle the game initialization here.
    public class ScreenManager : Game
    {
        List<Screen> screens = new List<Screen>();

        GraphicsDeviceManager graphics;
        public GraphicsDeviceManager getGraphicsDevice { get { return graphics; } }

        SpriteBatch spriteBatch;
        public SpriteBatch getSpriteBatch { get { return spriteBatch; } }

        BloomComponent bloom;
        public BloomComponent getBloom { get { return bloom; } }        

        SoundManager soundManager;
        public SoundManager GetSoundManager { get { return soundManager; } }

        SettingsManager settingsManager;
        public SettingsManager GetSettingsManager { get { return settingsManager; } }

        public ScreenManager()
        {
            //Initialize the game.
            graphics = new GraphicsDeviceManager(this);

            settingsManager = new SettingsManager();
            bool foundSettings = settingsManager.ReadSettingsFromFile();            

            //If there are no settings found
            if (!foundSettings)
            {
                //No settings found. Use default settings.
                DisplayMode defaultMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
                settingsManager.GetSettings.screenwidth = defaultMode.Width;
                settingsManager.GetSettings.screenheight = defaultMode.Height;
                settingsManager.GetSettings.fullscreen = true;
                settingsManager.GetSettings.soundvolume = 1.0f;

                //Write the settings file.
                settingsManager.WriteSettingsToFile();
            }

            
            //Settings are now loaded. Hook them into all the game systems.
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                this.Window.IsBorderlessEXT = settingsManager.GetSettings.fullscreen;
            }
            else
            {
                graphics.IsFullScreen = settingsManager.GetSettings.fullscreen;
            }

            graphics.PreferredBackBufferWidth = settingsManager.GetSettings.screenwidth;
            graphics.PreferredBackBufferHeight = settingsManager.GetSettings.screenheight;            
            SoundEffect.MasterVolume = settingsManager.GetSettings.soundvolume;

            graphics.SynchronizeWithVerticalRetrace = true; //vsync
            graphics.PreferMultiSampling = true;
            this.IsFixedTimeStep = false;
            this.Window.Title = Globals.WINDOWNAME;
            this.Window.AllowUserResizing = false;
            this.IsMouseVisible = true;

            soundManager = new SoundManager();

            Content.RootDirectory = Globals.baseFolder;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 60.0f);
        }

        //Call this when player changes their screen resolution while in-game.
        public void InitializeAllScreens()
        {
            for (int i = screens.Count - 1; i >= 0; i--)
            {
                if (screens[i] == null)
                    continue;

                screens[i].Reinitialize();
            }
        }

        //Add a new screen to the stack.
        public void AddScreen(Screen screen)
        {
            screens.Add(screen);
        }

        //Check whether the screen is at the top of the stack.
        public bool GetIsTopScreen(Screen screen)
        {
            return (screens[screens.Count - 1] == screen);
        }

        
        protected override void Initialize()
        {
            bloom = new BloomComponent(this);
            bloom.Settings = BloomSettings.PresetSettings[6];
            Components.Add(bloom); //Add bloom.
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.white = Content.Load<Texture2D>("textures\\white");

            AddScreen(new LoadScreen()); //Go to the loading screen.
        }

        protected override void UnloadContent()
        {
        }

        //This gets called every frame. This is the main update loop.
        protected override void Update(GameTime gameTime)
        {
            InputManager.Update(gameTime);

            for (int i = screens.Count - 1; i >= 0; i--)
            {
                if (screens[i] == null)  //Just in case....
                    continue;

                //remove any screens waiting to be removed.
                if (screens[i].getState == ScreenState.Deactivated)
                {
                    screens.RemoveAt(i);
                    continue;
                }

                //update screen.
                screens[i].Update(gameTime);
            }

            //update input only on screen on top of the stack.
            if (screens.Count > 0)
            {
                screens[screens.Count - 1].UpdateInput(gameTime);
            }

            soundManager.Update(gameTime);

            base.Update(gameTime);
        }

        //Draw 2D and 3D stuff.
        protected override void Draw(GameTime gameTime)
        {
            bloom.BeginDraw();
            GraphicsDevice.Clear(Globals.COLOR_BACKGROUND);                       

            for (int i = 0; i < screens.Count; i++)
            {
                if (screens[i] == null)
                    continue;

                //Draw 3D things.
                screens[i].Draw3D(gameTime);

                //Draw 2D things.
                spriteBatch.Begin();
                screens[i].Draw2D(gameTime);
                spriteBatch.End();
                graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            base.Draw(gameTime);
        }

        public void ResetBloom()
        {
            Components.Remove(bloom);
            bloom.Dispose();
            bloom = new BloomComponent(this);
            bloom.Settings = BloomSettings.PresetSettings[6];
            Components.Add(bloom);
        }

        //Delete all screens from the stack.
        public void ExitAllScreens()
        {
            screens.Clear();
        }

    }
}
