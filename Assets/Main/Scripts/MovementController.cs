using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
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

    Animator animator;

    public float movementSpeed = 3.0f;

    Vector2 movement = new Vector2();

    Rigidbody2D rb2D;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
        rb2D.velocity = movement * movementSpeed;

        if (movement.x == 0 && movement.y == 0)
            animator.SetInteger(Direction, (int) MovementState.Idle);
        else if (movement.x == 1)
            animator.SetInteger(Direction, (int) MovementState.Right);
        else if (movement.x == -1)
            animator.SetInteger(Direction, (int) MovementState.Left);
        if (movement.y == 1)
            animator.SetInteger(Direction, (int) MovementState.Top);
        else if (movement.y == -1)
            animator.SetInteger(Direction, (int) MovementState.Bottom);
    }
}
