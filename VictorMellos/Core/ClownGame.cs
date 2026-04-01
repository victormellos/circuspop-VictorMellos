using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Data;


namespace VictorMellos;

public class ClownGame : Game
{
    private GraphicsDeviceManager _graphics;
    // private SpriteBatch _spriteBatch;

    private List<Player> players;

    // private SoundEffect bounce;
    
    // private SpriteFont _textFont;

    private GameScreen gameScreen;
    private GameState gameState = GameState.Playing;

    public ClownGame()
    {
        _graphics = new GraphicsDeviceManager(this);

        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 500;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        players = new List<Player>();

        var clown1 = new Character(new Vector2(100f, 120f), Vector2.Zero);
        var trampoline1 = new Character(new Vector2(100f, 400f), Vector2.Zero);

        players.Add(new Player(trampoline1, clown1, Keys.Left, Keys.Right));

        base.Initialize();
    }

    protected override void LoadContent()
    {
        switch (gameState)
        {
            case GameState.Playing:
                gameScreen = new GameScreen(GraphicsDevice, Content, players);
                gameScreen.LoadContent();
                break;
        }
        // _spriteBatch = new SpriteBatch(GraphicsDevice);

        // bounce = Content.Load<SoundEffect>("bounce");

        // gameScreen = new GameScreen(GraphicsDevice, Content, players);

        // _textFont = Content.Load<SpriteFont>("ScoreFont"); 



    }

    protected override void Update(GameTime gameTime)

    {
        switch (gameState)
        {
            case GameState.Playing:
                gameScreen.Update(gameTime);
                break;
            default:
                Console.Write("Erro esquisitão");
                break;
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        switch (gameState)  
        {
            case GameState.Playing:
                gameScreen.Draw();
                break;
        }

        base.Draw(gameTime);
    }
}