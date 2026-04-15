using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Data;
using Microsoft.Xna.Framework.Content;
using System.ComponentModel;


namespace VictorMellos;

public class MainMenu
{
    private SpriteFont _textFont;

    private SpriteBatch _spriteBatch;

    private GraphicsDevice _graphicsDevice;
    
    // private List<Player> players;
    private ContentManager _content;
    private Texture2D background;
    
    // private GameState gameState = GameState.MainMenu;

    public MainMenu(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _graphicsDevice = graphicsDevice;
        _content = content;
        _spriteBatch = new SpriteBatch(graphicsDevice);
    }
    
    public void Initialize()
    {

    }

    public void LoadContent()
    {
        _textFont = _content.Load<SpriteFont>("fonts/ScoreFont");
        background = _content.Load<Texture2D>("images/background");        

    }

    public void Update(GameTime gameTime)

    {
        
        
    }

    public void Draw()
    {

        _graphicsDevice.Clear(Color.YellowGreen);

        _spriteBatch.Begin();

        _spriteBatch.Draw(background, new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), new Color(255, 255, 255, 0.5f));

        _spriteBatch.DrawString(
            _textFont,
            "JOGO LEGAL DO PALHACO FELIZ",
            new Vector2(0, 0),
            Color.Black
        );


        string texto = "Pressione START para jogar";
        Vector2 tamanho = _textFont.MeasureString(texto);

        float posX = (_graphicsDevice.Viewport.Width - tamanho.X) / 2;
        float posY = _graphicsDevice.Viewport.Height - tamanho.Y - 10;

        _spriteBatch.DrawString(
            _textFont,
            texto,
            new Vector2(posX, posY),
            Color.Black
        );
        _spriteBatch.End();
    }
}