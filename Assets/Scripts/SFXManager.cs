using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : Singleton<SFXManager>
{
    public AudioSource source;
    public AudioClip buttonClcik;
    public AudioMixerGroup buttonClickGroup;

    public static void PlaySound(AudioClip clip, AudioMixerGroup group, float volume = 1f)
    {
        if (clip == null) return;

        Instance.source.clip = clip;
        Instance.source.volume = volume;
        Instance.source.outputAudioMixerGroup = group;
        Instance.source.Play();
    }

    public static void PlayButtonClick()
    {
        PlaySound(Instance.buttonClcik, Instance.buttonClickGroup);
    }
}
