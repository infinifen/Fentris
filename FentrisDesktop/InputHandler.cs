using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FentrisDesktop;

public class InputHandler
{
    private KeyboardState _prevKeyboardState;
    private GamePadState _prevGamePadState;
    private DirectionInput _prevDirection = DirectionInput.Neutral;
    private DirectionInput _currentDirection = DirectionInput.Neutral;

    public InputHandler()
    {
        CycleInputStates();
    }

    public void CycleInputStates()
    {
        _prevKeyboardState = Keyboard.GetState();
        _prevGamePadState = GamePad.GetState(PlayerIndex.One);
        _prevDirection = _currentDirection;
    }

    public bool KeyboardIsJustDown(Keys k)
    {
        return Keyboard.GetState().IsKeyDown(k) && _prevKeyboardState.IsKeyUp(k);
    }

    public bool KeyboardIsDown(Keys k)
    {
        return Keyboard.GetState().IsKeyDown(k);
    }
    
    public bool KeyboardWasDown(Keys k)
    {
        return _prevKeyboardState.IsKeyDown(k);
    }

    public bool KeyboardIsJustUp(Keys k)
    {
        return Keyboard.GetState().IsKeyUp(k) && _prevKeyboardState.IsKeyDown(k);
    }

    public bool KeyboardIsHeld(Keys k)
    {
        return Keyboard.GetState().IsKeyDown(k) && _prevKeyboardState.IsKeyDown(k);
    }

    public GamemodeInputs GetInputs()
    {
        var result = new GamemodeInputs();
        
        // this logic can probably be much cleaner but i'm not doing that right now

        if (KeyboardIsJustDown(Keys.Left))
        {
            _currentDirection = DirectionInput.Left; // latest key overrides
        }

        if (KeyboardIsJustDown(Keys.Right))
        {
            _currentDirection = DirectionInput.Right; // latest key overrides
        }

        if (KeyboardIsDown(Keys.Left) && !KeyboardIsDown(Keys.Right))
        {
            _currentDirection = DirectionInput.Left; // both were held down and right was released
        }
        
        if (KeyboardIsDown(Keys.Right) && !KeyboardIsDown(Keys.Left))
        {
            _currentDirection = DirectionInput.Right; // both were held down and left was released
        }

        if (!(KeyboardIsDown(Keys.Left) || KeyboardIsDown(Keys.Right)))
        {
            _currentDirection = DirectionInput.Neutral; // neither is pressed, so go neutral
        }
        
        result.SonicDrop = KeyboardIsDown(Keys.Space);
        result.SoftDrop = KeyboardIsDown(Keys.Down);
        result.RotateCw = KeyboardIsJustDown(Keys.Up) || KeyboardIsJustDown(Keys.X);
        result.RotateCcw = KeyboardIsJustDown(Keys.Z);
        result.IrsCw = KeyboardIsDown(Keys.Up) || KeyboardIsDown(Keys.X);
        result.IrsCcw = KeyboardIsDown(Keys.Z);
        result.Direction = _currentDirection;
        result.PreviousDirection = _prevDirection;

        return result;
    }
}