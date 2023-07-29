using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Zenith.Components {
    public class TileMap {
        private int[,] mapData = new int[0,0];
        private Texture2D tileset;
        private int tileWidth;
        private int tileHeight;

        public TileMap(Texture2D tileset, string mapFile, int tileWidth, int tileHeight) {
            this.tileset = tileset;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            LoadMapData(mapFile);
        }

        private void LoadMapData(string mapFile) {
            using (StreamReader reader = new StreamReader(mapFile)) {
                // Read the map data from the file into the 2D array.
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition, Viewport viewport) {
            // Calculate the range of tiles visible on the screen based on the camera position and the viewport.
            int startX = (int)MathHelper.Clamp((int)(cameraPosition.X / tileWidth), 0, mapData.GetLength(0));
            int startY = (int)MathHelper.Clamp((int)(cameraPosition.Y / tileHeight), 0, mapData.GetLength(1));
            int endX = (int)MathHelper.Clamp(startX + viewport.Width / tileWidth + 1, 0, mapData.GetLength(0));
            int endY = (int)MathHelper.Clamp(startY + viewport.Height / tileHeight + 1, 0, mapData.GetLength(1));

            for (int y = startY; y < endY; y++) {
                for (int x = startX; x < endX; x++) {
                    int tileID = mapData[x, y];
                    int srcX = 0;
                    int srcY = 0;
                    // Calculate the source rectangle based on the tileID and draw the tile.
                    spriteBatch.Draw(tileset, new Vector2(x * tileWidth, y * tileHeight), new Rectangle(srcX, srcY, tileWidth, tileHeight), Color.White);
                }
            }
        }
    }
}
