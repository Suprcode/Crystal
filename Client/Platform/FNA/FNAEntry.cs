using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Client.Platform;
using Client.MirScenes;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirSounds;
using FontStashSharp;

namespace Client.Platform.FNA
{
    public class FNAEntry : Game
    {
        public static FNAEntry Instance { get; private set; }
        public GraphicsDeviceManager Graphics { get; }
        public FNARenderer Renderer { get; private set; }



        private KeyboardState _prevKeyboardState;
        private MouseState _prevMouseState;
        private long _cleanTime;

        public FNAEntry()
        {
            Instance = this;
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Settings.ScreenWidth,
                PreferredBackBufferHeight = Settings.ScreenHeight,
                IsFullScreen = Settings.FullScreen,
                SynchronizeWithVerticalRetrace = true
            };
            Graphics.PreparingDeviceSettings += (sender, e) =>
            {
                e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            };
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Renderer = new FNARenderer(GraphicsDevice);
            Renderer.Initialize(Settings.ScreenWidth, Settings.ScreenHeight, Settings.FullScreen);
            DXManager.Renderer = Renderer;

            base.Initialize();

            // Hook FNA's native TextInputEXT for text typing (handles full Chinese/IME inputs natively!)
            TextInputEXT.TextInput += OnTextInput;

            // Load baseline configurations
            _prevKeyboardState = Keyboard.GetState();
            _prevMouseState = Mouse.GetState();

            // Set running state
            CMain.Time = 0;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            
            // Initialize game managers
            DXManager.Renderer = Renderer;
            SoundManager.Create();
        }

        private void OnTextInput(char character)
        {
            if (MirScene.ActiveScene == null) return;

            var e = new MirKeyPressEventArgs(character);
            MirScene.ActiveScene.OnKeyPress(e);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update global frame timings
            CMain.Time = (long)gameTime.TotalGameTime.TotalMilliseconds;

            if (CMain.Time >= _cleanTime)
            {
                _cleanTime = CMain.Time + 1000;
                DXManager.Clean();
            }

            // Process Network packets
            Client.MirNetwork.Network.Process();

            // Handle Poll-based Inputs
            PollKeyboard();
            PollMouse();

            // Update scenes & animations
            if (MirScene.ActiveScene != null)
                MirScene.ActiveScene.Process();

            for (int i = 0; i < MirAnimatedControl.Animations.Count; i++)
                MirAnimatedControl.Animations[i].UpdateOffSet();

            for (int i = 0; i < MirAnimatedButton.Animations.Count; i++)
                MirAnimatedButton.Animations[i].UpdateOffSet();

            CMain.CreateHintLabel();

            if (Settings.DebugMode)
            {
                CMain.CreateDebugLabel();
            }
        }

        private void PollKeyboard()
        {
            if (MirScene.ActiveScene == null) return;

            var currState = Keyboard.GetState();
            var pressedKeys = currState.GetPressedKeys();

            // Determine modifiers
            var modifiers = MirKeys.None;
            if (currState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) || currState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                modifiers |= MirKeys.Shift;
            if (currState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) || currState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl))
                modifiers |= MirKeys.Control;
            if (currState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftAlt) || currState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt))
                modifiers |= MirKeys.Alt;

            CMain.Shift = (modifiers & MirKeys.Shift) == MirKeys.Shift;
            CMain.Ctrl = (modifiers & MirKeys.Control) == MirKeys.Control;
            CMain.Alt = (modifiers & MirKeys.Alt) == MirKeys.Alt;

            // Handle Screenshot globally on key release
            foreach (var key in _prevKeyboardState.GetPressedKeys())
            {
                if (!currState.IsKeyDown(key))
                {
                    var mirKey = (MirKeys)(int)key;
                    bool isScreenshot = false;
                    foreach (var keyCheck in CMain.InputKeys.Keylist)
                    {
                        if (keyCheck.function != KeybindOptions.Screenshot) continue;
                        if (keyCheck.Key != mirKey) continue;
                        if (keyCheck.RequireAlt != 2 && keyCheck.RequireAlt != (CMain.Alt ? 1 : 0)) continue;
                        if (keyCheck.RequireShift != 2 && keyCheck.RequireShift != (CMain.Shift ? 1 : 0)) continue;
                        if (keyCheck.RequireCtrl != 2 && keyCheck.RequireCtrl != (CMain.Ctrl ? 1 : 0)) continue;
                        if (keyCheck.RequireTilde != 2 && keyCheck.RequireTilde != (CMain.Tilde ? 1 : 0)) continue;

                        isScreenshot = true;
                        break;
                    }

                    if (isScreenshot)
                    {
                        CMain.CreateScreenShot();
                    }
                }
            }

            if (Client.MirControls.MirControl.ActiveControl is Client.MirControls.MirTextBox textBox)
            {
                // Key Down events
                foreach (var key in pressedKeys)
                {
                    if (!_prevKeyboardState.IsKeyDown(key))
                    {
                        var mirKey = (MirKeys)(int)key;
                        var e = new MirKeyEventArgs(mirKey, modifiers);
                        textBox.OnKeyDown(e);
                    }
                }

                // Key Up events
                foreach (var key in _prevKeyboardState.GetPressedKeys())
                {
                    if (!currState.IsKeyDown(key))
                    {
                        var mirKey = (MirKeys)(int)key;
                        var e = new MirKeyEventArgs(mirKey, modifiers);
                        textBox.OnKeyUp(e);
                    }
                }

                _prevKeyboardState = currState;
                return;
            }

            // Key Down events
            foreach (var key in pressedKeys)
            {
                if (!_prevKeyboardState.IsKeyDown(key))
                {
                    var mirKey = (MirKeys)(int)key;
                    var e = new MirKeyEventArgs(mirKey, modifiers);
                    MirScene.ActiveScene.OnKeyDown(e);
                }
            }

            // Key Up events
            foreach (var key in _prevKeyboardState.GetPressedKeys())
            {
                if (!currState.IsKeyDown(key))
                {
                    var mirKey = (MirKeys)(int)key;
                    var e = new MirKeyEventArgs(mirKey, modifiers);
                    MirScene.ActiveScene.OnKeyUp(e);
                }
            }

            _prevKeyboardState = currState;
        }

        private void PollMouse()
        {
            if (MirScene.ActiveScene == null) return;

            var currState = Mouse.GetState();
            CMain.MPoint = new System.Drawing.Point(currState.X, currState.Y);

            // Track Mouse Move
            if (currState.X != _prevMouseState.X || currState.Y != _prevMouseState.Y)
            {
                var buttons = GetButtons(currState);
                var e = new MirMouseEventArgs(buttons, 0, currState.X, currState.Y, 0);
                MirScene.ActiveScene.OnMouseMove(e);
            }

            // Track Mouse Scroll
            if (currState.ScrollWheelValue != _prevMouseState.ScrollWheelValue)
            {
                var delta = currState.ScrollWheelValue - _prevMouseState.ScrollWheelValue;
                var buttons = GetButtons(currState);
                var e = new MirMouseEventArgs(buttons, 0, currState.X, currState.Y, delta);
                MirScene.ActiveScene.OnMouseWheel(e);
            }

            // Mouse Down & Up checks
            CheckMouseButton(currState.LeftButton, _prevMouseState.LeftButton, MirMouseButtons.Left, currState);
            CheckMouseButton(currState.RightButton, _prevMouseState.RightButton, MirMouseButtons.Right, currState);
            CheckMouseButton(currState.MiddleButton, _prevMouseState.MiddleButton, MirMouseButtons.Middle, currState);

            _prevMouseState = currState;
        }

        private void CheckMouseButton(ButtonState curr, ButtonState prev, MirMouseButtons button, MouseState state)
        {
            if (curr == ButtonState.Pressed && prev == ButtonState.Released)
            {
                var e = new MirMouseEventArgs(button, 1, state.X, state.Y, 0);
                MirScene.ActiveScene.OnMouseDown(e);
            }
            else if (curr == ButtonState.Released && prev == ButtonState.Pressed)
            {
                var e = new MirMouseEventArgs(button, 1, state.X, state.Y, 0);
                MirScene.ActiveScene.OnMouseClick(e);
                MirScene.ActiveScene.OnMouseUp(e);
            }
        }

        private MirMouseButtons GetButtons(MouseState state)
        {
            var buttons = MirMouseButtons.None;
            if (state.LeftButton == ButtonState.Pressed) buttons |= MirMouseButtons.Left;
            if (state.RightButton == ButtonState.Pressed) buttons |= MirMouseButtons.Right;
            if (state.MiddleButton == ButtonState.Pressed) buttons |= MirMouseButtons.Middle;
            return buttons;
        }

        protected override void Draw(GameTime gameTime)
        {
            if (Renderer == null) return;

            CMain.UpdateFrameTime();

            // Clear screen
            Renderer.Clear(System.Drawing.Color.Black);

            // Execute scene rendering
            if (MirScene.ActiveScene != null)
            {
                Renderer.BeginDraw();
                
                MirScene.ActiveScene.Draw();

                Renderer.EndDraw();
            }

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Renderer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
