using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        //Pausing the game
        Time.timeScale = 0;
    }

    /// <summary>
	/// Update is called once per frame
	/// </summary>
    private void Update()
    {
        //Check for the ESC key being pressed
        if (!MenuManager.IsInHelpMenu && Input.GetKeyUp(KeyCode.Escape))
        {
            Destroy(gameObject);
        }
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
    /// Closes the current popup menu
    /// </summary>
    public void ClosePopUpButtonClick()
    {
        Destroy(gameObject);
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
