using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra;
using Myra.Graphics2D.UI;
using System;
using Zenith.Components;
using Zenith.Scenes;

namespace Zenith {
    public class MainGame : Game {
        public readonly GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public SpriteFont regularFont;
        public Desktop desktop;
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
            MyraEnvironment.Game = this;
            MyraEnvironment.SmoothText = true;
            fps = new FPSCounter();
            scene = new GameScene(this);
            Content.RootDirectory = "Content";
        }

        public void ChangeScene(Scene newScene) {
            desktop = new Desktop();
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
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            scene.Draw();
            spriteBatch.End();
            desktop?.Render();
            base.Draw(gameTime);
        }

        public void DrawFPSCounter(float x, float y) {
            spriteBatch.DrawString(regularFont, string.Format("FPS: {0}", (int)fps.AverageFramesPerSecond), new Vector2(x, y), Color.White);
        }
    }
}
