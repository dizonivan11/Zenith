using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Zenith.Components;

namespace Zenith.Scenes {
    public class GameScene : Scene {
        TileMap tileMap;
        Player player;
        Vector2 CameraPosition;
        Vector2 CameraPositionDestination;

        public GameScene(MainGame mainGame) : base(mainGame) {

        }

        public override void LoadContent() {
            tileMap = new TileMap(
                Importer.GetTexture2DFromFile(mainGame.GraphicsDevice, "assets/tilesets/highlands_terrain.png"),
                "assets/maps/highlands_0_0.txt", 32, 32);

            player = new Player(
                Importer.GetTexture2DFromFile(mainGame.GraphicsDevice, "assets/sprites/female_wizard.png"),
                new Vector2(0, 0), 0.2f, 64, 64, 10, 9, 0.1f);
        }

        public override void Update(GameTime gameTime) {
            Vector2 dir = Vector2.Zero;
            if (Keyboard.GetState().IsKeyDown(Keys.W)) {
                dir.Y = -1;
                player.currentFrameIndex = 8;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S)) {
                dir.Y = 1;
                player.currentFrameIndex = 10;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A)) {
                dir.X = -1;
                player.currentFrameIndex = 9;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D)) {
                dir.X = 1;
                player.currentFrameIndex = 11;
            }
            if (dir != Vector2.Zero) {
                dir.Normalize();
                player.position += dir * player.speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            CameraPositionDestination.X = player.position.X + (player.frameWidth / 2) - (mainGame.GraphicsDevice.Viewport.Width / 2);
            CameraPositionDestination.Y = player.position.Y + (player.frameHeight / 2) - (mainGame.GraphicsDevice.Viewport.Height / 2);
            CameraPosition = Vector2.Lerp(CameraPosition, CameraPositionDestination, 0.1f);
            player.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.F2)) {
                mainGame.ChangeScene(new EditorScene(mainGame));
            }
        }

        public override void Draw() {
            tileMap.Draw(mainGame.spriteBatch, CameraPosition, mainGame.GraphicsDevice.Viewport);
            player.Draw(mainGame.spriteBatch, CameraPosition);
            mainGame.DrawFPSCounter(1, 1);
        }

        public override void DrawUI(GameTime gameTime) {
            
        }
    }
}
