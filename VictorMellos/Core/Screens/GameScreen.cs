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
    private SpriteFont _textFont;
    private SpriteBatch _spriteBatch;

    private GraphicsDevice _graphicsDevice;
    
    private List<Player> players;
    private ContentManager _content;

    private Rectangle spawnArea;

    private Random random = new Random();
    private List<Rectangle> balloonAreas = new();

    

    private Balloon balloon;

    private List<Balloon> balloons = new();

    public GameScreen(GraphicsDevice graphicsDevice, ContentManager content)//, List<Player> players)
    {
        _graphicsDevice = graphicsDevice;
        _content = content;
        // this.players = players;
    }
    
    private Vector2 GetBalloonSpawnPosition()
    {
        int balloonWidth = 64;
        int balloonHeight = 64;


        while (true)
        {
            int x = random.Next(
                spawnArea.Left,
                spawnArea.Right - balloonWidth
            );

            int y = random.Next(
                spawnArea.Top,
                spawnArea.Bottom - balloonHeight
            );

            Rectangle newArea = new Rectangle(x, y, balloonWidth, balloonHeight);

            bool occupied = false;

            foreach (Rectangle area in balloonAreas)
            {
                if (area.Intersects(newArea))
                {
                    occupied = true;
                    break;
                }
            }

            if (!occupied)
            {
                balloonAreas.Add(newArea);
                return new Vector2(x, y);
            }
        }
    }

    public void CreateBalloons(int quantity)
    {
        int marginX = 80;
        int topMargin = 70;
        int bottomMargin = 180;

        spawnArea = new Rectangle(
            marginX,
            topMargin,
            _graphicsDevice.Viewport.Width - marginX * 2,
            _graphicsDevice.Viewport.Height - topMargin - bottomMargin
        );

        balloons.Clear();

        for (int i = 0; i < quantity; i++)
        {
            Vector2 position = GetBalloonSpawnPosition();

            Balloon balloonInstance = new Balloon(position, random.Next(200, 1001))
            {
                Sprite = balloon.Sprite
            };

            balloons.Add(balloonInstance);
        }
    }
    public void LoadContent()
    {  
        
        _spriteBatch = new SpriteBatch(_graphicsDevice);

        // balloonTexture = _content.Load<Texture2D>("balloon");
        balloon = new Balloon(Vector2.Zero, 0);
        balloon.Sprite = new Sprite();
        balloon.Sprite.Texture = _content.Load<Texture2D>("balloon");


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
            var clown = player.Clown;
            if (clown == null)
                continue;

            var trampoline = player.Trampoline;

            // Clamp do trampolim
            trampoline.Position.X = MathHelper.Clamp(
                trampoline.Position.X,
                0,
                _graphicsDevice.Viewport.Width - trampoline.Width
            );

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
                    player.Score.AddPoints(200); //trocar para balloon.value depois
                    balloons.RemoveAt(i);
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
                _spriteBatch.Draw(balloon.Sprite.Texture, balloon.Position, Color.White);
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