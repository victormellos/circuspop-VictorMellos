using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VictorMellos
{
    public class Character
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Sprite Sprite;

        public int Width => Sprite.Texture.Width;
        public int Height => Sprite.Texture.Height;

        public Character(Vector2 initialPosition, Vector2 initialVelocity)
        {
            Position = initialPosition;
            Velocity = initialVelocity;
        }
        public void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Sprite != null)
            {
                spriteBatch.Draw(Sprite.Texture, Position, Color.White);
            }
        }
    }
}