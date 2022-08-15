using UnityEngine;

public abstract class Hazard : BombermanGameObject
{
    public abstract bool CanKill { get; }
}
