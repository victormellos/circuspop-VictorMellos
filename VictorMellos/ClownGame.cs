using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


namespace VictorMellos;

public class ClownGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private int maxX;
    private int maxY;
    private Sprite clown;

    private Sprite trampoline;

    private SoundEffect bounce;

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

        clown = new Sprite(new Vector2(100f, 120f), Vector2.Zero);

        trampoline = new Sprite(new Vector2(100f, 120f), Vector2.Zero);


        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        bounce = Content.Load<SoundEffect>("bounce");

        clown.Texture = Content.Load<Texture2D>("clown");

        trampoline.Texture = Content.Load<Texture2D>("Trampoline");

        trampoline.Position = new Vector2(clown.Position.X - clown.Width, GraphicsDevice.Viewport.Height - trampoline.Height);

        maxX = GraphicsDevice.Viewport.Width - clown.Width;
        maxY = GraphicsDevice.Viewport.Height - clown.Height;   

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        KeyboardState keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Left)) trampoline.Position.X -= 10f;
        if (keyboardState.IsKeyDown(Keys.Right)) trampoline.Position.X += 10f;

        GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
        trampoline.Position.X += gamePadState.ThumbSticks.Left.X * 15f;



        Rectangle trampolineRect = new Rectangle((int)trampoline.Position.X, (int)trampoline.Position.Y, 
        trampoline.Width, trampoline.Height);

        Rectangle clownRect = new Rectangle((int)clown.Position.X, (int)clown.Position.Y, clown.Width, clown.Height);


        if (trampolineRect.Intersects(clownRect))
        {
            int angle = trampolineRect.Center.X - clownRect.Center.X + 90;

            clown.Velocity.X = 980f * (float)Math.Cos(MathHelper.ToRadians(angle));
            clown.Velocity.Y = 980f * (float)Math.Sin(MathHelper.ToRadians(angle)) * -1; // upward
            
            bounce.Play();
        }



        trampoline.Position.X = MathHelper.Clamp(trampoline.Position.X, 0, GraphicsDevice.Viewport.Width - trampoline.Texture.Width);

        //move clown by velocity (frame-rate independent)

        clown.Position += clown.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        clown.Velocity.Y += 16f;

        if (clown.Position.Y > maxY)
        {
            clown.Velocity.Y *= -1;  // reverse velocity to bounce back up
            clown.Position.Y = maxY; // clamp so it doesn't escape
        }

        if (clown.Position.X > maxX)
        {
            // We've hit the right side, reverse to create a bounce
            clown.Velocity.X *= -1;
            clown.Position.X = maxX;
        }
        else if (clown.Position.X < 0)
        {
            // We've hit the left side, reverse to create a bounce
            clown.Velocity.X *= -1;
            clown.Position.X = 0;
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        _spriteBatch.Draw(clown.Texture, clown.Position, Color.White);
        _spriteBatch.Draw(trampoline.Texture, trampoline.Position, Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}