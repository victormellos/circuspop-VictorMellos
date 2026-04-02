using Microsoft.Xna.Framework;

namespace VictorMellos
{
    public class Balloon : Character
    {
    
    public int Value;

    public Balloon(Vector2 initialPosition, int value)
    : base(initialPosition, Vector2.Zero)
        {
            Value = value;
            
        }


    
    }
}