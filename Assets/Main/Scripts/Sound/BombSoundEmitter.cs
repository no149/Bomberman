using System;
using UnityEngine;

public class BombSoundEmitter : SoundEmitter
{
    private Bomb _bomb;
    private const string DetonatedEventName = "Detonated";
    public BombSoundEmitter(AudioSource audioSource, Bomb bomb) : base(audioSource)
    {
        _bomb = bomb;
    }

    private void Detonated(object sender, EventArgs e)
    {
        Debug.Log("emitting bomb explosion sound...");
        EmitSound(DetonatedEventName);
    }

    protected override void RegisterEventSounds()
    {
        RegisterBombDetonation();
    }

    private void RegisterBombDetonation()
    {
        _bomb.Detonated += Detonated;
        RegisterEventSound(DetonatedEventName, "explosion");
    }

}