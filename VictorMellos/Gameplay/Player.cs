using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace VictorMellos
{
public class Player
{
    public Character Trampoline { get; private set; }
    public Character Clown { get; private set; }

    public Score Score;

    public string Name;

    private Keys leftKey;
    private Keys rightKey;

    public Player(Character trampoline, Character clown, Keys left, Keys right, string name = "Jogador")
    {
        Trampoline = trampoline;
        Clown = clown;

        Score = new Score();

        Name = name;

        leftKey = left;
        rightKey = right;
    }

    public void HandleInput()
    {
        var keyboard = Keyboard.GetState();

        if (keyboard.IsKeyDown(leftKey))
            Trampoline.Position.X -= 10f;

        if (keyboard.IsKeyDown(rightKey))
            Trampoline.Position.X += 10f;
    }
}
}