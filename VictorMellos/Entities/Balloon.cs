using Microsoft.Xna.Framework;

namespace VictorMellos
{
    public class Balloon : Character
    {
    
    public int Value;
    public override int Width => 64;
    public override int Height => 64;


    public Balloon(Vector2 initialPosition, int value)
    : base(initialPosition, Vector2.Zero)
        {
            Value = value;
            
        }

    public new Rectangle Bounds => new Rectangle(
        (int)Position.X,
        (int)Position.Y,
        Width,
        Height
    );


    
    }
}