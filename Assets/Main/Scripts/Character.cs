using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int HealthPoints;

    public bool respawnable;

    internal event EventHandler<int> Harmed;

    internal event EventHandler<bool> Died;

    internal event EventHandler<GameObject> Collided;

    protected Vector2 OriginalLocation;

    public abstract List<string> HazardTagNames { get; }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        OriginalLocation = gameObject.transform.position;
        GameManager.GameManagerInstance.SetupCharacter(this);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    public void Die(bool reincarnate)
    {
        if (respawnable && reincarnate)
        {
            gameObject.SetActive(false);
            transform.position = OriginalLocation;
            gameObject.SetActive(true);
            if (Died != null) Died(this, true);
        }
        else
        {
            Destroy (gameObject);
            if (Died != null) Died(this, false);
        }
    }

    protected void RaiseHarmed(int healthPoints)
    {
        if (Harmed != null) Harmed(this, healthPoints);
    }

    public void Harm()
    {
        print("Character Harm called");
        HealthPoints--;
        Die(HealthPoints > 0);

        RaiseHarmed (HealthPoints);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Collided?.Invoke(this, collision.gameObject);
    }
}
