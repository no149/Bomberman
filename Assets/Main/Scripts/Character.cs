using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int HealthPoints;

    public bool respawnable;

    internal event EventHandler<int> Damaged;

    internal event EventHandler<bool> Killed;

    protected Vector2 OriginalLocation;

    public abstract string AntagonistTagName { get; }

    private const string HazardTagName = "Hazard";

    // Start is called before the first frame update
    protected virtual void Start()
    {
        OriginalLocation = gameObject.transform.position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    public void Kill(bool reincarnate)
    {
        if (respawnable && reincarnate)
        {
            gameObject.SetActive(false);
            transform.position = OriginalLocation;
            gameObject.SetActive(true);
            if (Killed != null) Killed(this, true);
        }
        else
        {
            Destroy (gameObject);
            if (Killed != null) Killed(this, false);
        }
    }

    protected void RaiseDamaged(int healthPoints)
    {
        if (Damaged != null) Damaged(this, healthPoints);
    }

    public void TakeDamage()
    {
        HealthPoints--;
        Kill(HealthPoints > 0);

        RaiseDamaged (HealthPoints);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == HazardTagName)
        {
            Kill(false);
        }
    }
}
