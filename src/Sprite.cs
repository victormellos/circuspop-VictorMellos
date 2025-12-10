using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CircusPop;

public class Sprite : IMovingObject
{
    public Texture2D Texture;
    public Vector2 Position;
    public Vector2 Velocity;

    public Sprite(Vector2 initialPosition, Vector2 initialVelocity)
    {
        Position = initialPosition;
        Velocity = initialVelocity;
    }

    public Vector2 GetPosition()
    {
        return Position;
    }

    public Vector2 GetVelocity()
    {
        return Velocity;
    }
}
