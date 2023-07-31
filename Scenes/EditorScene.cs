using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Scenes {
    public class EditorScene : Scene {
        public EditorScene(MainGame mainGame) : base(mainGame) {

        }

        public override void LoadContent() {

        }

        public override void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.F2)) {
                mainGame.ChangeScene(new GameScene(mainGame));
            }
        }

        public override void Draw() {
            var fpsString = "EDITOR";
            mainGame.spriteBatch.DrawString(mainGame.regularFont, fpsString, new Vector2(250, 250), Color.Black);
        }
    }
}
