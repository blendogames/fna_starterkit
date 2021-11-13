using System;

using Microsoft.Xna.Framework;

namespace fna_starterkit
{
    public class Button
    {
        const int BUTTON_BUFFERMARGIN = 20;

        protected string displayText;
        protected event EventHandler<ButtonArgs> leftclickEvent;

        protected Vector2 position;
        public Vector2 GetPosition { get { return position; } }

        protected Rectangle backRect;
        public Rectangle GetRect { get { return backRect; } }

        protected bool hover;
        public bool GetHover { get { return hover; } }

        bool lastHover;

        protected ButtonArgs buttonArgs;
        public ButtonArgs GetButtonArgs { get { return buttonArgs; } }

        public Button(string _displayText, EventHandler<ButtonArgs> _leftclickEvent, ButtonArgs _arg = null)
        {
            displayText = _displayText;
            leftclickEvent = _leftclickEvent;

            Vector2 textBox = Globals.fontNTR.MeasureString(displayText);
            backRect = new Rectangle((int)position.X, (int)position.Y, (int)textBox.X + BUTTON_BUFFERMARGIN * 2, (int)textBox.Y);

            if (_arg != null)
            {
                buttonArgs = _arg;
            }
            else
            {
                buttonArgs = new ButtonArgs();
            }
        }

        public void SetDisplayText(string value)
        {
            this.displayText = value;
        }

        public virtual void SetPosition(Vector2 _position)
        {
            position = _position;
            backRect.X = (int)_position.X;
            backRect.Y = (int)_position.Y;
        }

        public virtual void Update(GameTime gameTime)
        {
            hover = backRect.Contains((int)InputManager.getMousePosition.X, (int)InputManager.getMousePosition.Y);

            //ignore if mouse cursor is outside window.
            Point pos = new Point((int)InputManager.getMousePosition.X, (int)InputManager.getMousePosition.Y);
            if (!Globals.screenManager.GraphicsDevice.Viewport.Bounds.Contains(pos))
                hover = false;


            if (hover && InputManager.GetMouseClick(0) && leftclickEvent != null)
            {
                leftclickEvent(this, buttonArgs);
                Globals.screenManager.GetSoundManager.Play("click");
            }

            if (lastHover != hover)
            {
                lastHover = hover;

                if (hover)
                {
                    Globals.screenManager.GetSoundManager.Play("rollover");
                }
            }
        }

        public virtual void Draw2D(GameTime gameTime)
        {
            Globals.screenManager.getSpriteBatch.Draw(Globals.white, backRect, hover ? Color.Goldenrod : Color.Chocolate);
            Globals.screenManager.getSpriteBatch.DrawString(Globals.fontNTR, displayText, position + new Vector2(BUTTON_BUFFERMARGIN,0),  Color.Black);
        }
    }

    public class ButtonArgs : EventArgs
    {
        public int var00, var01;
    }
}