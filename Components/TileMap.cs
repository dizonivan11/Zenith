using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Text;

namespace Zenith.Components {
    public class TileMap {
        public const int CHUNK_SIZE = 64;
        public const int CULL_OFFSET = 2;

        public readonly int[,] mapData = new int[CHUNK_SIZE, CHUNK_SIZE];
        public readonly string mapFile;
        public readonly Texture2D tileset;
        public readonly int maxTilesetX;
        public readonly int maxTilesetY;
        public readonly int tileWidth;
        public readonly int tileHeight;

        public TileMap(Texture2D tileset, string mapFile, int tileWidth, int tileHeight) {
            this.mapFile = mapFile;
            this.tileset = tileset;
            maxTilesetX = tileset.Width / tileWidth;
            maxTilesetY = tileset.Height / tileHeight;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            LoadMapData();
        }

        private void LoadMapData() {
            using StreamReader reader = new(mapFile);
            string[] data = reader.ReadToEnd().Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int d = 0; d < data.Length; d++) {
                int x = d % CHUNK_SIZE;
                int y = d / CHUNK_SIZE;
                mapData[x, y] = int.Parse(data[d]);
            }
        }

        public string SaveMapData() {
            try {
                StringBuilder data = new();
                for (int y = 0; y < CHUNK_SIZE; y++) {
                    for (int x = 0; x < CHUNK_SIZE; x++) {
                        data.Append(mapData[x, y]);
                        data.Append(' ');
                    }
                }
                using StreamWriter writer = new(mapFile);
                writer.Write(data.ToString().Trim());
                return "File saved";
            } catch (IOException ex) { return ex.Message; }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition, Viewport viewport) {
            // Calculate the range of tiles visible on the screen based on the camera position and the viewport.
            int startX = (int)(cameraPosition.X / tileWidth) - CULL_OFFSET;
            int startY = (int)(cameraPosition.Y / tileHeight) - CULL_OFFSET;
            int endX = MathHelper.Clamp(startX + viewport.Width / tileWidth + 1 + (CULL_OFFSET * 2), 0, CHUNK_SIZE);
            int endY = MathHelper.Clamp(startY + viewport.Height / tileHeight + 1 + (CULL_OFFSET * 2), 0, CHUNK_SIZE);
            startX = MathHelper.Clamp(startX, 0, CHUNK_SIZE);
            startY = MathHelper.Clamp(startY, 0, CHUNK_SIZE);
            Editor.TilesRendered += (endX - startX) * (endY - startY);

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

        public void UpdateEditor(Vector2 cameraPosition, Viewport viewport, ImGuiIOPtr guiInput, InputManager gameInput) {
            // Calculate the range of tiles visible on the screen based on the camera position and the viewport.
            int startX = (int)(cameraPosition.X / tileWidth) - CULL_OFFSET;
            int startY = (int)(cameraPosition.Y / tileHeight) - CULL_OFFSET;
            int endX = MathHelper.Clamp(startX + viewport.Width / tileWidth + 1 + (CULL_OFFSET * 2), 0, CHUNK_SIZE);
            int endY = MathHelper.Clamp(startY + viewport.Height / tileHeight + 1 + (CULL_OFFSET * 2), 0, CHUNK_SIZE);
            startX = MathHelper.Clamp(startX, 0, CHUNK_SIZE);
            startY = MathHelper.Clamp(startY, 0, CHUNK_SIZE);

            for (int y = startY; y < endY; y++) {
                for (int x = startX; x < endX; x++) {
                    float destX = (x * tileWidth) - cameraPosition.X;
                    float destY = (y * tileHeight) - cameraPosition.Y;

                    if (!guiInput.WantCaptureMouse && new Rectangle((int)destX, (int)destY, tileWidth, tileHeight).Contains(gameInput.MousePosition)) {
                        if (gameInput.MouseDown(MouseButton.Left)) {
                            mapData[x, y] = Editor.SelectedTile;
                        }
                    }
                }
            }
        }

        public void DrawEditor(SpriteBatch spriteBatch, Vector2 cameraPosition, Viewport viewport,
            ImGuiIOPtr guiInput, InputManager gameInput, Texture2D selector) {
            // Calculate the range of tiles visible on the screen based on the camera position and the viewport.
            int startX = (int)(cameraPosition.X / tileWidth) - CULL_OFFSET;
            int startY = (int)(cameraPosition.Y / tileHeight) - CULL_OFFSET;
            int endX = MathHelper.Clamp(startX + viewport.Width / tileWidth + 1 + (CULL_OFFSET * 2), 0, CHUNK_SIZE);
            int endY = MathHelper.Clamp(startY + viewport.Height / tileHeight + 1 + (CULL_OFFSET * 2), 0, CHUNK_SIZE);
            startX = MathHelper.Clamp(startX, 0, CHUNK_SIZE);
            startY = MathHelper.Clamp(startY, 0, CHUNK_SIZE);

            for (int y = startY; y < endY; y++) {
                for (int x = startX; x < endX; x++) {
                    float destX = (x * tileWidth) - cameraPosition.X;
                    float destY = (y * tileHeight) - cameraPosition.Y;

                    if (!guiInput.WantCaptureMouse && new Rectangle((int)destX, (int)destY, tileWidth, tileHeight).Contains(gameInput.MousePosition)) {
                        spriteBatch.Draw(
                            selector,
                            new Vector2(destX, destY),
                            Color.White);
                    }
                }
            }
        }
    }
}
