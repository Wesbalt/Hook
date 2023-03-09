using static Hook.Logger;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MGKeyboard = Microsoft.Xna.Framework.Input.Keyboard;
using MGMouse = Microsoft.Xna.Framework.Input.Mouse;
using MGGamePad = Microsoft.Xna.Framework.Input.GamePad;
using System.Linq;

namespace Hook
{
    public static class Keyboard
    {
        private static KeyboardState previous;
        private static KeyboardState current = MGKeyboard.GetState();

        public static void Update()
        {
            previous = current;
            current  = MGKeyboard.GetState();
        }

        public static bool KeyDown(Keys key)
        {
            return current.IsKeyDown(key);
        }

        public static bool KeyJustDown(Keys key)
        {
            return current.IsKeyDown(key) && !previous.IsKeyDown(key);
        }

        public static bool KeyJustUp(Keys key)
        {
            return current.IsKeyUp(key) && !previous.IsKeyUp(key);
        }
    }

    public static class Mouse
    {
        private static MouseState previous;
        private static MouseState current = MGMouse.GetState();

        public static void Update()
        {
            previous = current;
            current  = MGMouse.GetState();
        }

        public static bool LeftDown()
        {
            return current.LeftButton == ButtonState.Pressed;
        }

        public static bool LeftClick()
        {
            return previous.LeftButton == ButtonState.Released &&
                    current.LeftButton == ButtonState.Pressed;
        }

        public static bool RightDown()
        {
            return current.RightButton == ButtonState.Pressed;
        }

        public static bool RightClick()
        {
            return previous.RightButton == ButtonState.Released &&
                    current.RightButton == ButtonState.Pressed;
        }

        public static bool MiddleDown()
        {
            return current.MiddleButton == ButtonState.Pressed;
        }

        public static bool MiddleClick()
        {
            return previous.MiddleButton == ButtonState.Released &&
                    current.MiddleButton == ButtonState.Pressed;
        }

        public static Point Position()
        {
            return current.Position;
        }

        public static Point MoveDelta()
        {
            return current.Position - previous.Position;
        }

        public static int ScrollDelta()
        {
            return current.ScrollWheelValue - previous.ScrollWheelValue;
        }
    }

    public static class GamePads
    {
        private static GamePad[] gamePads;

        static GamePads()
        {
            gamePads = new GamePad[MGGamePad.MaximumGamePadCount];
            for (int i = 0; i < MGGamePad.MaximumGamePadCount; i++)
            {
                gamePads[i] = new GamePad(i);
            }
        }

        public static void Update()
        {
            foreach (var gamePad in gamePads)  gamePad.Update();
        }

        public static GamePad GetJustConnected()
        {
            foreach (var gamePad in gamePads)
            {
                if (gamePad.JustConnected())  return gamePad;
            }
            return null;
        }

        public static GamePad[] GetAllConnected()
        {
            return new List<GamePad>(gamePads)
                .Where(gamePad => gamePad.IsConnected())
                .ToArray();
        }
    }

    public class GamePad
    {
        public readonly int   index;
        private GamePadState  previous, current;
        private Vector2       dPadVector;

        internal protected GamePad(int i)
        {
            index      = i;
            current    = MGGamePad.GetState(index);
            previous   = current;
            dPadVector = new Vector2(0,0);
        }

        public bool IsConnected()
        {
            return current.IsConnected;
        }

        public bool JustConnected()
        {
            return !previous.IsConnected && current.IsConnected;
        }

        public bool JustDisconnected()
        {
            return previous.IsConnected && !current.IsConnected;
        }

        public bool ButtonDown(Buttons btn)
        {
            return current.IsButtonDown(btn);
        }

        public bool ButtonJustDown(Buttons btn)
        {
            return current.IsButtonDown(btn) && !previous.IsButtonDown(btn);
        }

        public bool ButtonUp(Buttons btn)
        {
            return current.IsButtonUp(btn);
        }

        public bool ButtonJustUp(Buttons btn)
        {
            return current.IsButtonUp(btn) && !previous.IsButtonUp(btn);
        }

        public Vector2 LeftThumbStick()
        {
            return current.ThumbSticks.Left;
        }

        public Vector2 RightThumbStick()
        {
            return current.ThumbSticks.Right;
        }

        public float LeftTrigger()
        {
            return current.Triggers.Left;
        }

        public float RightTrigger()
        {
            return current.Triggers.Right;
        }

        public Vector2 DPad()
        {
            return dPadVector;
        }

        internal protected void Update()
        {
            previous = current;
            current  = MGGamePad.GetState(index);

            dPadVector.X = 0;
            if (ButtonDown(Buttons.DPadLeft))   dPadVector.X--;
            if (ButtonDown(Buttons.DPadRight))  dPadVector.X++;

            dPadVector.Y = 0;
            if (ButtonDown(Buttons.DPadUp))    dPadVector.Y++;
            if (ButtonDown(Buttons.DPadDown))  dPadVector.Y--;
        }
    }
}
