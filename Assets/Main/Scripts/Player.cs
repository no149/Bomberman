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

    public Bomb[] BombPrefabs;
    internal Bomb SelectedBomb;
    internal BombTypeCount[] AvailableBombs;

    public bool CanSpawnMultipleBombs;
    public float movementSpeed = 3.0f;

    public const string AntagonistTagName = "BadGuy";

    public event EventHandler<Bomb> BombSpawned;
    public event EventHandler<Bomb.BombType> SelectedBombChanged;

    bool CanSpawnBomb
    {
        get
        {
            return AvailableBombs.Any(b => b.Count > 0);
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
        AvailableBombs = new BombTypeCount[BombPrefabs.Length];
        for (var i = 0; i < BombPrefabs.Length; i++)
        {
            if (BombPrefabs[i].Type == Bomb.BombType.Type1)
                AvailableBombs[i] = new BombTypeCount() { BombType = BombPrefabs[i].Type, Count = 4 };
            else
                AvailableBombs[i] = new BombTypeCount() { BombType = BombPrefabs[i].Type, Count = 0 };
        }
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
        else if (Input.GetKey(KeyCode.RightAlt) && (Input.GetKeyDown(KeyCode.Alpha1) ||
        Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3)))
        {
            Bomb.BombType bombType = Bomb.BombType.Type1;
            if (Input.GetKeyDown(KeyCode.Alpha1))
                bombType = Bomb.BombType.Type1;
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                bombType = Bomb.BombType.Type2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                bombType = Bomb.BombType.Type3;

            if (BombPrefabs != null)
            {
                var candidateBomb = AvailableBombs.Single(b => b.BombType == bombType);
                if (candidateBomb.Count > 0)
                {
                    SelectedBomb = BombPrefabs.SingleOrDefault(b => b.Type == bombType);
                    if (SelectedBomb == null)
                        throw new InvalidOperationException($"No Bomb prefab for bomb {bombType} is defined.");
                    if (SelectedBombChanged != null)
                        SelectedBombChanged(this, SelectedBomb.Type);
                }
            }
        }
    }

    void SpawnBomb()
    {
        if (CanSpawnBomb && !(!CanSpawnMultipleBombs && _detonatingBombs > 0))
        {
            if (SelectedBomb == null)
                return;

            var bomb =
                Instantiate(SelectedBomb,
                gameObject.transform.position,
                Quaternion.identity);
            bomb.Detonated += Bomb_Detonated;
            bomb.ObjectHit += Bomb_Hit;
            _detonatingBombs++;
            var bombCount = AvailableBombs.Single(b => b.BombType == bomb.Type);
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
