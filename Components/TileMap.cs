using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Zenith.Components {
    public class TileMap {
        public static readonly int CHUNK_SIZE = 64;

        private int[,] mapData = new int[CHUNK_SIZE, CHUNK_SIZE];
        private Texture2D tileset;
        private int maxTilesetX;
        private int maxTilesetY;
        private int tileWidth;
        private int tileHeight;

        public TileMap(Texture2D tileset, string mapFile, int tileWidth, int tileHeight) {
            this.tileset = tileset;
            maxTilesetX = tileset.Width / tileWidth;
            maxTilesetY = tileset.Height / tileHeight;
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
            int startX = MathHelper.Clamp((int)(cameraPosition.X / tileWidth), 0, CHUNK_SIZE);
            int startY = MathHelper.Clamp((int)(cameraPosition.Y / tileHeight), 0, CHUNK_SIZE);
            int endX = MathHelper.Clamp(startX + viewport.Width / tileWidth + 1, 0, CHUNK_SIZE);
            int endY = MathHelper.Clamp(startY + viewport.Height / tileHeight + 1, 0, CHUNK_SIZE);

            for (int y = startY; y < endY; y++) {
                for (int x = startX; x < endX; x++) {
                    int tileID = mapData[x, y];
                    int srcX = (tileID % maxTilesetX) * tileWidth;
                    int srcY = (tileID / maxTilesetX) * tileHeight;
                    // Calculate the source rectangle based on the tileID and draw the tile.
                    spriteBatch.Draw(tileset, new Vector2((x * tileWidth) - cameraPosition.X, (y * tileHeight) - cameraPosition.Y), new Rectangle(srcX, srcY, tileWidth, tileHeight), Color.White);
                }
            }
        }
    }
}
