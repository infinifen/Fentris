using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FentrisDesktop.Gamemode;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tweening;
using static FentrisDesktop.FentrisHelper;


namespace FentrisDesktop;

public class StartMenuScreen : GameScreen
{
    public static readonly MenuEntry[] Entries = new[]
    {
        new MenuEntry("Beginner Marathon", "Clear 100 lines as fast as you can!",
            game => { game.LoadGamemode(new Gamemode.BeginnerMarathonGamemode()); },
            new Color(0.1f, 0.8f, 0.2f, 1f)),
        
        new MenuEntry("Normal Score Attack", "Keep your multiplier high and score as high as possible in this 400-level mode!",
            game => { game.LoadGamemode(new Gamemode.NormalGamemode()); },
            new Color(0.5f, 0.4f, 0.73f, 1f)),

        new MenuEntry("Apocalypse", "Delay the descent into insanity.",
            game => { game.LoadGamemode(new ApocalypseGamemode()); },
            new Color(0.82f, 0.23f, 0.1f, 1f)),
        
        new MenuEntry("Ferocity", "Say goodbye to the serene.",
            game => game.SaveData.Highscores.Apocalypse.Level > 333, 
            game => { game.LoadGamemode(new FerocityGamemode()); },
            new Color(0.2f, 0.2f, 0.2f, 1f)),
        
        new MenuEntry("Key bindings", "You can change your controls here.", game =>
        {
            game.LoadKeyConfig();
        }, Color.Azure),

        new MenuEntry("Exit", "See you next time.", game => { game.Exit(); },
            new Color(1f, 1f, 1f, 1f))
    };
    
    private readonly MenuEntry[] _unlockedEntries;
    private int _menuIdx = 0;
    private MenuEntry CurrentMenuItem => _unlockedEntries[Mod(_menuIdx, Entries.Length)];
    private int CurrentMenuIdx => Mod(_menuIdx, Entries.Length);
    private FentrisGame _game;
    private SpriteBatch _spriteBatch;
    private Texture2D _keysBackground;
    private Effect _backgroundShader;

    public StartMenuScreen(FentrisGame game) : base(game)
    {
        this._game = game;
        _unlockedEntries = Entries.Where(me => me.IsUnlocked(game)).ToArray();
    }

    public override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
        _keysBackground = Content.Load<Texture2D>("tpiece_4k");
        _backgroundShader = Content.Load<Effect>("MainMenuBg");
    }


    public override void UnloadContent()
    {
        Console.WriteLine("menu_unloadContent");
        _spriteBatch.Dispose();
    }

    public override void Update(GameTime gameTime)
    {
        var state = KeyboardExtended.GetState();
        if (state.WasKeyJustUp(_game.KeyBinds.MenuDown))
        {
            _menuIdx++;
        }

        if (state.WasKeyJustUp(_game.KeyBinds.MenuUp))
        {
            _menuIdx--;
        }

        if (state.WasKeyJustUp(_game.KeyBinds.Start))
        {
            CurrentMenuItem.Action(_game);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        _backgroundShader.Parameters["tint"].SetValue(CurrentMenuItem.Tint.ToVector4());
        _backgroundShader.Parameters["time"].SetValue((float) gameTime.TotalGameTime.TotalSeconds);
        _backgroundShader.Techniques["SpriteDrawing"].Passes[0].Apply();
        var (bgPos, bgScale) = ComputeBackgroundScale(_keysBackground);
        _spriteBatch.Draw(_keysBackground, bgPos, scale: bgScale, sourceRectangle: null, color: Color.White,
            origin: Vector2.Zero, effects: SpriteEffects.None, rotation: 0f, layerDepth: 0f);
        _spriteBatch.End();

        _spriteBatch.Begin();
        var y = _game.H * 0.4f;
        for (int i = 0; i < _unlockedEntries.Length; i++)
        {
            var item = _unlockedEntries[i];
            var measure = _game.LargeFont.MeasureString(item.Title);
            var x = _game.W / 2.0f - measure.X / 2.0f;
            var color = i == CurrentMenuIdx ? Color.Yellow : Color.White;
            _spriteBatch.DrawString(_game.LargeFont, item.Title, new Vector2(x, y), color);
            y += measure.Y * 2;
        }

        var taglineMeasure = _game.MediumFont.MeasureString(CurrentMenuItem.Tagline);
        _spriteBatch.DrawString(_game.MediumFont, CurrentMenuItem.Tagline,
            new Vector2(20, _game.H - taglineMeasure.Y - 20), Color.White);


        _spriteBatch.End();
    }

    private (Vector2, float) ComputeBackgroundScale(Texture2D tex)
    {
        var xB = tex.Width;
        var yB = tex.Height;

        var xR = _game.W / (float)xB;
        var yR = _game.H / (float)yB;
        var scale = Math.Max(xR, yR);

        var excessX = xB * scale - _game.W;
        var excessY = yB * scale - _game.H;

        return (new Vector2(-excessX / 2, -excessY / 2), scale);
    }
}

public struct MenuEntry
{
    public delegate void MenuAction(FentrisGame game);

    public string Title;
    public string Tagline;
    public Func<FentrisGame, bool> IsUnlocked;
    public MenuAction Action;
    public Color Tint;

    public MenuEntry(string title, string tagline, MenuAction action, Color tint)
    {
        Title = title;
        Tagline = tagline;
        Action = action;
        Tint = tint;
        IsUnlocked = (_) => true;
    }

    public MenuEntry(string title, string tagline, Func<FentrisGame, bool> isUnlocked, MenuAction action, Color tint)
    {
        Title = title;
        Tagline = tagline;
        IsUnlocked = isUnlocked;
        Action = action;
        Tint = tint;
    }
}