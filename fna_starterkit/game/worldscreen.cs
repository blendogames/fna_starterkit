using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace fna_starterkit
{
    public class Worldscreen : Screen
    {
        Camera3D camera;
        public Camera3D GetCamera { get { return camera; } }

        Entity chair;

        public Worldscreen()
        {            
            camera = new Camera3D();
            camera.cameraPosition = Globals.CAMERAPOS;

            chair = new Entity("models/chair/chair.xnb");
            chair.SetPosition(new Vector3(0, 0, 50));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void UpdateInput(GameTime gameTime)
        {
            if (!Globals.screenManager.IsActive)
                return; //If game window is not focused, then exit here.
            
            camera.Update(gameTime); //Update the camera.


            if (InputManager.GetKeyboardHeld(Keys.W) || InputManager.GetKeyboardHeld(Keys.Up))
            {
                chair.SetPosition(new Vector3(chair.GetPosition.X, chair.GetPosition.Y + (gameTime.ElapsedGameTime.Milliseconds * .1f), chair.GetPosition.Z));
            }
            else if (InputManager.GetKeyboardHeld(Keys.S) || InputManager.GetKeyboardHeld(Keys.Down))
            {
                chair.SetPosition(new Vector3(chair.GetPosition.X, chair.GetPosition.Y + (gameTime.ElapsedGameTime.Milliseconds * -.1f), chair.GetPosition.Z));
            }

            if (InputManager.GetKeyboardHeld(Keys.A) || InputManager.GetKeyboardHeld(Keys.Left))
            {
                chair.SetPosition(new Vector3(chair.GetPosition.X + (gameTime.ElapsedGameTime.Milliseconds * -.1f), chair.GetPosition.Y, chair.GetPosition.Z));
            }
            else if (InputManager.GetKeyboardHeld(Keys.D) || InputManager.GetKeyboardHeld(Keys.Right))
            {
                chair.SetPosition(new Vector3(chair.GetPosition.X + (gameTime.ElapsedGameTime.Milliseconds * .1f), chair.GetPosition.Y, chair.GetPosition.Z));
            }


            if (InputManager.GetKeyboardClick(Keys.Escape)) //Handle key input.
            {
                Globals.screenManager.AddScreen(new PauseMenu());
            }
        }      
        

        public override void Draw2D(GameTime gameTime)
        {
            //Draw a message in center of screen.
            string message = "FNA Starter Kit\n\nWASD = move chair\nESC = menu";
            Vector2 messageSize = Globals.fontNTR.MeasureString(message);
            Globals.screenManager.getSpriteBatch.DrawString(Globals.fontNTR, message, new Vector2(Globals.screenManager.Window.ClientBounds.Width / 2 - messageSize.X / 2, 20 ), Color.White);
            
            //Draw image of an orange.
            Globals.screenManager.getSpriteBatch.Draw(Globals.orange, new Rectangle(20, 20, 200, 200), Color.White);
        }

        public override void Draw3D(GameTime gameTime)
        {
            //Render the chair model.
            chair.Draw3D(gameTime, this.camera);           
        }
    }
}