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

    public const string AntagonistTagName = "Player";

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

    float _turnAngle;

    int _turnAngleCoefficient = 1;

    private Vector3 _lastPos;
    bool _collided;
    GameObject _lastCollisionObject;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _rb2D = GetComponent<Rigidbody2D>();
        SetMoveDirection(true);
    }

    private void SetMoveDirection(bool init)
    {
        if (init)
            if (MovementDirection == MovementDirectionEnum.Horizontal)
                _turnAngle = Mathf.Repeat(_turnAngle, 360);
            else
                _turnAngle = 90;
        else
        {
            if (MovementDirection != MovementDirectionEnum.Both)
            {
                if (MovementDirection == MovementDirectionEnum.Vertical)
                    _turnAngle *= -1;
                else
                    _turnAngle = _turnAngle == 0 ? 180 : 0;
            }
            else
            {
                // print("old angle:" + _turnAngle);
                var coef = Random.Range(-2, 3);

                var newAngle = 90 * coef;
                if (newAngle == _turnAngle || 180 == Mathf.Abs(newAngle))
                {
                    coef = Random.Range(-2, 3);
                    newAngle = 90 * coef;
                }

                _turnAngle = newAngle;
                //   _turnAngle = Mathf.Repeat(newAngle, 360);
                //print (newAngle);
            }
        }

        print("final new angle:" + _turnAngle);
        //_turnAngle = Mathf.Repeat(_turnAngle, 360);
        float inputAngleRadians = _turnAngle * Mathf.Deg2Rad;
        _XmovementDirection =
            _turnAngleCoefficient * Mathf.Cos(inputAngleRadians);
        _YmovementDirection =
            _turnAngleCoefficient * Mathf.Sin(inputAngleRadians);
    }
    // Update is called once per frame
    protected override void Update()
    {


        var curpos = gameObject.transform.position;
        curpos.Normalize();
        //To prevent warbling in place, because of frame-rate.
        var changeDirCoef = 1f;
        if (curpos == _lastPos)
        {
            print(curpos.x);
            print(_lastPos.x);
            SetMoveDirection(false);

            //
            changeDirCoef = 1.1f;
        }

        _rb2D.velocity = new Vector2(Speed * _XmovementDirection * changeDirCoef,
                Speed * _YmovementDirection * changeDirCoef);
        _lastPos = curpos;
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        _collided = true;
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
