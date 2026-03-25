using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;


namespace VictorMellos;

public class ClownGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private int maxX;
    private int maxY;
    private Character clown;

    private Character trampoline;

    private List<Player> players;

    private SoundEffect bounce;
    
    //private SpriteFont _textFont;



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
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        bounce = Content.Load<SoundEffect>("bounce");

        foreach (var player in players)
        {
            player.Clown.Sprite = new Sprite();
            player.Clown.Sprite.Texture = Content.Load<Texture2D>("clown");

            player.Trampoline.Sprite = new Sprite();
            player.Trampoline.Sprite.Texture = Content.Load<Texture2D>("Trampoline");

                player.Trampoline.Position = new Vector2(
                    player.Trampoline.Position.X,
                    GraphicsDevice.Viewport.Height - player.Trampoline.Height
                );
        }

        // trampoline.Position = new Vector2(clown.Position.X - clown.Width, GraphicsDevice.Viewport.Height - trampoline.Height);

        // maxX = GraphicsDevice.Viewport.Width - clown.Width;
        // maxY = GraphicsDevice.Viewport.Height - clown.Height;   

    }

    protected override void Update(GameTime gameTime)
    {
        foreach (var player in players)
        {
            player.HandleInput();
        }
        
       foreach (var player in players)
        {
            var clown = player.Clown;
            var trampoline = player.Trampoline;

            // 1. Clamp do trampolim
            trampoline.Position.X = MathHelper.Clamp(
                trampoline.Position.X,
                0,
                GraphicsDevice.Viewport.Width - trampoline.Width
            );

            // 2. Movimento do clown
            clown.Position += clown.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // 3. Gravidade
            clown.Velocity.Y += 16f;

            // 4. Limites da tela
            int maxX = GraphicsDevice.Viewport.Width - clown.Width;
            int maxY = GraphicsDevice.Viewport.Height - clown.Height;

            if (clown.Position.Y > maxY)
            {
                clown.Velocity.Y *= -1;
                clown.Position.Y = maxY;
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

            // 5. Agora sim cria os rectangles atualizados
            Rectangle trampolineRect = new Rectangle(
                (int)trampoline.Position.X,
                (int)trampoline.Position.Y,
                trampoline.Width,
                trampoline.Height);

            Rectangle clownRect = new Rectangle(
                (int)clown.Position.X,
                (int)clown.Position.Y,
                clown.Width,
                clown.Height);

            // 6. Colisão
            if (trampolineRect.Intersects(clownRect))
            {
                int angle = trampolineRect.Center.X - clownRect.Center.X + 90;

                clown.Velocity.X = 980f * (float)Math.Cos(MathHelper.ToRadians(angle));
                clown.Velocity.Y = 980f * (float)Math.Sin(MathHelper.ToRadians(angle)) * -1;

                bounce.Play();
            }
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        
        foreach (var player in players)
        {
            _spriteBatch.Draw(player.Clown.Sprite.Texture, player.Clown.Position, Color.White);
            _spriteBatch.Draw(player.Trampoline.Sprite.Texture, player.Trampoline.Position, Color.White);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}