using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;

namespace VictorMellos;

class GameScreen
{
    private SoundEffect bounce;
    private SpriteFont _textFont;
    private SpriteBatch _spriteBatch;

    private GraphicsDevice _graphicsDevice;
    
    private List<Player> players;
    private ContentManager _content;

    public GameScreen(GraphicsDevice graphicsDevice, ContentManager content, List<Player> players)
    {
        _graphicsDevice = graphicsDevice;
        _content = content;
        this.players = players;
    }

    public void LoadContent()
    {  
        
        _spriteBatch = new SpriteBatch(_graphicsDevice);

        bounce = _content.Load<SoundEffect>("bounce");
        _textFont = _content.Load<SpriteFont>("ScoreFont");
                foreach (var player in players)
        {
            player.Clown.Sprite = new Sprite();
            player.Clown.Sprite.Texture = _content.Load<Texture2D>("clown");

            

            player.Trampoline.Sprite = new Sprite();
            player.Trampoline.Sprite.Texture = _content.Load<Texture2D>("Trampoline");

                player.Trampoline.Position = new Vector2(
                    player.Trampoline.Position.X,
                    _graphicsDevice.Viewport.Height - player.Trampoline.Height
                );
        }
 
    }
    public void Initialize()
    {
        players = new List<Player>();

        var clown1 = new Character(new Vector2(100f, 120f), Vector2.Zero);
        var trampoline1 = new Character(new Vector2(100f, 400f), Vector2.Zero);

        players.Add(new Player(trampoline1, clown1, Keys.Left, Keys.Right));

    }
    public void Update(GameTime gameTime)
    {
            /*
    Ordem para verificar:
    Input
    Movimento
    Física (gravidade, velocidade, etc.)
    Colisão
    Resposta da colisão
    */
            foreach (var player in players)
        {
            player.HandleInput();
        }
        
        foreach (var player in players)
        {
            var clown = player.Clown;
            if (clown == null)
                continue;

            var trampoline = player.Trampoline;

            // 1. Clamp do trampolim
            trampoline.Position.X = MathHelper.Clamp(
                trampoline.Position.X,
                0,
                _graphicsDevice.Viewport.Width - trampoline.Width
            );

            // 2. Movimento do clown
            clown.Position += clown.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // 3. Gravidade
            clown.Velocity.Y += 16f;

            // 4. Limites da tela
            int maxX = _graphicsDevice.Viewport.Width - clown.Width;
            int maxY = _graphicsDevice.Viewport.Height - clown.Height;

            if (clown.Position.Y > maxY)
            {
                player.Score.AddPoints(-1200);
                player.Lives --;

                if (player.Lives <= 0)
                {
                    player.IsAlive = false;
                }
                
                clown.Position = trampoline.Position + new Vector2(trampoline.Height, 0);

            }

            if (clown.Position.X > maxX)
            {
                clown.Velocity.X *= -1;
                clown.Position.X = maxX;
            }
            else if (clown.Position.X < 0)
            {
                clown.Velocity.X *= -1;
                clown.Position.X = 0;
            }




            // 6. Colisão
            if (Collision.Intersects(trampoline, clown)){
                int angle = trampoline.Bounds.Center.X - clown.Bounds.Center.X + 90;

                clown.Velocity.X = 980f * (float)Math.Cos(MathHelper.ToRadians(angle));
                clown.Velocity.Y = 980f * (float)Math.Sin(MathHelper.ToRadians(angle)) * -1;

                bounce.Play();
            }
        }
    }

    public void Draw()
    {
        _graphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        
        Vector2 scorePosition = new Vector2(10, 10);
        float offsetY = 0f;

        foreach (var player in players)
        {
            if (player.Clown != null)
            {
                _spriteBatch.Draw(player.Clown.Sprite.Texture, player.Clown.Position, Color.White);
            }
            
            _spriteBatch.Draw(player.Trampoline.Sprite.Texture, player.Trampoline.Position, Color.White);

            string scoreText = $"{player.Name} : {player.Score.Points}\nVidas: {player.Lives}";
            
            _spriteBatch.DrawString(
                _textFont,
                scoreText,
                new Vector2(scorePosition.X, scorePosition.Y + offsetY),
                Color.White
            );

            offsetY += 30f; 
        }

        _spriteBatch.End();


    }
}