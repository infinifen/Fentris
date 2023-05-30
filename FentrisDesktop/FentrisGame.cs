using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using FentrisDesktop.Board;
using FentrisDesktop.Config;
using FentrisDesktop.Gamemode;
using FentrisDesktop.Sound;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

namespace FentrisDesktop;

public class FentrisGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private readonly ScreenManager _screenManager;
    public FontSystem DefaultFonts;
    public DynamicSpriteFont LargeFont;
    public DynamicSpriteFont MediumFont;
    public DynamicSpriteFont SmallFont;
    public readonly Dictionary<SoundEffects, SoundEffect> Sfx = new();
    public ISoundEffectManager SfxManager;

    public KeyConfig KeyBinds
    {
        get => SaveData.Keybinds;
        set => SaveData.Keybinds = value;
    }

    public FentrisSaveData SaveData = FentrisSaveData.Default();

    public int W => _graphics.PreferredBackBufferWidth;
    public int H => _graphics.PreferredBackBufferHeight;
    
    public int BaseFontSize
    {
        get
        {
            var smallerDim = Math.Min(W, H);
            var fontSize = (smallerDim / 20 / 4) * 4; // lose precision on purpose to only load fonts in increments of 4
            return Math.Max(fontSize, 12);
        }
    }

    public FentrisGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        KeyBinds = KeyConfig.Default();
        
        _screenManager = new ScreenManager();
        Components.Add(_screenManager);
        
        Sfx.Add(SoundEffects.Drop, Content.Load<SoundEffect>("sfx/drop"));
        Sfx.Add(SoundEffects.Move, Content.Load<SoundEffect>("sfx/move"));
        Sfx.Add(SoundEffects.Unlock, Content.Load<SoundEffect>("sfx/unlock"));
        Sfx.Add(SoundEffects.LineClear, Content.Load<SoundEffect>("sfx/line"));
        Sfx.Add(SoundEffects.LineClear, Content.Load<SoundEffect>("sfx/clear"));
        SfxManager = new DictSfxManager(Sfx);

        Window.ClientSizeChanged += (sender, args) =>
        {
            _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            _graphics.ApplyChanges();
        };
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        
        //replace this with nci timing stuff later
        IsFixedTimeStep = true;  //Force the game to update at fixed time intervals
        TargetElapsedTime = TimeSpan.FromSeconds(1 / 60.0f);  //Set the time interval to 1/60th of a second
        Window.AllowUserResizing = true;

        base.Initialize();
        ReloadFonts();

        var loadNext = LoadSave();
        Console.WriteLine(SaveData);

        _screenManager.LoadScreen(loadNext, new FadeTransition(GraphicsDevice, Color.Black));
    }

    private Screen LoadSave()
    {
        try
        {
            var saveString = File.ReadAllText("Fentris.sav");
            var save = JsonSerializer.Deserialize<FentrisSaveData>(saveString);
            SaveData = save;
            return new StartMenuScreen(this);
        }
        catch
        {
            //todo: elaborate on this a bit more except of catching everything
            SaveData = FentrisSaveData.Default();
            return new KeyConfigScreen(this, false);
        }
    }

    public void WriteSave()
    {
        try
        {
            var serialized = JsonSerializer.Serialize(SaveData);
            File.WriteAllText("Fentris.sav", serialized);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Couldn't write save: {e.Message}");
        }
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        var fontStream = Content.OpenStream("3270-Regular.ttf");
        DefaultFonts = new FontSystem();
        DefaultFonts.AddFont(fontStream);
        DefaultFonts.CurrentAtlasFull += (e, a) => DefaultFonts.Reset();
        
        Window.ClientSizeChanged += (sender, args) => { ReloadFonts(); };

        // TODO: use this.Content to load your game content here
    }

    public void LoadGamemode(Gamemode.Gamemode mode)
    {
        var rendererConstructor = GamemodeRegistry.GetRendererForId(mode.Id);
        _screenManager.LoadScreen(rendererConstructor(this, mode), new FadeTransition(GraphicsDevice, Color.Black));
    }

    public void LoadMenu()
    {
        _screenManager.LoadScreen(new StartMenuScreen(this), new ExpandTransition(GraphicsDevice, Color.Gray, 0.2F));
    }

    public void LoadHighscores()
    {
        _screenManager.LoadScreen(new RecordScreen(this), new ExpandTransition(GraphicsDevice, Color.Gray, 0.2F));
    }
    
    private void ReloadFonts()
    {
        var fontSize = BaseFontSize;
        Console.WriteLine(fontSize);
        LargeFont = DefaultFonts.GetFont(fontSize);
        MediumFont = DefaultFonts.GetFont(fontSize / 1.4f);
        SmallFont = DefaultFonts.GetFont(fontSize / 1.8f);
    }

    protected override void Update(GameTime gameTime)
    {
        // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
        //     Keyboard.GetState().IsKeyDown(Keys.Escape))
        //     Exit();

        // TODO: Add your update logic here
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }

    public void LoadKeyConfig()
    {
        _screenManager.LoadScreen(new KeyConfigScreen(this));
    }
}