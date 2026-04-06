using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;
using System.Reflection.Metadata;

namespace VictorMellos;

class GameScreen
{
    private SoundEffect bounce;
    private SoundEffect pop;

    private SpriteFont _textFont;
    private SpriteBatch _spriteBatch;

    private GraphicsDevice _graphicsDevice;
    
    private List<Player> players;
    private ContentManager _content;
    

    //private Rectangle spawnArea;

    private bool created;

    private Random random = new Random();
    private List<Rectangle> balloonAreas = new();

    private Delay RespawnTimer = new Delay(500.0);    

    private Balloon balloon;

    private List<Balloon> balloons = new();

    public GameScreen(GraphicsDevice graphicsDevice, ContentManager content)//, List<Player> players)
    {
        _graphicsDevice = graphicsDevice;
        _content = content;
        // this.players = players;
    }


    public void CreateBalloons(int quantity)
{
    if (created)
        {
            return;
        }
    balloons.Clear();

    int balloonWidth = 64;
    
    int balloonHeight = 64;


    int spacingX = -15  ;
    int spacingY = 0;

    int columns = (_graphicsDevice.Viewport.Width + spacingX) / (balloonWidth + spacingX);
    
    for (int i = 0; i < quantity; i++) 
    {
        for (int j = 0; j < columns; j++)
        {
            int x = j * (balloonWidth + spacingX);
            int y = i * (balloonHeight + spacingY);

            Balloon balloonInstance = new Balloon(
                new Vector2(x, y),
                random.Next(200, 1001)
            )
            {
                Sprite = balloon.Sprite
            };

            balloons.Add(balloonInstance);
        }
    }
    created = true;
}

    public void LoadContent()
    {  
        
        _spriteBatch = new SpriteBatch(_graphicsDevice);

        // balloonTexture = _content.Load<Texture2D>("balloon");
        balloon = new Balloon(Vector2.Zero, 0);
        balloon.Sprite = new Sprite();
        balloon.Sprite.Texture = _content.Load<Texture2D>("images/balloon");


        bounce = _content.Load<SoundEffect>("sounds/bounce");
        pop = _content.Load<SoundEffect>("sounds/pop");
        
        _textFont = _content.Load<SpriteFont>("fonts/ScoreFont");
                foreach (var player in players)
        {
            player.Clown.Sprite = new Sprite();
            player.Clown.Sprite.Texture = _content.Load<Texture2D>("images/clown");

            

            player.Trampoline.Sprite = new Sprite();
            player.Trampoline.Sprite.Texture = _content.Load<Texture2D>("images/Trampoline");

                player.Trampoline.Position = new Vector2(
                    player.Trampoline.Position.X,
                    _graphicsDevice.Viewport.Height - player.Trampoline.Height
                );
        }
        
        CreateBalloons(10);
        
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
            
            var trampoline = player.Trampoline;

            // Clamp do trampolim
            trampoline.Position.X = MathHelper.Clamp(
                trampoline.Position.X,
                0,
                _graphicsDevice.Viewport.Width - trampoline.Width
            );
            
            var clown = player.Clown;
            if (clown == null)
                continue;


            // Movimento do clown
            clown.Position += clown.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Gravidade
            clown.Velocity.Y += 16f;

            // Limites da tela
            int maxX = _graphicsDevice.Viewport.Width - clown.Width;
            int maxY = _graphicsDevice.Viewport.Height - clown.Height;

            if (clown.Position.Y > (maxY + clown.Height))
            {
                player.Score.AddPoints(-1200);
                player.Lives--;


                RespawnTimer.Wait(gameTime, () =>
                {
                    clown.Position = trampoline.Position + new Vector2(trampoline.Width / 2, -clown.Height);

                    clown.Velocity = Vector2.Zero;
                });
            

                if (player.Lives <= 0)
                {
                    player.IsAlive = false;
                }



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




            // Colisão
            if (Collision.Intersects(trampoline, clown)){
                int angle = trampoline.Bounds.Center.X - clown.Bounds.Center.X + 90;

                clown.Velocity.X = 980f * (float)Math.Cos(MathHelper.ToRadians(angle));
                clown.Velocity.Y = 980f * (float)Math.Sin(MathHelper.ToRadians(angle)) * -1;

                bounce.Play();
            }
            for (int i = balloons.Count - 1; i >= 0; i--)
            {
                Balloon balloon = balloons[i];

                if (Collision.Intersects(clown, balloon))
                {
                    player.Score.AddPoints(balloon.Value);
                    balloons.RemoveAt(i);
                    pop.Play();
                }
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


            foreach (var balloon in balloons)
            {
                _spriteBatch.Draw(
                    balloon.Sprite.Texture,
                    new Rectangle((int)balloon.Position.X, (int)balloon.Position.Y, 64, 64),
                    Color.White
                );
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