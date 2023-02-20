using FentrisDesktop.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FentrisDesktop.Gamemode;

public class InputHandler
{
    private KeyboardState _prevKeyboardState;
    private GamePadState _prevGamePadState;
    private DirectionInput _prevDirection = DirectionInput.Neutral;
    private DirectionInput _currentDirection = DirectionInput.Neutral;
    private KeyConfig _binds;

    public InputHandler(KeyConfig binds)
    {
        _binds = binds;
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

        if (KeyboardIsJustDown(_binds.Left))
        {
            _currentDirection = DirectionInput.Left; // latest key overrides
        }

        if (KeyboardIsJustDown(_binds.Right))
        {
            _currentDirection = DirectionInput.Right; // latest key overrides
        }

        if (KeyboardIsDown(_binds.Left) && !KeyboardIsDown(_binds.Right))
        {
            _currentDirection = DirectionInput.Left; // both were held down and right was released
        }
        
        if (KeyboardIsDown(_binds.Right) && !KeyboardIsDown(_binds.Left))
        {
            _currentDirection = DirectionInput.Right; // both were held down and left was released
        }

        if (!(KeyboardIsDown(_binds.Left) || KeyboardIsDown(_binds.Right)))
        {
            _currentDirection = DirectionInput.Neutral; // neither is pressed, so go neutral
        }
        
        result.SonicDrop = KeyboardIsDown(_binds.Sonic);
        result.SoftDrop = KeyboardIsDown(_binds.Soft);
        result.RotateCw = KeyboardIsJustDown(_binds.RotateCw) || KeyboardIsJustDown(_binds.SecondaryRotateCw);
        result.RotateCcw = KeyboardIsJustDown(_binds.RotateCcw) || KeyboardIsJustDown(_binds.SecondaryRotateCcw);
        result.IrsCw = KeyboardIsDown(_binds.RotateCw) || KeyboardIsDown(_binds.SecondaryRotateCw);
        result.IrsCcw = KeyboardIsDown(_binds.RotateCcw) || KeyboardIsDown(_binds.SecondaryRotateCcw);
        result.Direction = _currentDirection;
        result.PreviousDirection = _prevDirection;

        return result;
    }
}