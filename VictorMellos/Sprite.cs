using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Sprite
{
    public Texture2D Texture;

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        
        spriteBatch.Draw(Texture, position, Color.White);
    }
}