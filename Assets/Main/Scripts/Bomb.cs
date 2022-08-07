using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Hazard
{
    public enum BombType
    {
        Type1,
        Type2,
        Type3,
    }
    public enum HitRadius
    {
        Low = 1,
        Medium = 2,
        High = 3,
    }


    public BombType Type;
    public int ExplosionWindow;

    public event EventHandler<GameObject> ObjectHit;

    public event EventHandler Detonated;

    public HitRadius RayHitRadius;
    public const string TagName = "Bomb";

    Animator _animator;
    bool _fuming;
    bool _detonated;

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

    }

    void Fume_Started()
    {
        _fuming = true;
    }

    void Detonate()
    {
        print("detonated:" + RayHitRadius);
        _detonated = true;
        _animator.SetBool("Detonated", true);
        _animator.SetInteger("HitRadius", (int)RayHitRadius);
        if (Detonated != null) Detonated(this, EventArgs.Empty);
    }

    void Fume_Over()
    {
        Destroy(gameObject);
    }
}
