using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace VictorMellos
{
public class Player
{
    public Character Trampoline { get; private set; }
    public Character Clown { get; private set; }

    public Score Score;

    public string Name;
    public int Lives;
    private bool isAlive;
    public bool IsAlive
    {
        get { return isAlive; }
        set
        {
            isAlive = value;

            if (!isAlive)
            {
                OnDeath();
            }
        }
    }
    private Keys leftKey;
    private Keys rightKey;

    public Player(Character trampoline, Character clown, Keys left, Keys right, string name = "Jogador", int lives = 5)
    {
        Trampoline = trampoline;
        Clown = clown;

        Score = new Score();

        Name = name;
        Lives = lives;

        leftKey = left;
        rightKey = right;
    }

    public void HandleInput()
    {
        KeyboardState keyboard = Keyboard.GetState();

        if (keyboard.IsKeyDown(leftKey))
            Trampoline.Position.X -= 10f;

        if (keyboard.IsKeyDown(rightKey))
            Trampoline.Position.X += 10f;

        GamePadState padState = GamePad.GetState(PlayerIndex.One);
        Trampoline.Position.X += padState.ThumbSticks.Left.X * 15f;

        //      DEBUG  ------- REMOVER DEPOIS

        if (keyboard.IsKeyDown(Keys.I))
        {
                Lives = 999;
        }

    }
        private void OnDeath()
        {  
            Clown = null;
            Score.ResetCombo();
        }
}
}