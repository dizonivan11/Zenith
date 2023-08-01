using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Zenith.Components {
    /// <summary>
    /// This class was made to separate inputs from GUI and Game
    /// All the capture flags are used on this class to prevent the input going  through both GUI and Game
    /// ex1. MouseDown event works on the game as well as dragging the movable GUI window.
    /// ex2. Moving your character using WASD while also typing on a text input on the GUI.
    /// 'WantCaptureMouse', 'WantCaptureKeyboard', 'WantTextInput' effectively prevents any input from going through the game
    /// </summary>
    public class InputManager {
        readonly MainGame mainGame;
        MouseState previousMS, currentMS;
        KeyboardState previousKS, currentKS;
        public Point MousePosition { get { return currentMS.Position; } }

        public InputManager(MainGame mainGame) {
            this.mainGame = mainGame;
            currentMS = new MouseState();
            previousMS = currentMS;
            currentKS = new KeyboardState();
            previousKS = currentKS;
        }

        ButtonState GetStateByMouseButton(MouseState ms, MouseButton button) {
            switch (button) {
                case MouseButton.Left:
                    return ms.LeftButton;
                case MouseButton.Middle:
                    return ms.MiddleButton;
                case MouseButton.Right:
                    return ms.RightButton;
                default:
                    return new ButtonState();
            }
        }

        public void BeginCapture(MouseState ms, KeyboardState ks) {
            currentMS = ms;
            currentKS = ks;
        }

        public void EndCapture() {
            previousMS = currentMS;
            previousKS = currentKS;
        }

        public bool MouseClicked(MouseButton button) {
            return !mainGame.guiInput.WantCaptureMouse &&
                GetStateByMouseButton(previousMS, button) == ButtonState.Released &&
                GetStateByMouseButton(currentMS, button) == ButtonState.Pressed;
        }

        public bool MouseDown(MouseButton button) {
            return !mainGame.guiInput.WantCaptureMouse &&
                GetStateByMouseButton(previousMS, button) == ButtonState.Pressed &&
                GetStateByMouseButton(currentMS, button) == ButtonState.Pressed;
        }

        public bool MouseReleased(MouseButton button) {
            return !mainGame.guiInput.WantCaptureMouse &&
                GetStateByMouseButton(previousMS, button) == ButtonState.Pressed &&
                GetStateByMouseButton(currentMS, button) == ButtonState.Released;
        }

        public bool KeyPressed(Keys key) {
            return !mainGame.guiInput.WantTextInput && previousKS.IsKeyUp(key) && currentKS.IsKeyDown(key);
        }

        public bool KeyDown(Keys key) {
            return !mainGame.guiInput.WantTextInput && previousKS.IsKeyDown(key) && currentKS.IsKeyDown(key);
        }

        public bool KeyReleased(Keys key) {
            return !mainGame.guiInput.WantTextInput && previousKS.IsKeyDown(key) && currentKS.IsKeyUp(key);
        }
    }
    public enum MouseButton { Left, Middle, Right }
}
