using UnityEngine;

public abstract class Hazard : MonoBehaviour
{
    public abstract bool CanKill { get; }
}
