using System;
using System.Collections.Generic;
//using System.Runtime.InteropServices; //For caps lock.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace fna_starterkit
{
    //Handles all input.
    public static class InputManager
    {
        private static KeyboardState keyboardState;
        private static KeyboardState prevKeyboardState;

        private static MouseState mouseState;
        private static MouseState prevMouseState;

        private static GamePadState gamepadState;
        private static GamePadState prevGamepadState;

        public static Vector2 getPrevMousePosition { get { return new Vector2(prevMouseState.X, prevMouseState.Y); } }
        public static Vector2 getMouseDelta { get { return new Vector2(mouseState.X - prevMouseState.X, mouseState.Y - prevMouseState.Y); } }
        public static Vector2 getMousePosition { get { return new Vector2(mouseState.X, mouseState.Y); } }

        public static void Update(GameTime gameTime)
        {
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            prevGamepadState = gamepadState;
            gamepadState = GamePad.GetState(PlayerIndex.One);

            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
        }

        public static bool GetKeyboardHeld(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public static bool GetKeyboardClick(Keys key)
        {
            return (keyboardState.IsKeyDown(key) && !prevKeyboardState.IsKeyDown(key));
        }

        public static bool GetMouseHeld(int buttonIndex)
        {
            if (buttonIndex == 0)
                return (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Pressed);
            else if (buttonIndex == 1)
                return (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Pressed);
            else
                return (mouseState.MiddleButton == ButtonState.Pressed && prevMouseState.MiddleButton == ButtonState.Pressed);
        }

        public static bool GetMouseClick(int buttonIndex)
        {
            if (buttonIndex == 0)
                return (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released);
            else if (buttonIndex == 1)
                return (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released);
            else
                return (mouseState.MiddleButton == ButtonState.Pressed && prevMouseState.MiddleButton == ButtonState.Released);
        }

        public static bool GetMouseJustReleased(int buttonIndex)
        {
            if (buttonIndex == 0)
                return (mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed);
            else
                return (mouseState.RightButton == ButtonState.Released && prevMouseState.RightButton == ButtonState.Pressed);
        }

        public static float GetGamepadTriggers(int index)
        {
            if (index == 0)
                return gamepadState.Triggers.Left;
            else
                return gamepadState.Triggers.Right;
        }

        public static Vector2 GetGamepadStick(int index)
        {
            if (index == 0)
                return gamepadState.ThumbSticks.Left;
            else
                return gamepadState.ThumbSticks.Right;
        }

        public static bool GetGamepadStickClick(int index)
        {
            if (index == 0)
                return gamepadState.Buttons.LeftStick == ButtonState.Pressed && prevGamepadState.Buttons.LeftStick == ButtonState.Released;
            else
                return gamepadState.Buttons.RightStick == ButtonState.Pressed && prevGamepadState.Buttons.RightStick == ButtonState.Released;
        }

        public static bool GetScrollwheelChanged()
        {
            return (mouseState.ScrollWheelValue != prevMouseState.ScrollWheelValue);
        }

        public static int GetScrollwheelDelta()
        {
            return mouseState.ScrollWheelValue - prevMouseState.ScrollWheelValue;
        }

        //For textbox input entry.
        public static string ConvertKeyToChar(Keys key, bool shift, bool allowCarriageReturn)
        {
            //bool capsLock = (((ushort)GetKeyState(0x14)) & 0xffff) != 0;
            bool capsLock = false;

            switch (key)
            {
                case Keys.Space: return " ";

                // Escape Sequences 
                case Keys.Enter:
                    {
                        if (allowCarriageReturn)
                            return "\n"; // Create a new line 
                        else
                            return "";
                    }

                // D-Numerics (strip above the alphabet) 
                case Keys.D0: return shift ? ")" : "0";
                case Keys.D1: return shift ? "!" : "1";
                case Keys.D2: return shift ? "@" : "2";
                case Keys.D3: return shift ? "#" : "3";
                case Keys.D4: return shift ? "$" : "4";
                case Keys.D5: return shift ? "%" : "5";
                case Keys.D6: return shift ? "^" : "6";
                case Keys.D7: return shift ? "&" : "7";
                case Keys.D8: return shift ? "*" : "8";
                case Keys.D9: return shift ? "(" : "9";

                // Numpad 
                case Keys.NumPad0: return "0";
                case Keys.NumPad1: return "1";
                case Keys.NumPad2: return "2";
                case Keys.NumPad3: return "3";
                case Keys.NumPad4: return "4";
                case Keys.NumPad5: return "5";
                case Keys.NumPad6: return "6";
                case Keys.NumPad7: return "7";
                case Keys.NumPad8: return "8";
                case Keys.NumPad9: return "9";
                case Keys.Add: return "+";
                case Keys.Subtract: return "-";
                case Keys.Multiply: return "*";
                case Keys.Divide: return "/";
                case Keys.Decimal: return ".";

                // Alphabet 
                case Keys.A: return (!capsLock && shift) || (capsLock && !shift) ? "A" : "a";
                case Keys.B: return (!capsLock && shift) || (capsLock && !shift) ? "B" : "b";
                case Keys.C: return (!capsLock && shift) || (capsLock && !shift) ? "C" : "c";
                case Keys.D: return (!capsLock && shift) || (capsLock && !shift) ? "D" : "d";
                case Keys.E: return (!capsLock && shift) || (capsLock && !shift) ? "E" : "e";
                case Keys.F: return (!capsLock && shift) || (capsLock && !shift) ? "F" : "f";
                case Keys.G: return (!capsLock && shift) || (capsLock && !shift) ? "G" : "g";
                case Keys.H: return (!capsLock && shift) || (capsLock && !shift) ? "H" : "h";
                case Keys.I: return (!capsLock && shift) || (capsLock && !shift) ? "I" : "i";
                case Keys.J: return (!capsLock && shift) || (capsLock && !shift) ? "J" : "j";
                case Keys.K: return (!capsLock && shift) || (capsLock && !shift) ? "K" : "k";
                case Keys.L: return (!capsLock && shift) || (capsLock && !shift) ? "L" : "l";
                case Keys.M: return (!capsLock && shift) || (capsLock && !shift) ? "M" : "m";
                case Keys.N: return (!capsLock && shift) || (capsLock && !shift) ? "N" : "n";
                case Keys.O: return (!capsLock && shift) || (capsLock && !shift) ? "O" : "o";
                case Keys.P: return (!capsLock && shift) || (capsLock && !shift) ? "P" : "p";
                case Keys.Q: return (!capsLock && shift) || (capsLock && !shift) ? "Q" : "q";
                case Keys.R: return (!capsLock && shift) || (capsLock && !shift) ? "R" : "r";
                case Keys.S: return (!capsLock && shift) || (capsLock && !shift) ? "S" : "s";
                case Keys.T: return (!capsLock && shift) || (capsLock && !shift) ? "T" : "t";
                case Keys.U: return (!capsLock && shift) || (capsLock && !shift) ? "U" : "u";
                case Keys.V: return (!capsLock && shift) || (capsLock && !shift) ? "V" : "v";
                case Keys.W: return (!capsLock && shift) || (capsLock && !shift) ? "W" : "w";
                case Keys.X: return (!capsLock && shift) || (capsLock && !shift) ? "X" : "x";
                case Keys.Y: return (!capsLock && shift) || (capsLock && !shift) ? "Y" : "y";
                case Keys.Z: return (!capsLock && shift) || (capsLock && !shift) ? "Z" : "z";

                // Oem 
                case Keys.OemOpenBrackets: return shift ? "{" : "[";
                case Keys.OemCloseBrackets: return shift ? "}" : "]";
                case Keys.OemComma: return shift ? "<" : ",";
                case Keys.OemPeriod: return shift ? ">" : ".";
                case Keys.OemMinus: return shift ? "_" : "-";
                case Keys.OemPlus: return shift ? "+" : "=";
                case Keys.OemQuestion: return shift ? "?" : "/";
                case Keys.OemSemicolon: return shift ? ":" : ";";
                case Keys.OemQuotes: return shift ? "\"" : "'";
                case Keys.OemPipe: return shift ? "|" : "\\";
                case Keys.OemTilde: return shift ? "~" : "`";
            }

            return string.Empty;
        }

        //For textbox input entry.
        public static List<Keys> getPressedKeys
        {
            get
            {
                Keys[] keyArray = keyboardState.GetPressedKeys();

                if (keyArray.Length <= 0)
                    return null;

                List<Keys> pressedKeys = new List<Keys>();
                for (int i = 0; i < keyArray.Length; i++)
                {
                    if (prevKeyboardState.IsKeyUp(keyArray[i]) == true)
                    {
                        pressedKeys.Add(keyArray[i]);
                    }
                }

                if (pressedKeys.Count > 0)
                {
                    return pressedKeys;
                }
                else
                {
                    return null;
                }
            }
        }



    }
}