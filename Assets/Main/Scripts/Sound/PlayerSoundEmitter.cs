using UnityEngine;

public class PlayerSoundEmitter : SoundEmitter
{
    Player _player;
    public PlayerSoundEmitter(AudioSource audioSource, Player player) : base(audioSource)
    {
        _player = player;
    }

    protected override void RegisterEventSounds()
    {
        throw new System.NotImplementedException();
    }
}