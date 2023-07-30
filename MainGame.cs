using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Zenith.Components;

namespace Zenith {
    public class MainGame : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TileMap tileMap;
        Player player;
        Vector2 CameraPosition;

        public MainGame() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tileMap = new TileMap(Importer.GetTexture2DFromFile(GraphicsDevice, "assets/tilesets/highlands_terrain.png"), "assets/maps/highlands_0_0.txt", 32, 32);
            player = new Player(Importer.GetTexture2DFromFile(GraphicsDevice, "assets/sprites/female_wizard.png"), new Vector2(0, 0), 64, 64, 5, 0.25f);
        }

        protected override void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                CameraPosition += new Vector2(0, -10);

            if (Keyboard.GetState().IsKeyDown(Keys.S))
                CameraPosition += new Vector2(0, 10);

            if (Keyboard.GetState().IsKeyDown(Keys.A))
                CameraPosition += new Vector2(-10, 0);

            if (Keyboard.GetState().IsKeyDown(Keys.D))
                CameraPosition += new Vector2(10, 0);

            player.Update(gameTime, tileMap);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            tileMap.Draw(spriteBatch, CameraPosition, GraphicsDevice.Viewport);
            player.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
