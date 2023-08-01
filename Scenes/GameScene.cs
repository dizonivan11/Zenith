using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Zenith.Components;

namespace Zenith.Scenes {
    public class GameScene : Scene {
        TileMap tileMap;
        Player player;
        Vector2 cameraPosition;
        Vector2 cameraPositionDestination;

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
            if (mainGame.gameInput.KeyDown(Keys.W)) {
                dir.Y = -1;
                player.currentFrameIndex = 8;
            }
            if (mainGame.gameInput.KeyDown(Keys.S)) {
                dir.Y = 1;
                player.currentFrameIndex = 10;
            }
            if (mainGame.gameInput.KeyDown(Keys.A)) {
                dir.X = -1;
                player.currentFrameIndex = 9;
            }
            if (mainGame.gameInput.KeyDown(Keys.D)) {
                dir.X = 1;
                player.currentFrameIndex = 11;
            }
            if (dir != Vector2.Zero) {
                dir.Normalize();
                player.position += dir * player.speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            cameraPositionDestination.X = player.position.X + (player.frameWidth / 2) - (mainGame.GraphicsDevice.Viewport.Width / 2);
            cameraPositionDestination.Y = player.position.Y + (player.frameHeight / 2) - (mainGame.GraphicsDevice.Viewport.Height / 2);
            cameraPosition = Vector2.Lerp(cameraPosition, cameraPositionDestination, 0.1f);

            // Snap the camera if its almost at the destination to prevent pixel jittering causing blur
            if (Vector2.Distance(cameraPosition, cameraPositionDestination) <= 1f) cameraPosition = cameraPositionDestination;

            player.Update(gameTime);

            if (mainGame.gameInput.KeyPressed(Keys.F2))
                mainGame.ChangeScene(new EditorScene(mainGame));
        }

        public override void Draw() {
            tileMap.Draw(mainGame.spriteBatch, cameraPosition, mainGame.GraphicsDevice.Viewport);
            player.Draw(mainGame.spriteBatch, cameraPosition);
            mainGame.DrawFPSCounter(1, 1);
        }

        public override void DrawUI(GameTime gameTime) {
            
        }
    }
}
