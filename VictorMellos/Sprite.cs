using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Sprite
{
    public Texture2D Texture { get; set; }
    public Vector2 Position;
    public Vector2 Velocity;

    public int Width => Texture.Width;
    public int Height => Texture.Height;

    public Sprite(Vector2 initialPosition, Vector2 initialVelocity)
    {
        Position = initialPosition;
        Velocity = initialVelocity;
    }
}