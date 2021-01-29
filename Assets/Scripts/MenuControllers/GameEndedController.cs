using System.Collections;
using UnityEngine;

public class GameEndedController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        //Pausing the game
        Time.timeScale = 0;
    }

    /// <summary>
    /// Called during the destruction of the prefab
    /// </summary>
    private void OnDestroy()
    {
        //Unpausing the game
        Time.timeScale = 1;

        //Inform the game we are not paused anymore
        MenuManager.IsGamePaused = false;
    }

    /// <summary>
    /// Configures the menu to use the correct title and scores
    /// </summary>
    /// <param name="isGameOver">Game ended with the last ball being lost</param>
    public void SetupMenu(bool isGameOver)
    {
        //Configures the title and music appropriately
        if (isGameOver)
        {
            AudioManager.PlayBGM(AudioClipNames.GameOverMusic);
        }
        else
        {
            AudioManager.PlayBGM(AudioClipNames.LevelEndedMusic);
        }
    }

    /// <summary>
    /// Goes to the next level
    /// </summary>
    public void NextLevelButtonClick()
    {
        //Plays the button sound
        AudioManager.PlaySFX(AudioClipNames.ButtonClick);

        //Goes to the next gameplay scene
        MenuManager.GoToNextGameplayScene();
    }

    /// <summary>
    /// Retries the current game level
    /// </summary>
    public void TryAgainButtonClick()
    {
        //Plays the button sound
        AudioManager.PlaySFX(AudioClipNames.ButtonClick);

        //Loads the same scene again
        MenuManager.TryCurrentSceneAgain();
    }

    /// <summary>
    /// Returns to the main menu
    /// </summary>
    public void BackToMenuButtonClick()
    {
        //Plays the button sound
        AudioManager.PlaySFX(AudioClipNames.ButtonClick);

        //Repeats the current scene
        MenuManager.GotoMenu(MenuNames.MainMenu);
    }

    // <summary>
    /// Handles the quit button functionality
    /// </summary>
    private IEnumerator QuitButtonClickedExec()
    {
        //Plays the button sound
        AudioManager.PlaySFX(AudioClipNames.ButtonClick);

        //Adds a small delay to let the button sound play
        yield return new WaitForSeconds(AudioManager.GetAudioClipLength(AudioClipNames.ButtonClick) / 2);

        //Quits the game
        Application.Quit();
    }
}
