using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Hazard
{
    public enum BombPower
    {
        Low = 1,
        Mid,
        High,
    }


    public BombPower Power;
    public int ExplosionWindow;

    public event EventHandler<GameObject> ObjectHit;

    public event EventHandler Detonated;

    public const string TagName = "Bomb";

    Animator _animator;
    bool _fuming;
    bool _detonated;

    public override SoundEmitter SoundEmitter => new BombSoundEmitter(GetComponent<AudioSource>(), this);

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
            switch (Power)
            {
                case BombPower.Low:
                    return 1.5f;
                case BombPower.Mid:
                    return 1.7f;
                case BombPower.High:
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
        SoundEmitter.Init();
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
        _detonated = true;
        _animator.SetBool("Detonated", true);
        _animator.SetInteger("HitRadius", (int)Power);
        if (Detonated != null) Detonated(this, EventArgs.Empty);
    }

    void Fume_Over()
    {
        Destroy(gameObject);
    }
}
