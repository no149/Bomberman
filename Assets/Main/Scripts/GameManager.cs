using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string SpwanPointTagName = "SpawnPoint";

    private static GameManager _gameManager;

    public GameObject GameOverCanvas;

    public static GameManager GameManagerInstance
    {
        get
        {
            if (_gameManager == null) _gameManager = new GameManager();
            return _gameManager;
        }
        private
        set
        {
            _gameManager = value;
        }
    }

    public void SetupScene()
    {
        LifeCount.Instance.Text = Player.Instance.HealthPoints.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManagerInstance != null && GameManagerInstance != this)
        {
            Destroy (gameObject);
        }
        else
            GameManagerInstance = this;

        SetupScene();
        Player.Instance.Damaged += Player_Damaged;
        Player.Instance.Killed += Player_Killed;
        Player.Instance.BombSpawned += Bomb_Spawned;
    }

    void Bomb_Spawned(object sender, Bomb bomb)
    {
        //   bomb.ObjectHit += Bomb_Hit;
    }

    void Bomb_Hit(object sender, GameObject hitObject)
    {
        print("bomb hit=" + hitObject.tag);
        if (hitObject.tag == BadGuy.TagName || hitObject.tag == Player.TagName)
        {
            var character = hitObject.GetComponent<Character>();
            character.TakeDamage();
        }
        else if (hitObject.tag == "BreakableObject") Destroy(hitObject);
    }

    void Player_Killed(object sender, bool reincarnate)
    {
        if (!reincarnate) GameOver();
    }

    void Player_Damaged(object sender, int currentHealthPoints)
    {
        print("Player_Damaged called");
        LifeCount.Instance.Text = currentHealthPoints.ToString();
        if (currentHealthPoints == 0 && GameOverCanvas != null) GameOver();
    }

    void GameOver()
    {
        GameOverCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
