using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Zenith.Components;

namespace Zenith.Components {
    public class Player {
        private Texture2D spritesheet;
        private Vector2 position;
        private int frameWidth;
        private int frameHeight;
        private int currentFrame;
        private int totalFrames;
        private float frameTime;
        private float timeElapsed;

        public Player(Texture2D spritesheet, Vector2 position, int frameWidth, int frameHeight, int totalFrames, float frameTime) {
            this.spritesheet = spritesheet;
            this.position = position;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.totalFrames = totalFrames;
            this.frameTime = frameTime;
            this.currentFrame = 0;
            this.timeElapsed = 0f;
        }

        public void Update(GameTime gameTime, TileMap tileMap) {
            // Handle player movement based on keyboard input and collision with tiles in the tilemap.

            // Update the animation frame.
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed > frameTime) {
                timeElapsed -= frameTime;
                currentFrame = (currentFrame + 1) % totalFrames;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            // Calculate the source rectangle based on the current animation frame.
            Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);

            spriteBatch.Draw(spritesheet, position, sourceRect, Color.White);
        }
    }
}
