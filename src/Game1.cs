using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CircusPop
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private readonly Sprite _clown = new(new Vector2(100.0f, 100.0f), Vector2.Zero);

        private bool _isGameStarted = false;

        private SpriteFont _scoreFont;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            this.Services.AddService(typeof(IMovingObject), _clown);
            this.Components.Add(new VelocityDisplay(this));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _clown.Texture = Content.Load<Texture2D>("clown");

            _scoreFont = Content.Load<SpriteFont>("File");
        }

        protected override void Update(GameTime gameTime)
        {
            // Exit the game when Back is pressed on GamePad or Escape is pressed on Keyboard
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // Skip updating if the game has not "started"
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _isGameStarted = true;
            }

            if (!_isGameStarted)
            {
                base.Update(gameTime);
                return;
            }

            //_clownPosition += _clownVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _clown.Position += _clown.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            int MaxX = GraphicsDevice.Viewport.Width - _clown.Texture.Width;
            int MinX = 0;
            int MaxY = GraphicsDevice.Viewport.Height - _clown.Texture.Height;
            int MinY = 0;

            if (_clown.Position.Y > MaxY)
            {
                // the _clown has hit the ground and would "lose"

                // for testing, force a "soft" 90 degree bounce
                float newY = 750 * (float)Math.Sin(MathHelper.ToRadians(90.0f));
                float newX = 750 * (float)Math.Cos(MathHelper.ToRadians(90.0f));
                _clown.Velocity.X = newX;
                _clown.Velocity.Y = newY;

                _clown.Velocity.Y *= -1;
                _clown.Position.Y = MaxY;
            }

            _clown.Velocity.Y += 16.0f;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_clown.Texture, _clown.Position, Color.White);

            if (!_isGameStarted)
            {
                _spriteBatch.DrawString(_scoreFont, "Press <Space> To Start", new Vector2(250.0f, 55.0f), Color.DarkBlue);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
