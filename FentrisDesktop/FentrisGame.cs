﻿using System;
using FentrisDesktop.Board;
using FentrisDesktop.Gamemode;
using FontStashSharp;
using Microsoft.Xna.Framework;
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
        _screenManager = new ScreenManager();
        Components.Add(_screenManager);

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
        // _screenManager.LoadScreen(new GamemodeRenderer(this, new Gamemode.ApocalypseGamemode()), new FadeTransition(GraphicsDevice, Color.Black));
        _screenManager.LoadScreen(new StartMenuScreen(this), new FadeTransition(GraphicsDevice, Color.Black));
        // LoadMenu();
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
        _screenManager.LoadScreen(new GamemodeRenderer(this, mode), new FadeTransition(GraphicsDevice, Color.Black));
    }

    public void LoadMenu()
    {
        _screenManager.LoadScreen(new StartMenuScreen(this), new ExpandTransition(GraphicsDevice, Color.Gray, 0.2F));
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
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}