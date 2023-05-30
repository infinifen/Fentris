namespace FentrisDesktop.Sound;

public interface ISoundEffectManager
{
    public void PlaySound(SoundEffects se);
    void PlaySoundOnce(SoundEffects se);
}