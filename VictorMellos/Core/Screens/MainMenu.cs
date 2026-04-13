using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Data;
using Microsoft.Xna.Framework.Content;


namespace VictorMellos;

public class MainMenu
{
    private SpriteFont _textFont;

    private SpriteBatch _spriteBatch;

    private GraphicsDevice _graphicsDevice;
    
    // private List<Player> players;
    private ContentManager _content;
    
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


    }

    public void Update(GameTime gameTime)

    {
        
        
    }

    public void Draw()
    {

        _graphicsDevice.Clear(Color.YellowGreen);

        _spriteBatch.Begin();
        _spriteBatch.DrawString(
            _textFont,
            "JOGO LEGAL DO PALHACO FELIZ",
            new Vector2(0, 0),
            Color.White
        );


        string texto = "Pressione START para jogar";
        Vector2 tamanho = _textFont.MeasureString(texto);

        float posX = (_graphicsDevice.Viewport.Width - tamanho.X) / 2;
        float posY = _graphicsDevice.Viewport.Height - tamanho.Y - 10;

        _spriteBatch.DrawString(
            _textFont,
            texto,
            new Vector2(posX, posY),
            Color.White
        );
        _spriteBatch.End();
    }
}