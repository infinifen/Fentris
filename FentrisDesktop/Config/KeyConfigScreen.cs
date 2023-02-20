using System;
using System.Collections.Generic;
using System.Linq;
using FontStashSharp;
using FontStashSharp.RichText;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.Screens;

namespace FentrisDesktop.Config;

public class KeyConfigScreen : Screen
{
    private readonly FentrisGame _game;
    public DynamicSpriteFont LargeFont => _game.LargeFont;
    public DynamicSpriteFont MediumFont => _game.MediumFont;
    public DynamicSpriteFont SmallFont => _game.SmallFont;
    private readonly KeyboardListener _kbListener = new KeyboardListener();
    private List<Keys> keys;
    private string[] keyNames = new[]
    {
        "Start", "Back", "Menu Up", "Menu Down", "Left", "Right", "Sonic Drop",
        "Soft Drop", "Rotate Right", "Secondary Rotate Right", "Rotate Left", "Secondary Rotate Left"
    };
    private bool Finished => keys.Count >= KeyConfig.KeyCount;
    private bool firstInputIgnored = false;

    private EventHandler<KeyboardEventArgs> OnKeyPressed;
    private SpriteBatch _spriteBatch;


    public KeyConfigScreen(FentrisGame game)
    {
        _game = game;
        keys = new List<Keys>(KeyConfig.KeyCount);
        OnKeyPressed = (sender, args) => { Advance(args.Key); };
        _kbListener.KeyPressed += OnKeyPressed;
    }

    public override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
    }

    public override void UnloadContent()
    {
        _spriteBatch.Dispose();
    }

    private void Advance(Keys key)
    {
        if (!firstInputIgnored)
        {
            firstInputIgnored = true;
            return;
        }
        if (Finished)
        {
            if (key != Keys.Escape)
            {
                _game.KeyBinds = new KeyConfig(keys[0], keys[1], keys[2], keys[3], keys[4],
                    keys[5], keys[6], keys[7], keys[8], keys[9],
                    keys[10], keys[11]);
                _game.LoadMenu();
            }
            else
            {
                _game.LoadKeyConfig();
            }
        }
        else
        {
            keys.Add(key);
        }
    }

    public override void Update(GameTime gameTime)
    {
        _kbListener.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();
        _spriteBatch.FillRectangle(0, 0, _game.W, _game.H, Color.Black);
        if (!Finished)
        {
            _spriteBatch.DrawString(LargeFont, $"press key for {keyNames[keys.Count]}", new Vector2(0, 0), Color.White);
        }
        else
        {
            var bindingStringArr = keyNames.Zip(keys).Select(x => $"{x.First}: {x.Second}").ToArray();
            var bindingString = String.Join("\n", bindingStringArr);
            bindingString += "\npress any key to accept binds, press escape to try again";
            var rtl = new RichTextLayout
            {
                Font = MediumFont,
                Text = bindingString,
            };
            rtl.Draw(_spriteBatch, new Vector2(0, 0), Color.White);
        }
        _spriteBatch.End();

    }

    public override void Dispose()
    {
        base.Dispose();
        _kbListener.KeyPressed -= OnKeyPressed;
        _spriteBatch.Dispose();
    }
}