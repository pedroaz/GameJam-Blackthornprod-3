using System.Collections;
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
        //Plays the button sound
        AudioManager.PlaySFX(AudioClipNames.ButtonClick);

        //Closes the current menu
        Destroy(gameObject);
    }

    /// <summary>
    /// Returns to the main menu
    /// </summary>
    public void BackToMenuButtonClick()
    {
        //Plays the button sound
        AudioManager.PlaySFX(AudioClipNames.ButtonClick);

        //Returns to the main menu
        MenuManager.GotoMenu(MenuNames.MainMenu);
    }

    /// <summary>
    /// Handles the quit button functionality
    /// </summary>
    private IEnumerator QuitButtonClickedExec()
    {
        //Adds a small delay to let the button sound play
        yield return new WaitForSeconds(AudioManager.GetAudioClipLength(AudioClipNames.ButtonClick) / 2);

        //Quits the game
        Application.Quit();
    }
}
