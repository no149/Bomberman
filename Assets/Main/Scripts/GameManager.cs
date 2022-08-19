using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameManager : MonoBehaviour
{
    public enum GameEndReason

    {
        PlayerDied,
        PlayerWon,
        TimeUp
    }
    private const string SpwanPointTagName = "SpawnPoint";
    private const string HazardTagName = "Hazard";
    private const string ExitBridgeName = "ExitBridge";


    private static GameManager _gameManager;

    public GameObject GameEndedCanvas;
    public GameObject FinishText;
    public GameObject Exit;
    public GameObject HeartPrefab;
    public GameObject BoxPrefab;

    internal int LevelNo = 1;
    int _currentBadguysCount;
    GameObject _exitBridge;
    event EventHandler<Box> _boxAdded;
    public static GameManager GameManagerInstance
    {
        get; private set;
    }
    void Awake()
    {
        if (GameManagerInstance != null && GameManagerInstance != this)
        {
            Destroy(gameObject);
        }
        else
            GameManagerInstance = this;
    }

    void Start()
    {
        SetupScene();
        Player.Instance.Died += Player_Died;
        Player.Instance.BombSpawned += Bomb_Spawned;
        _exitBridge = GameObject.Find(ExitBridgeName);
        SetBadguysCount();
        ToggleExitBridgeActiveState();
    }

    private void SetBadguysCount()
    {
        var badguys = GameObject.FindGameObjectsWithTag(BadGuy.TagName);
        _currentBadguysCount = badguys.Length;
        foreach (var badguy in badguys)
        {
            badguy.GetComponent<BadGuy>().Died += BadGuy_Died;
        }
    }

    private void BadGuy_Died(object sender, bool reincarnate)
    {
        if (!reincarnate)
            _currentBadguysCount--;
        if (_currentBadguysCount == 0)
            ToggleExitBridgeActiveState();
    }

    private void ToggleExitBridgeActiveState()
    {
        _exitBridge.SetActive(!_exitBridge.activeSelf);
        var underlyingwater = GameObject.Find("WaterUnderBridge");
        underlyingwater.SetActive(!_exitBridge.activeSelf);
    }

    private void RandomizeBoxHearts()
    {
        var breakables = GameObject.FindGameObjectsWithTag("Breakable");
        var boxes = breakables.Where(b => b.GetComponent<Box>() != null).Select(b => b.GetComponent<Box>()).ToArray();
        var heartsCount = (int)(boxes.Length * (0.3 / LevelNo));
        System.Random rand = new System.Random();
        for (int i = 0; i < heartsCount; i++)
        {
            var heartNo = rand.Next(heartsCount);
            boxes[heartNo].HasHeart = true;
        }
    }

    void Character_Collided(object sender, CollisionData collisionData)
    {
        GameObject collidedObject = collisionData.CollidedGameObject;
        if (sender is Player)
        {
            if (collisionData.IsTrigger && collidedObject.tag == HazardTagName)
            {
                Player.Instance.Die(false);
            }
            else if (collidedObject.tag == Player.AntagonistTagName && !collisionData.IsTrigger)
            {
                HarmPlayer((Player)sender, collidedObject.GetComponent<BadGuy>().DamagePower);
            }
            else if (collidedObject.name == "Exit")
            {
                EndGame(GameEndReason.PlayerWon);
            }
            else if (collidedObject.tag == "Health")
            {
                IncreasePlayerHealth();
                Destroy(collidedObject);
            }
        }
    }

    void IncreasePlayerHealth()
    {
        Player.Instance.HealthPoints++;
        LifeCount.Instance.Text = Player.Instance.HealthPoints.ToString();
    }

    void HarmPlayer(Player player, int damageLevel)
    {
        for (var i = 1; i <= damageLevel; i++)
            player.Harm();

        Player_Harmed(player.HealthPoints);
    }

    void Bomb_Spawned(object sender, Bomb bomb)
    {
        bomb.ObjectHit += Bomb_Hit;
    }

    void Bomb_Hit(object sender, GameObject hitObject)
    {
        if (hitObject.tag == BadGuy.TagName)
        {
            HarmBadGuy(hitObject);
        }
        else if (hitObject.tag == Player.TagName)
        {
            var character = hitObject.GetComponent<Character>();
            character.Die(false);
        }
        else if (hitObject.tag == "Breakable")
        {
            var box = hitObject.GetComponent<Box>();
            if (box.HasHeart)
                Instantiate(HeartPrefab, hitObject.transform.position, Quaternion.identity);
            Destroy(hitObject);
        }
    }

    private static void HarmBadGuy(GameObject badguy)
    {
        var badguyScript = badguy.GetComponent<BadGuy>();
        badguyScript.Harm();
    }

    void Player_Died(object sender, bool reincarnate)
    {
        if (!reincarnate) EndGame(GameEndReason.PlayerDied);
    }

    void Player_Harmed(int currentHealthPoints)
    {
        LifeCount.Instance.Text = currentHealthPoints.ToString();
        if (currentHealthPoints == 0 && GameEndedCanvas != null) EndGame(GameEndReason.PlayerDied);
    }

    void EndGame(GameEndReason endReason)
    {
        var endTextMesh = FinishText.GetComponent<TextMesh>();
        if (endReason == GameEndReason.PlayerDied)
        {
            endTextMesh.text = "Game over!";
            GameEndedCanvas.transform.Find("Lose Particle System").gameObject.SetActive(true);
            GameEndedCanvas.transform.Find("Win Particle System").gameObject.SetActive(false);
        }
        else if (endReason == GameEndReason.PlayerWon)
        {
            endTextMesh.text = "You won!";
            GameEndedCanvas.transform.Find("Lose Particle System").gameObject.SetActive(false);
            GameEndedCanvas.transform.Find("Win Particle System").gameObject.SetActive(true);
        }
        GameEndedCanvas.SetActive(true);
    }
    void HideIntroCanvas()
    {
        var introCanvas = GameObject.Find("IntroCanvas");
        introCanvas.SetActive(false);
    }
    private void SetupIntroCasvasTimer()
    {
        Invoke("HideIntroCanvas", 4);
    }


    public void SetupCharacter(Character character)
    {
        character.Collided += Character_Collided;
    }

    public void SetupScene()
    {
        LifeCount.Instance.Text = Player.Instance.HealthPoints.ToString();
        RandomizeBoxHearts();
        if (LevelNo == 1)
            SetupIntroCasvasTimer();
    }


}
