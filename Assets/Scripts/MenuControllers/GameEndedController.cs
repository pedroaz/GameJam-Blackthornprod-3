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
    /// Goes to the next level
    /// </summary>
    public void NextLevelButtonClick()
    {
        MenuManager.GoToNextGameplayScene();
    }

    /// <summary>
    /// Retries the current game level
    /// </summary>
    public void TryAgainButtonClick()
    {
        MenuManager.TryCurrentSceneAgain();
    }

    /// <summary>
    /// Returns to the main menu
    /// </summary>
    public void BackToMenuButtonClick()
    {
        MenuManager.GotoMenu(MenuNames.MainMenu);
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitGameButtonClick()
    {
        Application.Quit();
    }
}
