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
    private KeyboardState keyboard;
    private GamePadState gamepad;

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
    private PlayerIndex playerIndex;

    public Player(Character trampoline, Character clown, Keys left, Keys right, PlayerIndex playerIndex, string name = "Jogador", int lives = 5)
    {
        Trampoline = trampoline;
        Clown = clown;

        Score = new Score();

        Name = name;
        Lives = lives;

        leftKey = left;
        rightKey = right;

        this.playerIndex = playerIndex;
    }
    public void HandleInputGameplay()
    {


        if (keyboard.IsKeyDown(leftKey))
            Trampoline.Position.X -= 10f;

        if (keyboard.IsKeyDown(rightKey))
            Trampoline.Position.X += 10f;

        
        Trampoline.Position.X += gamepad.ThumbSticks.Left.X * 15f;

        //      DEBUG  ------- REMOVER DEPOIS

        if (keyboard.IsKeyDown(Keys.I))
        {
                Lives = 999;
        }

    }
    public void UpdateInput()
    {
        keyboard = Keyboard.GetState();
        gamepad = GamePad.GetState(playerIndex);
    }
    void HandleInputMainMenu()
    {
        
    }
    public void HandleInput(GameState gameState)
    {

        switch (gameState)
        {
            case GameState.Playing:
                HandleInputGameplay();
                break;

            case GameState.MainMenu:
                HandleInputMainMenu();
                break;
        }
    }
        private void OnDeath()
        {  
            Clown = null;
            Score.ResetCombo();
        }
}
}