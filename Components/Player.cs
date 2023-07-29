using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Zenith.Components {
    public class Player {
        private Texture2D texture;
        private Vector2 position;

        public Player(Texture2D texture, Vector2 position) {
            this.texture = texture;
            this.position = position;
        }

        public void Update(GameTime gameTime, TileMap tileMap) {
            // Handle player movement based on keyboard input and collision with tiles in the tilemap.
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
