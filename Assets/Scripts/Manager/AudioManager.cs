using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource bgmAudio;
    [SerializeField] private AudioSource sfxAudio;

    protected override void Awake()
    {
        base.Awake();
    }

    public void PlayBGM(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        bgmAudio.clip = clip;
        bgmAudio.volume = volume;
        bgmAudio.loop = true;
        bgmAudio.Play();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        sfxAudio.PlayOneShot(clip, volume);
    }
}
