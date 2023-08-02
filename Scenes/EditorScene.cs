using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Zenith.Components;
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
using NVector2 = System.Numerics.Vector2;

namespace Zenith.Scenes {
    public class EditorScene : Scene {
        const int TOOLBOX_WIDTH = 90;
        const int TILESET_WIDTH = 408;
        const int INFO_PADDING = 10;
        const int ACTION_BUFFER = 50; // Number of undo and redo action we can store

        TileMap tileMap;
        Texture2D[] tiles;
        Texture2D selector;
        Vector2 cameraPosition;
        Vector2 cameraPositionDestination;
        readonly float panSpeed;

        float mainMenuHeight;

        public EditorScene(MainGame mainGame) : base(mainGame) {
            panSpeed = 100.0f;
            cameraPosition = new Vector2(-TOOLBOX_WIDTH, 0);
            cameraPositionDestination = cameraPosition;
        }

        public override void LoadContent() {
            tileMap = new TileMap(
                Importer.GetTexture2DFromFile(mainGame.GraphicsDevice, "assets/tilesets/highlands_terrain.png"),
                "assets/maps/highlands_0_0.txt", 32, 32);

            // Create all Texture2D for all tiles which will be used by ImageButtons
            tiles = new Texture2D[tileMap.maxTilesetX * tileMap.maxTilesetY];
            for (int y = 0; y < tileMap.maxTilesetY; y++) {
                for (int x = 0; x < tileMap.maxTilesetX; x++) {
                    tiles[(y * tileMap.maxTilesetX) + x] =
                        tileMap.tileset.CreateTexture(
                            mainGame.GraphicsDevice,
                            new Rectangle(x * tileMap.tileWidth, y * tileMap.tileHeight, tileMap.tileWidth, tileMap.tileHeight));
                }
            }

            selector = Importer.GetTexture2DFromFile(mainGame.GraphicsDevice, "assets/gfx/selector.png");
        }

        public override void Update(GameTime gameTime) {
            Vector2 dir = Vector2.Zero;
            if (mainGame.gameInput.KeyDown(Keys.W)) dir.Y = -1;
            if (mainGame.gameInput.KeyDown(Keys.S)) dir.Y = 1;
            if (mainGame.gameInput.KeyDown(Keys.A)) dir.X = -1;
            if (mainGame.gameInput.KeyDown(Keys.D)) dir.X = 1;
            if (dir != Vector2.Zero) {
                dir.Normalize();
                cameraPositionDestination = cameraPosition + dir * panSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            cameraPosition = Vector2.Lerp(cameraPosition, cameraPositionDestination, 0.01f);
            
            // Snap the camera if its almost at the destination to prevent pixel jittering causing blur
            if (Vector2.Distance(cameraPosition, cameraPositionDestination) <= 1f) cameraPositionDestination = cameraPosition;

            tileMap.UpdateEditor(cameraPosition, mainGame.GraphicsDevice.Viewport, mainGame.guiInput, mainGame.gameInput);

            if (mainGame.gameInput.KeyPressed(Keys.F1))
                mainGame.ChangeScene(new GameScene(mainGame));

            if (mainGame.gameInput.KeyPressed(Keys.F2))
                windowDemo = !windowDemo;
        }

        public override void Draw() {
            Editor.TilesRendered = 0;
            tileMap.Draw(mainGame.spriteBatch, cameraPosition, mainGame.GraphicsDevice.Viewport);
            tileMap.DrawEditor(mainGame.spriteBatch, cameraPosition, mainGame.GraphicsDevice.Viewport, mainGame.guiInput, mainGame.gameInput, selector);
        }

        /// <summary>
        /// ImGui Notes:
        /// Declaring a bool is like toggling windows, setting it to true makes it visible
        /// It can also be set as false initially then control it on other widgets/functions
        /// </summary>
        bool windowNotice = false;
        string stringNotice = string.Empty;
        bool windowDemo = false;
        bool windowTools = true;
        bool windowTileset = true;
        bool windowInfo = true;
        public override void DrawUI(GameTime gameTime) {
            ImGui.BeginMainMenuBar();
            mainMenuHeight = ImGui.GetWindowSize().Y;
            if (ImGui.BeginMenu("File")) {
                ImGui.MenuItem("New");
                ImGui.MenuItem("Open");
                if (ImGui.MenuItem("Save")) {
                    stringNotice = tileMap.SaveMapData();
                    windowNotice = true;
                }
                ImGui.MenuItem("Save as");
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Edit")) {
                ImGui.MenuItem("Undo");
                ImGui.MenuItem("Redo");
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Window")) {
                if (ImGui.MenuItem("Tools", null, windowTools)) {
                    windowTools = !windowTools;
                }
                if (ImGui.MenuItem("Tileset", null, windowTileset)) {
                    windowTileset = !windowTileset;
                }
                if (ImGui.MenuItem("Info", null, windowInfo)) {
                    windowInfo = !windowInfo;
                }
                ImGui.EndMenu();
            }
            ImGui.EndMainMenuBar();

            if (windowNotice) {
                ImGui.SetNextWindowPos(new NVector2(
                    mainGame.GraphicsDevice.Viewport.Width / 2,
                    mainGame.GraphicsDevice.Viewport.Height / 2),
                    ImGuiCond.Always, new NVector2(0.5f, 0.5f));
                ImGui.Begin("Notice",
                        ImGuiWindowFlags.NoResize |
                        ImGuiWindowFlags.NoSavedSettings |
                        ImGuiWindowFlags.Modal |
                        ImGuiWindowFlags.NoCollapse);
                ImGui.Text(stringNotice);
                if (ImGui.Button("Ok")) windowNotice = false;
                ImGui.End();
            }

            if (windowDemo)
                ImGui.ShowDemoWindow(ref windowDemo);

            if (windowTools) {
                ImGui.SetNextWindowPos(new NVector2(0, mainMenuHeight), ImGuiCond.None);
                ImGui.SetNextWindowSize(new NVector2(TOOLBOX_WIDTH, mainGame.GraphicsDevice.Viewport.Height - mainMenuHeight));
                ImGui.Begin("Tools",
                    ref windowTools,
                    ImGuiWindowFlags.NoSavedSettings |
                    ImGuiWindowFlags.NoMove |
                    ImGuiWindowFlags.NoResize);
                ImGui.Button("Select", new NVector2(-1, 20));
                ImGui.Button("Move", new NVector2(-1, 20));
                ImGui.Button("Brush", new NVector2(-1, 20));
                ImGui.Button("Erase", new NVector2(-1, 20));
                ImGui.Button("Fill", new NVector2(-1, 20));
                ImGui.End();
            }

            if (windowTileset) {
                ImGui.SetNextWindowPos(new NVector2(mainGame.GraphicsDevice.Viewport.Width - TILESET_WIDTH, mainMenuHeight), ImGuiCond.None);
                ImGui.SetNextWindowSize(new NVector2(TILESET_WIDTH, mainGame.GraphicsDevice.Viewport.Height - mainMenuHeight));
                ImGui.Begin("Tileset",
                    ref windowTileset,
                    ImGuiWindowFlags.NoSavedSettings |
                    ImGuiWindowFlags.NoMove |
                    ImGuiWindowFlags.NoResize |
                    ImGuiWindowFlags.HorizontalScrollbar |
                    ImGuiWindowFlags.AlwaysVerticalScrollbar);
                for (int y = 0; y < tileMap.maxTilesetY; y++) {
                    for (int x = 0; x < tileMap.maxTilesetX; x++) {
                        int t = (y * tileMap.maxTilesetX) + x;
                        if (ImGui.ImageButton(string.Format("{0}-{1}", x, y),
                            mainGame.ui.BindTexture(tiles[t]),
                            new NVector2(tileMap.tileWidth, tileMap.tileHeight))) {
                            Editor.SelectedTile = t;
                        }
                        if ((t + 1) % tileMap.maxTilesetX != 0) ImGui.SameLine();
                    }
                }
                ImGui.End();
            }

            if (windowInfo) {
                ImGui.SetNextWindowPos(new NVector2(TOOLBOX_WIDTH + INFO_PADDING, mainMenuHeight + INFO_PADDING), ImGuiCond.None);
                ImGui.Begin("Info",
                    ref windowInfo,
                    ImGuiWindowFlags.NoSavedSettings |
                    ImGuiWindowFlags.NoMove |
                    ImGuiWindowFlags.NoResize |
                    ImGuiWindowFlags.NoTitleBar |
                    ImGuiWindowFlags.NoBackground);
                ImGui.Text(string.Format("FPS: {0}", (int)mainGame.fps.AverageFramesPerSecond));
                ImGui.Text(string.Format("Tiles Rendered: {0}", Editor.TilesRendered));
                ImGui.Text(string.Format("Mouse Position: X:{0} Y:{1}", mainGame.gameInput.MousePosition.X, mainGame.gameInput.MousePosition.Y));
                ImGui.End();
            }
        }
    }
}
