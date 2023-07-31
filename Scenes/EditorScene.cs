using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Zenith.Components;

namespace Zenith.Scenes {
    public class EditorScene : Scene {
        readonly int ToolboxSize = 90;
        TileMap tileMap;
        Vector2 cameraPosition;
        Vector2 cameraPositionDestination;
        float panSpeed;

        public EditorScene(MainGame mainGame) : base(mainGame) {
            panSpeed = 20.0f;
        }

        public override void LoadContent() {
            tileMap = new TileMap(
                Importer.GetTexture2DFromFile(mainGame.GraphicsDevice, "assets/tilesets/highlands_terrain.png"),
                "assets/maps/highlands_0_0.txt", 32, 32);

            var grid = new Grid {
                ShowGridLines = true,
                RowSpacing = 8,
                ColumnSpacing = 8
            };

            grid.ColumnsProportions.Add(new Proportion(ProportionType.Pixels, ToolboxSize));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Fill));
            grid.RowsProportions.Add(new Proportion(ProportionType.Fill));
            grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 200));

            ScrollViewer svTileset = new() { GridColumn = 1, GridRow = 1 };
            grid.Widgets.Add(svTileset);

            Image tileset = new() { Renderable = new TextureRegion(tileMap.tileset, tileMap.tileset.Bounds) };
            svTileset.Content = tileset;

            Window toolbox = new() {
                Title = "Tools",
                Left = 0,
                Top = 0,
            };
            TextButton button = new TextButton {
                Text = "S",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            toolbox.Content = button;

            Window w1 = new() {
                Title = "Properties",
                Left = 0,
                Top = 0,
            };
            TextButton b1 = new TextButton {
                Text = "W",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            w1.Content = b1;

            mainGame.desktop.Root = grid;
            toolbox.Show(mainGame.desktop);
            w1.Show(mainGame.desktop);
        }

        public override void Update(GameTime gameTime) {
            Vector2 dir = Vector2.Zero;
            if (Keyboard.GetState().IsKeyDown(Keys.W)) {
                dir.Y = -1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S)) {
                dir.Y = 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A)) {
                dir.X = -1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D)) {
                dir.X = 1;
            }
            if (dir != Vector2.Zero) {
                dir.Normalize();
                cameraPositionDestination = cameraPosition + dir * panSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            cameraPosition = Vector2.Lerp(cameraPosition, cameraPositionDestination, 0.1f);

            if (Keyboard.GetState().IsKeyDown(Keys.F1)) {
                mainGame.ChangeScene(new GameScene(mainGame));
            }
        }

        public override void Draw() {
            tileMap.Draw(mainGame.spriteBatch, cameraPosition, mainGame.GraphicsDevice.Viewport);
            mainGame.DrawFPSCounter(ToolboxSize + 10, 1);
        }
    }
}
