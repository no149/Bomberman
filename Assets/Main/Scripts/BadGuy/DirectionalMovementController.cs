using UnityEngine;
class DirectionalMovementController
{
    public void FollowTarget(GameObject target)
    {

    }
    // private IEnumerator ChaseAntagonist()
    // {
    //     var endPosition = _antagonistTransform.position;
    //     float remainingDistance =
    //         (transform.position - endPosition).sqrMagnitude;
    //     while (remainingDistance > float.Epsilon)
    //     {
    //         _animator.SetBool("Running", true);
    //         Vector3 newPosition =
    //             Vector3
    //                 .MoveTowards(transform.position,
    //                 endPosition,
    //                 BaseSpeed * Time.deltaTime);

    //         _rb2D.MovePosition(newPosition);
    //         remainingDistance =
    //             (transform.position - endPosition).sqrMagnitude;

    //         yield return new WaitForFixedUpdate();
    //     }
    // }

    // public void Move()
    // {
    //     if (_antagonistInSight && ChasePlayer)
    //     {
    //         _currentSpeed = BaseSpeed * 1.2f; //pursuitSpeed;            
    //         _animator.SetBool("Running", true);
    //         if (_chaseAntagonistRoutine == null)
    //             _chaseAntagonistRoutine = StartCoroutine(ChaseAntagonist());
    //     }
    //     else
    //     {
    //         if (_chaseAntagonistRoutine != null)
    //         {
    //             StopCoroutine(_chaseAntagonistRoutine);
    //             _chaseAntagonistRoutine = null;
    //             _animator.SetBool("Running", false);
    //             _currentSpeed = BaseSpeed;
    //         }
    //         var curpos = gameObject.transform.position;
    //         //To prevent warbling in place, because of frame-rate.
    //         var changeDirCoef = 1f;
    //         var curPosRounded = curpos.Round(2);
    //         if (curPosRounded == _lastPos)
    //         {
    //             ChangeMoveDirection(false);
    //             changeDirCoef = 1.1f;
    //         }

    //         _lastPos = curPosRounded;
    //         _rb2D.velocity = new Vector2(_currentSpeed * _XmovementDirection * changeDirCoef,
    //                 _currentSpeed * _YmovementDirection * changeDirCoef);
    //     }

    //  public MovementDirection CurrentDirection
    //     {
    //         get
    //         {
    //             if (MovementType != MovementDirection.RandomDirection)
    //                 return MovementType;
    //             else
    //             {
    //                 return _YmovementDirection == 0 && Mathf.Abs(_XmovementDirection) == 1 ? MovementDirection.Horizontal :
    //                 MovementDirection.Vertical;
    //             }
    //         }
    //     }

    //  private void ChangeMoveDirection(bool init)
    //     {
    //         if (init)
    //             if (MovementType == MovementDirection.Horizontal)
    //                 _turnAngle = Mathf.Repeat(_turnAngle, 360);
    //             else
    //                 _turnAngle = 90;
    //         else
    //         {
    //             if (MovementType != MovementDirection.RandomDirection)
    //             {
    //                 if (MovementType == MovementDirection.Vertical)
    //                     _turnAngle *= -1;
    //                 else
    //                     _turnAngle = _turnAngle == 0 ? 180 : 0;
    //             }
    //             else
    //             {
    //                 var coef = Random.Range(-2, 3);

    //                 var newAngle = 90 * coef;
    //                 if (newAngle == _turnAngle || 180 == Mathf.Abs(newAngle))
    //                 {
    //                     coef = Random.Range(-2, 3);
    //                     newAngle = 90 * coef;
    //                 }

    //                 _turnAngle = newAngle;
    //             }
    //         }
    //         float inputAngleRadians = _turnAngle * Mathf.Deg2Rad;
    //         _XmovementDirection = Mathf.Cos(inputAngleRadians);
    //         _YmovementDirection = Mathf.Sin(inputAngleRadians);
    //     }
}