using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace FentrisDesktop.Sound;

public class DictSfxManager : ISoundEffectManager
{
    private Dictionary<SoundEffects, SoundEffect> _sfx;
    private Dictionary<SoundEffects, SoundEffectInstance> _instances = new();

    public DictSfxManager(Dictionary<SoundEffects, SoundEffect> sfx)
    {
        _sfx = sfx;
    }

    public void PlaySound(SoundEffects se)
    {
        _sfx[se].Play();
    }

    public void PlaySoundOnce(SoundEffects se)
    {
        if (!_instances.ContainsKey(se))
        {
            _instances[se] = _sfx[se].CreateInstance();
        }
        _instances[se].Play();
    }
}