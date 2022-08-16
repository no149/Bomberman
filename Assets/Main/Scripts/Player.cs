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
    public event EventHandler<Bomb.BombPower> SelectedBombChanged;

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

    public override SoundEmitter SoundEmitter => new PlayerSoundEmitter(GetComponent<AudioSource>(), this);

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _movementController = new MovementController(GetComponent<Animator>(), GetComponent<Rigidbody2D>(),
        movementSpeed);
        AvailableBombs = new BombTypeCount[BombPrefabs.Length];
        for (var i = 0; i < BombPrefabs.Length; i++)
        {
            if (BombPrefabs[i].Power == Bomb.BombPower.Low)
                AvailableBombs[i] = new BombTypeCount() { BombType = BombPrefabs[i].Power, Count = 4 };
            else
                AvailableBombs[i] = new BombTypeCount() { BombType = BombPrefabs[i].Power, Count = 0 };
        }
        SoundEmitter.Init();
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
        else if (Input.GetKey(KeyCode.RightControl) && (Input.GetKeyDown(KeyCode.Alpha1) ||
        Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3)))
        {
            Bomb.BombPower bombType = Bomb.BombPower.Low;
            if (Input.GetKeyDown(KeyCode.Alpha1))
                bombType = Bomb.BombPower.Low;
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                bombType = Bomb.BombPower.Mid;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                bombType = Bomb.BombPower.High;

            if (BombPrefabs != null)
            {
                var candidateBomb = AvailableBombs.Single(b => b.BombType == bombType);
                if (candidateBomb.Count > 0)
                {
                    SelectedBomb = BombPrefabs.SingleOrDefault(b => b.Power == bombType);
                    if (SelectedBomb == null)
                        throw new InvalidOperationException($"No Bomb prefab for bomb {bombType} is defined.");
                    if (SelectedBombChanged != null)
                        SelectedBombChanged(this, SelectedBomb.Power);
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
            var bombCount = AvailableBombs.Single(b => b.BombType == bomb.Power);
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
