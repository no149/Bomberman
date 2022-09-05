using UnityEngine;
interface BadGuyMovementController
{
    MovementType MovementType { get; }
    MovementDirection CurrentDirection { get; }
    void Init();
    void Move();
    void ChangeDirection();
    void Stop();
    void FollowTarget(GameObject target);
}