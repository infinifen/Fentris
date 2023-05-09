using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace FentrisDesktop.Sound;

public class DictSfxManager : ISoundEffectManager
{
    private Dictionary<SoundEffects, SoundEffect> _sfx;

    public DictSfxManager(Dictionary<SoundEffects, SoundEffect> sfx)
    {
        _sfx = sfx;
    }

    public void PlaySound(SoundEffects se)
    {
        _sfx[se].Play();
    }
}