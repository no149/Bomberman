using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Hazard
{
    public enum HitRadius
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Super = 5
    }

    public int DangerFactor;
    public int ExplosionWindow;

    public event EventHandler<GameObject> ObjectHit;

    public event EventHandler Detonated;

    public HitRadius RayHitRadius;

    Animator _animator;

    bool _fuming;

    bool _detonated;

    GameObject _lastCollidedObject;

    public const string TagName = "Bomb";

    public override bool CanKill
    {
        get
        {
            return _detonated && !_fuming;
        }
    }

    float SpeedMultiplier
    {
        get
        {
            switch (RayHitRadius)
            {
                case HitRadius.Low:
                    return 1.5f;
                case HitRadius.Medium:
                    return 1.7f;
                case HitRadius.High:
                    return 2f;
                case HitRadius.Super:
                    return 3f;
                default:
                    return 0;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("OnTimedEvent", 0, 1);
        _animator = GetComponent<Animator>();
        _animator.SetFloat("SpeedMultiplier", SpeedMultiplier);
    }

    private void OnTimedEvent()
    {
        ExplosionWindow--;
        if (ExplosionWindow < 1)
        {
            CancelInvoke();
            Detonate();
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (_detonated && ObjectHit != null && !_fuming)
        {
            ObjectHit(this, collision.gameObject);
            print("bomb collided with object:" + collision.gameObject.tag);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _animator
            .SetBool("Done",
            gameObject.transform.localScale.y >= (float)RayHitRadius * .3);
    }

    void Fume_Started()
    {
        _fuming = true;
    }

    void Detonate()
    {
        print("detonated");
        _detonated = true;
        var boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        var circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
        boxCollider2D.enabled = true;
        circleCollider2D.enabled = false;
        _animator.SetBool("Detonated", true);
        if (Detonated != null) Detonated(this, EventArgs.Empty);
    }

    void Fume_Over()
    {
        Destroy(gameObject);
    }
}
