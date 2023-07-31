using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.ImGui;
using System;
using Zenith.Components;
using Zenith.Scenes;

namespace Zenith {
    public class MainGame : Game {
        public readonly GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public SpriteFont regularFont;
        public ImGuiRenderer ui;
        public readonly FPSCounter fps;
        Scene scene;

        public MainGame() {
            graphics = new GraphicsDeviceManager(this) {
                PreferMultiSampling = true,
                GraphicsProfile = GraphicsProfile.HiDef,
                HardwareModeSwitch = false, // true = Fullscreen; false = Borderless Fullscreen
                IsFullScreen = true,
                SynchronizeWithVerticalRetrace = false
            };
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000d / 165);
            IsMouseVisible = true;
            fps = new FPSCounter();
            scene = new GameScene(this);
            Content.RootDirectory = "Content";
        }

        public void ChangeScene(Scene newScene) {
            ui = new ImGuiRenderer(this).Initialize().RebuildFontAtlas();
            newScene.LoadContent();
            scene = newScene;
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            regularFont = Content.Load<SpriteFont>("default");
            ui = new ImGuiRenderer(this).Initialize().RebuildFontAtlas();
            scene.LoadContent();
        }

        protected override void Update(GameTime gameTime) {
            scene.Update(gameTime);
            fps.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            
            if (Keyboard.GetState().IsKeyDown(Keys.F11)) {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                SamplerState.PointWrap,
                DepthStencilState.DepthRead,
                RasterizerState.CullCounterClockwise);
            scene.Draw();
            spriteBatch.End();
            ui.BeginLayout(gameTime);
            scene.DrawUI(gameTime);
            ui.EndLayout();
            base.Draw(gameTime);
        }

        public void DrawFPSCounter(float x, float y) {
            spriteBatch.DrawString(regularFont, string.Format("FPS: {0}", (int)fps.AverageFramesPerSecond), new Vector2(x, y), Color.White);
        }
    }
}
