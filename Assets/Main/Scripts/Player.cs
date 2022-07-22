using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private static Player _instance;

    int _detonatingBombs;

    public Bomb Bomb;

    public bool CanSpawnMultipleBombs;

    public const string AntagonistTagName = "BadGuy";

    public event EventHandler<Bomb> BombSpawned;

    internal static Player Instance
    {
        get
        {
            return _instance;
        }
    }

    public const string TagName = "Player";

    public override List<string> HazardTagNames
    {
        get
        {
            return new List<string>();
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!(!CanSpawnMultipleBombs && _detonatingBombs > 0))
            {
                var bomb =
                    Instantiate(Bomb,
                    gameObject.transform.position,
                    Quaternion.identity);
                bomb.Detonated += Bomb_Detonated;
                bomb.ObjectHit += Bomb_Hit;
                _detonatingBombs++;
                if (BombSpawned != null) BombSpawned(this, bomb);
            }
        }
    }

    void Bomb_Detonated(object sender, EventArgs e)
    {
        _detonatingBombs--;
    }

    void Bomb_Hit(object sender, GameObject hitObject)
    {
        if (hitObject == gameObject)
        {
            Die(false);
        }
    }

    void Awake()
    {
        _instance = this;
    }
}
