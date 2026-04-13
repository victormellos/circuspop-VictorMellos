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

    // private List<Player> players;

    // private SoundEffect bounce;
    
    // private SpriteFont _textFont;

    private GameScreen gameScreen;
    private MainMenu mainMenu;
    private GameState gameState = GameState.Playing;

    public ClownGame()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 800,
            PreferredBackBufferHeight = 500
        };

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        switch (gameState)
        {
            case GameState.Playing:
                gameScreen = new GameScreen(GraphicsDevice, Content);// new List<Player>());
                gameScreen.Initialize();
                break;
            case GameState.MainMenu:
                mainMenu = new MainMenu(GraphicsDevice, Content);
                mainMenu.Initialize();
                break;
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        switch (gameState)
        {
            case GameState.Playing:
                gameScreen.LoadContent();
                break;
            case GameState.MainMenu:
                mainMenu.LoadContent();
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
                gameScreen.Update(gameTime, gameState);
                break;
            case GameState.MainMenu:
                mainMenu.Update(gameTime);
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
            case GameState.MainMenu:
                mainMenu.Draw();
                break;
        }

        base.Draw(gameTime);
    }
}