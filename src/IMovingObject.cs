using Microsoft.Xna.Framework;

namespace CircusPop;

interface IMovingObject
{
    Vector2 GetPosition();
    Vector2 GetVelocity();
}
