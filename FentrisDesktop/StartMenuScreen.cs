using System;
using FentrisDesktop.Gamemode;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Screens;
using static FentrisDesktop.FentrisHelper;


namespace FentrisDesktop;

public class StartMenuScreen : GameScreen
{
    public static readonly MenuEntry[] Entries = new[]
    {
        new MenuEntry("Normal mode", "The classic beginner experience", game =>
        {
            game.LoadGamemode(new Gamemode.Gamemode());
        }),
        
        new MenuEntry("Apocalypse", "Delay the descent into insanity.", game =>
        {
            game.LoadGamemode(new ApocalypseGamemode());
        })
    };

    private int _menuIdx = 0;
    private MenuEntry CurrentMenuItem => Entries[Mod(_menuIdx, Entries.Length)];
    private FentrisGame _game;
    private DynamicSpriteFont _menuItemFont;
    private SpriteBatch _spriteBatch;
    
    public StartMenuScreen(FentrisGame game) : base(game)
    {
        this._game = game;
    }

    public override void LoadContent()
    {
        _menuItemFont = _game.DefaultFonts.GetFont(24);
        _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
    }

    public override void UnloadContent()
    {
       _spriteBatch.Dispose();
    }

    public override void Update(GameTime gameTime)
    {
        var state = KeyboardExtended.GetState();

        if (state.WasKeyJustUp(Keys.Down))
        {
            _menuIdx++;
        }
        
        if (state.WasKeyJustUp(Keys.Up))
        {
            _menuIdx--;
        }
        
        if (state.WasKeyJustUp(Keys.Enter))
        {
            CurrentMenuItem.Action(_game);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();
        _spriteBatch.DrawString(_menuItemFont, CurrentMenuItem.Title, Vector2.Zero, Color.White);
        _spriteBatch.End();
    }
}

public struct MenuEntry
{
    public delegate void MenuAction(FentrisGame game); 
    public string Title;
    public string Tagline;
    public MenuAction Action;

    public MenuEntry(string title, string tagline, MenuAction action)
    {
        Title = title;
        Tagline = tagline;
        Action = action;
    }
}