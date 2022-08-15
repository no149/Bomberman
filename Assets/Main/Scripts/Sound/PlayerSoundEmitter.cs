using System;
using UnityEngine;

public class PlayerSoundEmitter : SoundEmitter
{
    Player _player;
    private const string HarmedEventName = "Harmed";

    public PlayerSoundEmitter(AudioSource audioSource, Player player) : base(audioSource)
    {
        _player = player;
    }

    protected override void RegisterEventSounds()
    {
        RegisterHarmedEventSound();
    }

    private void RegisterHarmedEventSound()
    {
        _player.Harmed += Harmed;
        RegisterEventSound(HarmedEventName, "player_damage");
    }

    private void Harmed(object sender, int e)
    {
        EmitSound(HarmedEventName);
    }
}