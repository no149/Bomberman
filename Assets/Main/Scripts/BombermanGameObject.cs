using UnityEngine;
public abstract class BombermanGameObject : MonoBehaviour
{
    SoundEmitter _soundEmitter;
    public abstract SoundEmitter SoundEmitter{get;}
}