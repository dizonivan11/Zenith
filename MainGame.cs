using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Zenith.Components;

public class MainGame : Game {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    TileMap tileMap;
    Player player;

    public MainGame() {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    protected override void LoadContent() {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        tileMap = new TileMap(Content.Load<Texture2D>("tileset"), "mapdata.txt");
        player = new Player(Content.Load<Texture2D>("player"), new Vector2(0, 0));
    }

    protected override void Update(GameTime gameTime) {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        player.Update(gameTime, tileMap);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        spriteBatch.Begin();
        tileMap.Draw(spriteBatch);
        player.Draw(spriteBatch);
        spriteBatch.End();

        base.Draw(gameTime);
    }
}