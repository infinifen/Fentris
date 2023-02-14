using System;
using System.Collections.Generic;

namespace FentrisDesktop.Gamemode;

public class GamemodeRegistry
{
    public delegate GamemodeRenderer RegistryReturn(FentrisGame game, Gamemode mode);

    public static readonly Dictionary<string, RegistryReturn> Registry = new()
    {
        { "base", (game, mode) => new GamemodeRenderer(game, mode) },
        { "normal", (game, mode) => new NormalRenderer(game, mode) },
        { "apocalypse", (game, mode) => new ApocalypseRenderer(game, mode) }
    };

    public static RegistryReturn GetRendererForId(string id)
    {
        return Registry.GetValueOrDefault(id, (game, mode) => new GamemodeRenderer(game, mode));
    }
}