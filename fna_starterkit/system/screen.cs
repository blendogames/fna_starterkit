using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace fna_starterkit
{
    public enum ScreenState
    {
        Active,
        Deactivated,
        TransitionOn,
        TransitionOff
    }

    //A screen. i.e. options menu, title screen, game screen, etc. All of the screens are handled through ScreenManager.
    public abstract class Screen
    {
        public SpriteBatch getSpriteBatch { get { return Globals.screenManager.getSpriteBatch; } }
        public ContentManager getContentManager { get { return Globals.screenManager.Content; } }
        public GraphicsDeviceManager getGraphicsDevice { get { return Globals.screenManager.getGraphicsDevice; } }

        ScreenState state = ScreenState.Active;
        public ScreenState getState { get { return state; } }

        protected int transitionOnTime;
        protected int transitionOffTime;
        int transitionTimer;
        float transition;
        public float getTransition { get { return transition; } }

        public Screen()
        {
            transitionOnTime = 300;
            transitionOffTime = 100;
            state = ScreenState.TransitionOn;
        }

        //This gets called when resolution is changed. Use this function to recalculate & re-arrange UI elements for new screen resolution, etc.
        public virtual void Reinitialize()
        {            
        }

        //Immediately remove this screen from the stack, no transition.
        public virtual void KillScreen()
        {
            state = ScreenState.Deactivated;
        }

        //Remove this screen from the stack with a nice transition.
        public virtual void ExitScreen()
        {
            transitionTimer = transitionOffTime;
            state = ScreenState.TransitionOff;
        }

        /// <summary>
        /// Gets called every frame.
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            //update the transition states.
            if (state == ScreenState.TransitionOn)
            {
                transitionTimer = (int)MathHelper.Min(transitionTimer + gameTime.ElapsedGameTime.Milliseconds, transitionOnTime);
                transition = (float)transitionTimer / transitionOnTime;

                if (transitionTimer >= transitionOnTime)
                {
                    state = ScreenState.Active;
                }
            }
            else if (state == ScreenState.TransitionOff)
            {
                transitionTimer = (int)MathHelper.Max(transitionTimer - gameTime.ElapsedGameTime.Milliseconds, 0);
                transition = (float)transitionTimer / transitionOffTime;

                if (transitionTimer <= 0)
                {
                    state = ScreenState.Deactivated;
                }
            }
        }

        //Gets called every frame, but only if this screen is on top of the stack.
        public virtual void UpdateInput(GameTime gameTime)
        {
        }

        //Draw 3D things.
        public virtual void Draw3D(GameTime gameTime)
        {
        }

        //Draw 2D things.
        public virtual void Draw2D(GameTime gameTime)
        {
        }
    }
}