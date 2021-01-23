using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        //Plays the Main Menu start effects when returning to this menu
        AudioManager.PlaySFXFollowedByBGM(new[] { AudioClipNames.MainMenuStart, AudioClipNames.MainMenuMusic });
    }


    /// <summary>
    /// Starts the game
    /// </summary>
    public void StartGameButtonClick()
    {
        MenuManager.GoToNextGameplayScene();
    }

    /// <summary>
    /// Opens the help menu PopUp
    /// </summary>
    public void HelpMenuButtonClicked()
    {
        MenuManager.GotoMenu(MenuNames.HelpMenu);
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitGameButtonClick()
    {
        Application.Quit();
    }
}
