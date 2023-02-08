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
        }),
        
        new MenuEntry("Exit", "See you next time.", game =>
        {
            game.Exit();
        })
    };

    private int _menuIdx = 0;
    private MenuEntry CurrentMenuItem => Entries[Mod(_menuIdx, Entries.Length)];
    private int CurrentMenuIdx => Mod(_menuIdx, Entries.Length);
    private FentrisGame _game;
    private DynamicSpriteFont _menuItemFont;
    private DynamicSpriteFont _taglineFont;
    private SpriteBatch _spriteBatch;
    
    private int BaseFontSize
    {
        get
        {
            var smallerDim = Math.Min(_game.W, _game.H);
            var fontSize = (smallerDim / 20 / 4) * 4; // lose precision on purpose to only load fonts in increments of 4
            return Math.Max(fontSize, 12);
        }
    }
    
    public StartMenuScreen(FentrisGame game) : base(game)
    {
        this._game = game;
    }

    public override void LoadContent()
    {
        ReloadFont();
        _spriteBatch = new SpriteBatch(_game.GraphicsDevice);

        Game.Window.ClientSizeChanged += (sender, args) =>
        {
            ReloadFont();
        };
    }

    private void ReloadFont()
    {
        var fontSize = BaseFontSize;
        Console.WriteLine(fontSize);
        _menuItemFont = _game.DefaultFonts.GetFont(fontSize);
        _taglineFont = _game.DefaultFonts.GetFont(fontSize / 1.4f);
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
        var y = _game.H * 0.4f;
        for(int i = 0; i < Entries.Length; i++)
        {
            var item = Entries[i];
            var measure = _menuItemFont.MeasureString(item.Title);
            var x = _game.W / 2.0f - measure.X / 2.0f;
            var color = i == CurrentMenuIdx ? Color.Yellow : Color.White;
            _spriteBatch.DrawString(_menuItemFont, item.Title, new Vector2(x, y), color);
            y += measure.Y * 2;
        }

        var taglineMeasure = _menuItemFont.MeasureString(CurrentMenuItem.Tagline);
        _spriteBatch.DrawString(_menuItemFont, CurrentMenuItem.Tagline,
            new Vector2(20, _game.H - taglineMeasure.Y - 20), Color.White);
        

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