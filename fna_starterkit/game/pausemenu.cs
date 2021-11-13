using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace fna_starterkit
{

    public class PauseMenu : Screen
    {
        const int MARGIN_LEFT = 50;

        Button[] buttons;

        public PauseMenu()
        {
            this.transitionOffTime = 200;
            this.transitionOnTime = 200;

            Reinitialize(); //Set up all the buttons.
        }

        public override void Reinitialize()
        {
            buttons = new Button[]
            {
            new Button("Resume", HitButton_Resume),
            new Button("Options", HitButton_Settings),            
            new Button("Exit to desktop", HitButton_Quit)
            };


            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].SetPosition(new Vector2(100, 200 + i * 90));
            }
        }

    
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void UpdateInput(GameTime gameTime)
        {
            if (!Globals.screenManager.IsActive)
                return;

            if (InputManager.GetKeyboardClick(Keys.Escape))
            {
                HitButton_Resume(null, null);
            }
            
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Update(gameTime);
            }            

            base.UpdateInput(gameTime);
        }
        
        private void HitButton_Resume(object sender, ButtonArgs data)
        {
            ExitScreen();
        }

        private void HitButton_Settings(object sender, ButtonArgs data)
        {
            Globals.screenManager.AddScreen(new OptionsPage());
        }      

        private void HitButton_Quit(object sender, ButtonArgs data)
        {
            Globals.screenManager.Exit();
        }

        public override void Draw2D(GameTime gameTime)
        {
            //Dark BG.
            Globals.screenManager.getSpriteBatch.Draw(Globals.white, new Rectangle(0, 0, Globals.screenManager.Window.ClientBounds.Width, Globals.screenManager.Window.ClientBounds.Height), (Color.PaleVioletRed * .8f) * this.getTransition);

            //header title.
            Globals.screenManager.getSpriteBatch.DrawString(Globals.fontNTR, "FNA Starter Kit", new Vector2(100,100), Color.White * this.getTransition);

            //Buttons.            
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Draw2D(gameTime);
            }
                        
        }
    }
}