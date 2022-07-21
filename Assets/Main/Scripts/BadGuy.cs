using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGuy : Character
{
    public enum MovementDirectionEnum
    {
        Horizontal = 1,
        Vertical,
        Both
    }

    public MovementDirectionEnum MovementDirection;

    public float Speed = 3;

    // whether it can onlyh walk straight until it hits a block.
    public bool StraightWalker = true;

    public const string TagName = "BadGuy";

    public override string AntagonistTagName
    {
        get
        {
            return "Player";
        }
    }

    public override List<string> HazardTagNames
    {
        get
        {
            return new List<string>() { "Bomb" };
        }
    }

    Rigidbody2D _rb2D;

    float _XmovementDirection = 0;

    float _YmovementDirection = 0;

    float _turnAngle = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _rb2D = GetComponent<Rigidbody2D>();
        if (
            MovementDirection == MovementDirectionEnum.Horizontal ||
            MovementDirection == MovementDirectionEnum.Both
        )
        {
            _turnAngle = 0;
        }
        else if (MovementDirection == MovementDirectionEnum.Vertical)
        {
            _turnAngle = 90;
        }

        float inputAngleRadians = _turnAngle * Mathf.Deg2Rad;
        _XmovementDirection = Mathf.Cos(inputAngleRadians);
        _YmovementDirection = Mathf.Sin(inputAngleRadians);
    }

    // Update is called once per frame
    protected override void Update()
    {
        //print((speed * _XmovementDirection));
        _rb2D.velocity =
            new Vector2(Speed * _XmovementDirection,
                Speed * _YmovementDirection);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (
            MovementDirection == MovementDirectionEnum.Horizontal ||
            MovementDirection == MovementDirectionEnum.Vertical
        )
        {
            _turnAngle += 180;
        }
        else
            _turnAngle += 90;

        print (_turnAngle);
        _turnAngle = Mathf.Repeat(_turnAngle, 360);
        float inputAngleRadians = _turnAngle * Mathf.Deg2Rad;
        _XmovementDirection = Mathf.Cos(inputAngleRadians);

        _YmovementDirection = Mathf.Sin(inputAngleRadians);
    }

    Vector3 Vector3FromAngle(float inputAngleDegrees)
    {
        // 1
        float inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;

        // 2
        return new Vector3(Mathf.Cos(inputAngleRadians),
            Mathf.Sin(inputAngleRadians),
            0);
    }
}
