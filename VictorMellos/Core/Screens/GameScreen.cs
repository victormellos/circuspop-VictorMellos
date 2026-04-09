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
    private SoundEffect pop;

    private SpriteFont _textFont;
    private SpriteBatch _spriteBatch;

    private GraphicsDevice _graphicsDevice;
    
    private List<Player> players;
    private ContentManager _content;
    

    private Level level = new Level(1);
    private int level_Number;

    private Random random = new Random();

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
        balloons.Clear();

        int spacingX = 4;
        int spacingY = 4;

        int rows = quantity;

        int balloonSize = (_graphicsDevice.Viewport.Height - ((rows - 1) * spacingY)) / rows;

        balloonSize = Math.Clamp(balloonSize, 16, 48);

        int columns = (_graphicsDevice.Viewport.Width + spacingX) / (balloonSize + spacingX);

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Balloon balloonInstance = new Balloon(
                    new Vector2(
                        x * (balloonSize + spacingX),
                        y * (balloonSize + spacingY)
                    ),
                    random.Next(200, 1001)
                )
                {
                    Sprite = balloon.Sprite,
                    Width = balloonSize,
                    Height = balloonSize
                };

                balloons.Add(balloonInstance);
            }
        }
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
            player.Clown.Width = player.Clown.Sprite.Texture.Width;
            player.Clown.Height = player.Clown.Sprite.Texture.Height;

            

            player.Trampoline.Sprite = new Sprite();
            player.Trampoline.Sprite.Texture = _content.Load<Texture2D>("images/Trampoline");
            player.Trampoline.Width = player.Trampoline.Sprite.Texture.Width;
            player.Trampoline.Height = player.Trampoline.Sprite.Texture.Height;


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
        if (balloons.Count == 0)
        {
            level_Number++;
            level = new Level(level_Number);
            CreateBalloons(level.Diff);
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

        
            
            float baseSpeed = 980f;
            float comboMultiplier = 1f + (player.Score.Combo * 0.0008f);

            //comboMultiplier = Math.Min(1.5f, comboMultiplier);
            
            float speed = baseSpeed * comboMultiplier;

            // Gravidade
            clown.Velocity.Y += 16f * comboMultiplier;

            int maxX = _graphicsDevice.Viewport.Width - clown.Width;
            int maxY = _graphicsDevice.Viewport.Height - clown.Height;

            if (clown.Position.Y > (maxY + clown.Height))
            {
                player.Score.AddPoints(-1200);
                player.Lives--;
                player.Score.ResetCombo();


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
            else if (clown.Position.Y < -10)
            {
                clown.Velocity.Y *= -1;
                clown.Position.Y = -10;
            }

            // Colisão
            if (Collision.Intersects(trampoline, clown)){
                int angle = trampoline.Bounds.Center.X - clown.Bounds.Center.X + 90;

                clown.Velocity.X = speed * (float)Math.Cos(MathHelper.ToRadians(angle));
                clown.Velocity.Y = speed * (float)Math.Sin(MathHelper.ToRadians(angle)) * -1;

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

            string scoreText = $"{player.Name} : {player.Score.Points}\n{(player.Lives <=0 ? "Morto!" : $"Vidas: {player.Lives}")}\n{(player.Score.Combo <=0 ? null : $"Combo : {player.Score.Combo}")}";
            
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