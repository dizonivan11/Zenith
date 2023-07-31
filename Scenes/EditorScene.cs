﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Zenith.Components;
using ImGuiNET;

namespace Zenith.Scenes {
    public class EditorScene : Scene {
        readonly int ToolboxWidth = 90;
        readonly int TilesetWidth = 400;

        TileMap tileMap;
        Vector2 cameraPosition;
        Vector2 cameraPositionDestination;
        readonly float panSpeed;

        public EditorScene(MainGame mainGame) : base(mainGame) {
            panSpeed = 20.0f;
        }

        public override void LoadContent() {
            tileMap = new TileMap(
                Importer.GetTexture2DFromFile(mainGame.GraphicsDevice, "assets/tilesets/highlands_terrain.png"),
                "assets/maps/highlands_0_0.txt", 32, 32);
        }

        public override void Update(GameTime gameTime) {
            Vector2 dir = Vector2.Zero;
            if (Keyboard.GetState().IsKeyDown(Keys.W)) dir.Y = -1;
            if (Keyboard.GetState().IsKeyDown(Keys.S)) dir.Y = 1;
            if (Keyboard.GetState().IsKeyDown(Keys.A)) dir.X = -1;
            if (Keyboard.GetState().IsKeyDown(Keys.D)) dir.X = 1;
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
            mainGame.DrawFPSCounter(ToolboxWidth + 10, 1);
        }

        /// <summary>
        /// ImGui Notes:
        /// Declaring a bool is like creating windows, setting it to true makes it visible
        /// It can also be set as false initially then control it on other widgets/functions
        /// </summary>
        bool windowTools = true;
        bool windowTileset = true;
        public override void DrawUI(GameTime gameTime) {
            if (windowTools) {
                ImGui.SetNextWindowPos(new System.Numerics.Vector2(0, 0), ImGuiCond.FirstUseEver);
                ImGui.SetNextWindowSize(new System.Numerics.Vector2(ToolboxWidth, mainGame.GraphicsDevice.Viewport.Height));
                ImGui.Begin("Tools",
                    ref windowTools,
                    ImGuiWindowFlags.NoSavedSettings |
                    ImGuiWindowFlags.NoMove |
                    ImGuiWindowFlags.NoResize);
                ImGui.Button("Select", new System.Numerics.Vector2(-1, 20));
                ImGui.Button("Move", new System.Numerics.Vector2(-1, 20));
                ImGui.Button("Brush", new System.Numerics.Vector2(-1, 20));
                ImGui.Button("Erase", new System.Numerics.Vector2(-1, 20));
                ImGui.Button("Fill", new System.Numerics.Vector2(-1, 20));
                ImGui.End();
            }
            if (windowTileset) {
                ImGui.SetNextWindowPos(new System.Numerics.Vector2(mainGame.GraphicsDevice.Viewport.Width - TilesetWidth, 0), ImGuiCond.FirstUseEver);
                ImGui.SetNextWindowSize(new System.Numerics.Vector2(TilesetWidth, mainGame.GraphicsDevice.Viewport.Height));
                ImGui.Begin("Tileset",
                    ref windowTileset,
                    ImGuiWindowFlags.NoSavedSettings |
                    ImGuiWindowFlags.NoMove |
                    ImGuiWindowFlags.NoResize |
                    ImGuiWindowFlags.AlwaysHorizontalScrollbar |
                    ImGuiWindowFlags.AlwaysVerticalScrollbar);
                ImGui.Image(mainGame.ui.BindTexture(tileMap.tileset), new System.Numerics.Vector2(tileMap.tileset.Width, tileMap.tileset.Height));
                ImGui.End();
            }
        }
    }
}