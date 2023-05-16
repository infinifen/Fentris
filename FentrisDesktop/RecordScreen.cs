using System;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using static FentrisDesktop.FentrisHelper;

namespace FentrisDesktop;

public class RecordScreen : GameScreen
{
    public new FentrisGame Game;
    private SpriteBatch _batch;
    private Texture2D _bg;

    public RecordScreen(Game game) : base(game)
    {
        Game = (FentrisGame)game;
        _batch = new SpriteBatch(GraphicsDevice);
    }

    public override void LoadContent()
    {
        base.LoadContent();
        _bg = Content.Load<Texture2D>("galaxy-invert-dark-4k");
    }

    public override void UnloadContent()
    {
        base.UnloadContent();
        _batch.Dispose();
    }

    public override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Game.SaveData.Keybinds.Back) || Keyboard.GetState().IsKeyDown(Game.SaveData.Keybinds.Start))
        {
            Game.LoadMenu();
        }
    }

    public override void Draw(GameTime gameTime)
    {
        _batch.Begin();
        var (bgPos, bgScale) = ComputeBackgroundScale(_bg);
        _batch.Draw(_bg, bgPos, scale: bgScale, sourceRectangle: null, color: Color.White,
            origin: Vector2.Zero, effects: SpriteEffects.None, rotation: 0f, layerDepth: 0f);
        
        string highscoreContent = $"""
        Highscores:
          Beginner Marathon: {Game.SaveData.Highscores.Beginner.Lines} lines, {Game.SaveData.Highscores.Beginner.Score}pts. @ {FormatTime(FramesToTime(Game.SaveData.Highscores.Beginner.Frames))}
          Normal Score Attack: lv{Game.SaveData.Highscores.Normal.Level}, {Game.SaveData.Highscores.Normal.Score}pts. @ {FormatTime(FramesToTime(Game.SaveData.Highscores.Normal.Frames))}
          Apocalypse: lv{Game.SaveData.Highscores.Apocalypse.Level} @ {FormatTime(FramesToTime(Game.SaveData.Highscores.Apocalypse.Frames))}
          {(Game.SaveData.Highscores.FerocityUnlocked ? "Ferocity" : "???")}: {(Game.SaveData.Highscores.FerocityUnlocked ? Game.SaveData.Highscores.Ferocity.Level.ToString() : "???")} @ {(Game.SaveData.Highscores.FerocityUnlocked ? FormatTime(FramesToTime(Game.SaveData.Highscores.Ferocity.Frames)) : "???")}
        """;

        Game.MediumFont.DrawText(_batch, highscoreContent, new Vector2(0, 0), Color.White, effect: FontSystemEffect.Stroked, effectAmount: 1);
        _batch.End();
    }
    
    private (Vector2, float) ComputeBackgroundScale(Texture2D tex)
    {
        var xB = tex.Width;
        var yB = tex.Height;

        var xR = Game.W / (float)xB;
        var yR = Game.H / (float)yB;
        var scale = Math.Max(xR, yR);

        var excessX = xB * scale - Game.W;
        var excessY = yB * scale - Game.H;

        return (new Vector2(-excessX / 2, -excessY / 2), scale);
    }
}