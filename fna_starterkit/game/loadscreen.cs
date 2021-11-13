#define SKIPMAINMENU

using System;

using Microsoft.Xna.Framework;



namespace fna_starterkit
{
    //This is the first screen the player sees. It is basically here just so the player has something to look at while the game loads.
    public class LoadScreen : Screen
    {
        bool loadDone;
        int loadTimer; //Let it sit for a short time so the game has time to draw something on the screen.

        public LoadScreen()
        {
            this.transitionOffTime = 0;
            this.transitionOnTime = 0;
        }

        public override void Update(GameTime gameTime)
        {
            loadTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (!loadDone && loadTimer >= 100)
            {
                loadDone = true;
            
                //load the content data.
                Globals.Initialize(getContentManager);
            
                ExitScreen();
            
                //Load game world.
                Worldscreen worldscreen = new Worldscreen();
                Globals.screenManager.AddScreen(worldscreen);                
            }

            base.Update(gameTime);
        }

        public override void Draw2D(GameTime gameTime)
        {
            base.Draw2D(gameTime);
        }
    }
}