using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController
{
    const string Direction = "Direction";

    enum MovementState
    {
        Idle,
        Left,
        Top,
        Right,
        Bottom
    }
    float _movementSpeed;

    Animator _animator;


    Vector2 movement = new Vector2();

    Rigidbody2D _rb2D;

    // Start is called before the first frame update
    public MovementController(Animator animator, Rigidbody2D rb2d, float movementSpeed)
    {
        _animator = animator;
        _rb2D = rb2d;
        _movementSpeed = movementSpeed;
    }

    // Update is called once per frame
    public void Move(float x, float y)
    {
        movement.x = x;
        movement.y = y;
        movement.Normalize();
        _rb2D.velocity = movement * _movementSpeed;

        if (movement.x == 0 && movement.y == 0)
            _animator.SetInteger(Direction, (int)MovementState.Idle);
        else if (movement.x == 1)
            _animator.SetInteger(Direction, (int)MovementState.Right);
        else if (movement.x == -1)
            _animator.SetInteger(Direction, (int)MovementState.Left);
        if (movement.y == 1)
            _animator.SetInteger(Direction, (int)MovementState.Top);
        else if (movement.y == -1)
            _animator.SetInteger(Direction, (int)MovementState.Bottom);
    }
}
