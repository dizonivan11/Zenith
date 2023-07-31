using Microsoft.Xna.Framework;

namespace Zenith.Scenes {
    public abstract class Scene {
        internal readonly MainGame mainGame;

        public Scene(MainGame mainGame) {
            this.mainGame = mainGame;
        }

        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw();
        public abstract void DrawUI(GameTime gameTime);
    }
}
