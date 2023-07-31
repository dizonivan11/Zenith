using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Zenith.Components;

namespace Zenith.Components {
    public class Player {
        Texture2D spritesheet;
        public Vector2 position;
        public float speed;
        public int frameWidth;
        public int frameHeight;
        public int currentFrameIndex;
        int currentFrame;
        int totalFrames;
        float frameTime;
        float timeElapsed;

        public Player(Texture2D spritesheet, Vector2 position, float speed, int frameWidth, int frameHeight, int initialFrameIndex, int totalFrames, float frameTime) {
            this.spritesheet = spritesheet;
            this.position = position;
            this.speed = speed;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            currentFrameIndex = initialFrameIndex;
            this.totalFrames = totalFrames;
            this.frameTime = frameTime;
            currentFrame = 0;
            timeElapsed = 0f;
        }

        public void Update(GameTime gameTime) {
            // Update the animation frame.
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed > frameTime) {
                timeElapsed -= frameTime;
                currentFrame = (currentFrame + 1) % totalFrames;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition) {
            // Calculate the source rectangle based on the current animation frame.
            Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, currentFrameIndex * frameHeight, frameWidth, frameHeight);

            spriteBatch.Draw(spritesheet,
                position - cameraPosition,
                sourceRect, Color.White);
        }
    }
}
