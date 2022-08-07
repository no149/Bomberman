using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : Character
{
    private static Player _instance;
    MovementController _movementController;
    int _detonatingBombs;

    public Bomb Bomb;
    public BombTypeCount[] Bombs;

    public bool CanSpawnMultipleBombs;
    public float movementSpeed = 3.0f;

    public const string AntagonistTagName = "BadGuy";

    public event EventHandler<Bomb> BombSpawned;

    bool CanSpawnBomb
    {
        get
        {
            return Bombs.Any(b => b.Count > 0);
        }
    }
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
        _movementController = new MovementController(GetComponent<Animator>(), GetComponent<Rigidbody2D>(),
        movementSpeed);
    }

    // Update is called once per frame
    protected override void Update()
    {
        ProcessKeyInput();

    }

    private void ProcessKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnBomb();
        }
    }

    void SpawnBomb()
    {
        if (CanSpawnBomb && !(!CanSpawnMultipleBombs && _detonatingBombs > 0))
        {
            var bomb =
                Instantiate(Bomb,
                gameObject.transform.position,
                Quaternion.identity);
            bomb.Detonated += Bomb_Detonated;
            bomb.ObjectHit += Bomb_Hit;
            _detonatingBombs++;
            var bombCount = Bombs.Single(b => b.BombType == bomb.Type);
            if (bombCount.Count > 0)
                bombCount.Count--;
            if (BombSpawned != null) BombSpawned(this, bomb);
        }
    }
    private bool CanSpawnMultiDirectionalBomb()
    {
        return GameManager.GameManagerInstance.LevelNo > 0;
    }

    void FixedUpdate()
    {
        _movementController.Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
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
