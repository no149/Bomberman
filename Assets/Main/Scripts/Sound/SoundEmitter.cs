using System;
using UnityEngine;

public abstract class SoundEmitter
{
    private const string SoundsDirectory = "Sounds";
    private AudioSource _audioSource;
    private System.Collections.Generic.Dictionary<string, string> _eventSounds;

    public SoundEmitter(AudioSource audioSource)
    {
        _audioSource = audioSource;
        _eventSounds = new System.Collections.Generic.Dictionary<string, string>();
    }

    public void Init()
    {
        RegisterEventSounds();
    }
    public void EmitSound(string eventName)
    {
        _audioSource.PlayOneShot((AudioClip)Resources.Load(SoundsDirectory + "/" + _eventSounds[eventName]));
    }
    protected void RegisterEventSound(string eventName, string soundFile)
    {
        _eventSounds.Add(eventName, soundFile);
    }

    protected abstract void RegisterEventSounds();
}