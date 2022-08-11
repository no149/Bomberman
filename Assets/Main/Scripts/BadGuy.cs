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

    public int DamagePower;
    public MovementDirectionEnum MovementDirection;

    public float BaseSpeed = 3;

    // whether it can only walk straight until it hits a block.
    public bool ChasePlayer = true;

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
    Animator _animator;
    float _XmovementDirection = 0;

    float _YmovementDirection = 0;

    float _turnAngle;


    private Vector3 _lastPos;
    Transform _antagonistTransform;
    bool _antagonistInSight;
    private Coroutine _chaseAntagonistRoutine;
    float _currentSpeed;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        SetMoveDirection(true);
        _currentSpeed = BaseSpeed;
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
                var coef = Random.Range(-2, 3);

                var newAngle = 90 * coef;
                if (newAngle == _turnAngle || 180 == Mathf.Abs(newAngle))
                {
                    coef = Random.Range(-2, 3);
                    newAngle = 90 * coef;
                }

                _turnAngle = newAngle;
            }
        }
        float inputAngleRadians = _turnAngle * Mathf.Deg2Rad;
        _XmovementDirection = Mathf.Cos(inputAngleRadians);
        _YmovementDirection = Mathf.Sin(inputAngleRadians);
        print("_YmovementDirection:" + _YmovementDirection);
        print("_turnAngle:" + _turnAngle);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (_antagonistInSight && ChasePlayer)
        {
            _currentSpeed = BaseSpeed * 1.2f; //pursuitSpeed;            
            _animator.SetBool("Running", true);
            if (_chaseAntagonistRoutine == null)
                _chaseAntagonistRoutine = StartCoroutine(ChaseAntagonist());
        }
        else
        {
            if (_chaseAntagonistRoutine != null)
            {
                StopCoroutine(_chaseAntagonistRoutine);
                _chaseAntagonistRoutine = null;
                _animator.SetBool("Running", false);
                _currentSpeed = BaseSpeed;
            }
            var curpos = gameObject.transform.position;    
            //To prevent warbling in place, because of frame-rate.
            var changeDirCoef = 1f;
            if (curpos == _lastPos)
            {
                SetMoveDirection(false);
                changeDirCoef = 1.1f;
            }

            _lastPos = curpos;
            _rb2D.velocity = new Vector2(_currentSpeed * _XmovementDirection * changeDirCoef,
                    _currentSpeed * _YmovementDirection * changeDirCoef);
        }
    }

    private IEnumerator ChaseAntagonist()
    {
        var endPosition = _antagonistTransform.position;
        float remainingDistance =
            (transform.position - endPosition).sqrMagnitude;
        while (remainingDistance > float.Epsilon)
        {
            _animator.SetBool("Running", true);
            Vector3 newPosition =
                Vector3
                    .MoveTowards(transform.position,
                    endPosition,
                    BaseSpeed * Time.deltaTime);

            _rb2D.MovePosition(newPosition);
            remainingDistance =
                (transform.position - endPosition).sqrMagnitude;

            yield return new WaitForFixedUpdate();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag(AntagonistTagName) && ChasePlayer)
        {
            _antagonistTransform = collision.gameObject.transform;
            _antagonistInSight = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(AntagonistTagName))
        {
            _antagonistInSight = false;
            _antagonistTransform = null;
        }
    }



    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
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
