using Assets.Scripts.Gameplay;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Awake is called before Start
    /// </summary>
    void Awake()
    {
        EventManager.AddGameOverListener(HandleGameOverEvent);
        EventManager.AddGameEndedListener(HandleGameEndedEvent);
    }

    // Start is called before the first frame update
    private void Start()
    {
        //Stops the currently playing audio
        AudioManager.StopBGM();

        //Gets a random BGM music to play (Currently only 1 music available)
        int randomMusicNumber = Random.Range(0, 1);
        AudioClipNames randomAudioClip;
        switch (randomMusicNumber)
        {
            case 0:
                randomAudioClip = AudioClipNames.GameplayMusic0;
                break;
            case 1:
                randomAudioClip = AudioClipNames.GameplayMusic1;
                break;
            case 2:
                randomAudioClip = AudioClipNames.GameplayMusic2;
                break;
            default:
                randomAudioClip = AudioClipNames.GameplayMusic0;
                break;
        }

        //Plays the Gameplay start effects when returning to this menu
        AudioManager.PlayBGM(randomAudioClip);
    }

    /// <summary>
    /// Handles the ending of the game in a game over
    /// </summary>
    void HandleGameOverEvent()
    {
        //Clears the spawner and pools
        SetupGameEnded();

        //Loads the game over scene
        MenuManager.GotoMenu(MenuNames.GameOver);
    }

    /// <summary>
    /// Handles the ending of the game in a level complete
    /// </summary>
    void HandleGameEndedEvent()
    {
        //Clears the spawner and pools
        SetupGameEnded();

        //Loads the game over scene
        MenuManager.GotoMenu(MenuNames.LevelEnded);
    }

    /// <summary>
    /// Sets up the ending of the game in a level complete or game over
    /// </summary>
    void SetupGameEnded()
    {
        // Stops the currently playing audio
        AudioManager.StopBGM();

        // Stop enemy spawner
        EnemySpawner spawner = GameObject.Find("[Enemies Container]").GetComponent<EnemySpawner>();
        spawner.Stop();

        // Return all enemies and bullets to pools
        GameObject[] playerBullets = GameObject.FindGameObjectsWithTag("PlayerBullet");
        for (int i = playerBullets.Length - 1; i >= 0; i--)
        {
            ObjectPool.ReturnBullet(playerBullets[i]);
        }
        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for (int i = enemyBullets.Length - 1; i >= 0; i--)
        {
            ObjectPool.ReturnBullet(enemyBullets[i]);
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyShip");
        for (int i = enemies.Length - 1; i >= 0; i--)
        {
            ObjectPool.ReturnEnemy(enemies[i]);
        }
    }
}
