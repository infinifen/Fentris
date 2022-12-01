using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FentrisDesktop;

public class InputHandler
{
    private KeyboardState PrevKeyboardState;
    private GamePadState PrevGamePadState;
    public DirectionInput CurrentDirection = DirectionInput.Neutral;

    public InputHandler()
    {
        CycleInputStates();
    }

    public void CycleInputStates()
    {
        PrevKeyboardState = Keyboard.GetState();
        PrevGamePadState = GamePad.GetState(PlayerIndex.One);
    }

    public bool KeyboardIsJustDown(Keys k)
    {
        return Keyboard.GetState().IsKeyDown(k) && PrevKeyboardState.IsKeyUp(k);
    }

    public bool KeyboardIsDown(Keys k)
    {
        return Keyboard.GetState().IsKeyDown(k);
    }
    
    public bool KeyboardWasDown(Keys k)
    {
        return PrevKeyboardState.IsKeyDown(k);
    }

    public bool KeyboardIsJustUp(Keys k)
    {
        return Keyboard.GetState().IsKeyUp(k) && PrevKeyboardState.IsKeyDown(k);
    }

    public bool KeyboardIsHeld(Keys k)
    {
        return Keyboard.GetState().IsKeyDown(k) && PrevKeyboardState.IsKeyDown(k);
    }

    public GamemodeInputs GetInputs()
    {
        var result = new GamemodeInputs();
        
        // this logic can probably be much cleaner but i'm not doing that right now

        if (KeyboardIsJustDown(Keys.Left))
        {
            CurrentDirection = DirectionInput.Left; // latest key overrides
        }

        if (KeyboardIsJustDown(Keys.Right))
        {
            CurrentDirection = DirectionInput.Right; // latest key overrides
        }

        if (KeyboardIsDown(Keys.Left) && !KeyboardIsDown(Keys.Right))
        {
            CurrentDirection = DirectionInput.Left; // both were held down and right was released
        }
        
        if (KeyboardIsDown(Keys.Right) && !KeyboardIsDown(Keys.Left))
        {
            CurrentDirection = DirectionInput.Right; // both were held down and left was released
        }

        if (!(KeyboardIsDown(Keys.Left) || KeyboardIsDown(Keys.Right)))
        {
            CurrentDirection = DirectionInput.Neutral; // neither is pressed, so go neutral
        }
        
        result.SonicDrop = KeyboardIsJustDown(Keys.Space);
        result.SoftDrop = KeyboardIsJustDown(Keys.Down);
        result.RotateCw = KeyboardIsJustDown(Keys.Up) || KeyboardIsJustDown(Keys.X);
        result.RotateCcw = KeyboardIsJustDown(Keys.Z);
        result.IrsCw = KeyboardIsDown(Keys.Up) || KeyboardIsDown(Keys.X);
        result.IrsCcw = KeyboardIsDown(Keys.Z);

        return result;
    }
}