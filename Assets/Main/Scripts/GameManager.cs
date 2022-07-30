using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private static GameManager _gameManager;

    public GameObject GameEndedCanvas;
    public GameObject FinishText;

    public GameObject Exit;

    public static GameManager GameManagerInstance
    {
        get; private set;
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
    }

    void Character_Collided(object sender, CollisionData collisionData)
    {
        GameObject collidedObject = collisionData.CollidedGameObject;
        if (collisionData.IsTrigger && sender is Player && collidedObject.tag == HazardTagName)
            Player.Instance.Die(false);

        else if (
            ((MonoBehaviour)sender).tag == Player.TagName &&
            collidedObject.tag == Player.AntagonistTagName
        )
        {
            HarmPlayer((Player)sender, collidedObject.GetComponent<BadGuy>().DamagePower);
        }
        else if (((MonoBehaviour)sender).tag == Player.TagName && collidedObject.name == "Exit")
        {
            EndGame(GameEndReason.PlayerWon);
        }
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
        else if (hitObject.tag == "BreakableObject") Destroy(hitObject);
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
        print("Player_Harmed called:" + currentHealthPoints);
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

    // Update is called once per frame
    void Update()
    {
    }
}
