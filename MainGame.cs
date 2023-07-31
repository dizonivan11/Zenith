using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Zenith.Components;
using Zenith.Scenes;

namespace Zenith {
    public class MainGame : Game {
        public readonly GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public SpriteFont regularFont;
        public readonly FPSCounter fps;
        Scene scene;

        public MainGame() {
            graphics = new GraphicsDeviceManager(this) {
                PreferMultiSampling = true,
                GraphicsProfile = GraphicsProfile.HiDef,
                HardwareModeSwitch = false, // true = Fullscreen; false = Borderless Fullscreen
                IsFullScreen = true
            };
            graphics.PreferMultiSampling = true;
            graphics.SynchronizeWithVerticalRetrace = false;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000d / 165);
            IsMouseVisible = true;
            fps = new FPSCounter();
            scene = new GameScene(this);
            Content.RootDirectory = "Content";
        }

        public void ChangeScene(Scene newScene) {
            newScene.LoadContent();
            scene = newScene;
        }

        protected override void LoadContent() {
            regularFont = Content.Load<SpriteFont>("default");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            scene.LoadContent();
        }

        protected override void Update(GameTime gameTime) {
            scene.Update(gameTime);
            fps.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            scene.Draw();
            var fpsString = string.Format("FPS: {0}", (int)fps.AverageFramesPerSecond);
            spriteBatch.DrawString(regularFont, fpsString, new Vector2(1, 1), Color.Black);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
