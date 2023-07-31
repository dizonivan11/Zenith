using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace Zenith.Components {
    public class TileMap {
        public static readonly int CHUNK_SIZE = 64;
        public static readonly int CULL_OFFSET = 2;

        public readonly int[,] mapData = new int[CHUNK_SIZE, CHUNK_SIZE];
        public readonly Texture2D tileset;
        public readonly int maxTilesetX;
        public readonly int tileWidth;
        public readonly int tileHeight;

        public TileMap(Texture2D tileset, string mapFile, int tileWidth, int tileHeight) {
            this.tileset = tileset;
            maxTilesetX = tileset.Width / tileWidth;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            LoadMapData(mapFile);
        }

        private void LoadMapData(string mapFile) {
            using (StreamReader reader = new(mapFile)) {
                int y = 0;
                while (!reader.EndOfStream) {
                    string[] line = reader.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    for (int x = 0; x < line.Length; x++) {
                        mapData[x, y] = int.Parse(line[x]);
                    }
                    y++;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition, Viewport viewport) {
            // Calculate the range of tiles visible on the screen based on the camera position and the viewport.
            int startX = MathHelper.Clamp((int)(cameraPosition.X / tileWidth) - CULL_OFFSET, 0, CHUNK_SIZE);
            int startY = MathHelper.Clamp((int)(cameraPosition.Y / tileHeight) - CULL_OFFSET, 0, CHUNK_SIZE);
            int endX = MathHelper.Clamp(startX + viewport.Width / tileWidth + 1 + (CULL_OFFSET * 2), 0, CHUNK_SIZE);
            int endY = MathHelper.Clamp(startY + viewport.Height / tileHeight + 1 + (CULL_OFFSET  * 2), 0, CHUNK_SIZE);

            for (int y = startY; y < endY; y++) {
                for (int x = startX; x < endX; x++) {
                    int tileID = mapData[x, y];

                    // Calculate the source rectangle based on the tileID and draw the tile.
                    int srcX = (tileID % maxTilesetX) * tileWidth;
                    int srcY = (tileID / maxTilesetX) * tileHeight;

                    spriteBatch.Draw(
                        tileset,
                        new Vector2((x * tileWidth) - cameraPosition.X, (y * tileHeight) - cameraPosition.Y),
                        new Rectangle(srcX, srcY, tileWidth, tileHeight),
                        Color.White);
                }
            }
        }

        public void DrawEditor(SpriteBatch spriteBatch, Vector2 cameraPosition, Viewport viewport, Texture2D selector, MouseState mouse) {
            // Calculate the range of tiles visible on the screen based on the camera position and the viewport.
            int startX = MathHelper.Clamp((int)(cameraPosition.X / tileWidth) - CULL_OFFSET, 0, CHUNK_SIZE);
            int startY = MathHelper.Clamp((int)(cameraPosition.Y / tileHeight) - CULL_OFFSET, 0, CHUNK_SIZE);
            int endX = MathHelper.Clamp(startX + viewport.Width / tileWidth + 1 + (CULL_OFFSET * 2), 0, CHUNK_SIZE);
            int endY = MathHelper.Clamp(startY + viewport.Height / tileHeight + 1 + (CULL_OFFSET * 2), 0, CHUNK_SIZE);

            for (int y = startY; y < endY; y++) {
                for (int x = startX; x < endX; x++) {
                    float destX = (x * tileWidth) - cameraPosition.X;
                    float destY = (y * tileHeight) - cameraPosition.Y;

                    if (!ImGui.IsAnyItemHovered() && new Rectangle((int)destX, (int)destY, tileWidth, tileHeight).Contains(mouse.Position)) {
                        spriteBatch.Draw(
                            selector,
                            new Vector2(destX, destY),
                            Color.White);

                        if (mouse.LeftButton == ButtonState.Pressed) {
                            mapData[x, y] = 182;
                        }
                    }
                }
            }
        }
    }
}
