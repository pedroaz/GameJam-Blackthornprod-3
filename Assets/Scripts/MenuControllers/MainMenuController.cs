using System.Collections;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        //Plays the Main Menu start effects when returning to this menu
        AudioManager.PlayBGM(AudioClipNames.MainMenuMusic);
    }


    /// <summary>
    /// Starts the game
    /// </summary>
    public void StartGameButtonClick()
    {
        //Stops the currently playing audio
        AudioManager.StopBGM();

        //Plays the button sound
        AudioManager.PlaySFX(AudioClipNames.ButtonClick);

        //Go to the gameplay scene
        MenuManager.GoToCurrentGameplayScene();
    }

    /// <summary>
    /// Opens the help menu PopUp
    /// </summary>
    public void HelpMenuButtonClicked()
    {
        //Plays the button sound
        AudioManager.PlaySFX(AudioClipNames.ButtonClick);

        //Goes to the menu popup
        MenuManager.GotoMenu(MenuNames.HelpMenu);
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
