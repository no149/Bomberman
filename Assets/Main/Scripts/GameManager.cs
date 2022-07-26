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

    public void SetupCharacter(Character character)
    {
        character.Collided += Character_Collided;
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
            Destroy(gameObject);
        }
        else
            GameManagerInstance = this;

        SetupScene();
        Player.Instance.Died += Player_Died;
        Player.Instance.BombSpawned += Bomb_Spawned;
    }

    void Character_Collided(object sender, CollisionData collisionData)
    {
        GameObject collidedObject = collisionData.CollidedGameObject;
        if (collisionData.IsTrigger && sender is Player)
            Player.Instance.Die(false);

        else if (
            ((MonoBehaviour)sender).tag == Player.TagName &&
            collidedObject.tag == Player.AntagonistTagName
        )
        {
            HarmPlayer((Player)sender);
        }
    }

    void HarmPlayer(Player player)
    {
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
            var character = hitObject.GetComponent<Character>();
            character.Harm();
        }
        else if (hitObject.tag == Player.TagName)
        {
            var character = hitObject.GetComponent<Character>();
            character.Die(false);
        }
        else if (hitObject.tag == "BreakableObject") Destroy(hitObject);
    }

    void Player_Died(object sender, bool reincarnate)
    {
        if (!reincarnate) GameOver();
    }

    void Player_Harmed(int currentHealthPoints)
    {
        print("Player_Harmed called:" + currentHealthPoints);
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
