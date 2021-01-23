using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Enum of the menus present in the game
/// </summary>
public enum MenuNames
{
    MainMenu,
    HelpMenu,
    PauseMenu,
    Gameplay1,
    Gameplay2,
    Gameplay3,
    GameOver,
    LevelEnded
}

public class MenuManager : MonoBehaviour
{
    #region Fields

    static bool isGamePaused = false;
    static bool isInHelpMenu = false;

    private static int currentGameplayScene = -1;
    private static readonly MenuNames[] sceneOrder = new[] { MenuNames.Gameplay1, MenuNames.Gameplay2, MenuNames.Gameplay3 };

    #endregion

    #region Properties

    /// <summary>
    /// Checks if the game is pause
    /// </summary>
    public static bool IsGamePaused
    {
        get { return isGamePaused; }
        set { isGamePaused = value; }
    }

    /// <summary>
    /// Checks if the help menu is on screen
    /// </summary>
    public static bool IsInHelpMenu
    {
        get { return isInHelpMenu; }
        set { isInHelpMenu = value; }
    }

    #endregion

    /// <summary>
    /// Selects which menu should be displayed to the player
    /// </summary>
    /// <param name="toMenu">Menu that will be brought up</param>
    public static void GotoMenu(MenuNames toMenu)
    {
        switch (toMenu)
        {
            case MenuNames.Gameplay1:
                SceneManager.LoadScene("GameplayScene1");
                break;
            case MenuNames.Gameplay2:
                SceneManager.LoadScene("GameplayScene2");
                break;
            case MenuNames.Gameplay3:
                SceneManager.LoadScene("GameplayScene3");
                break;
            case MenuNames.MainMenu:
                SceneManager.LoadScene("MainMenuScene");
                break;
            case MenuNames.HelpMenu:
                isInHelpMenu = true;
                Object.Instantiate(Resources.Load("Menus/MenuHelp"));
                break;
            case MenuNames.PauseMenu:
                isGamePaused = true;
                Object.Instantiate(Resources.Load("Menus/MenuPause"));
                break;
            case MenuNames.GameOver:
                isGamePaused = true;
                Object.Instantiate(Resources.Load("Menus/MenuGameOver"));
                break;
            case MenuNames.LevelEnded:
                isGamePaused = true;
                Object.Instantiate(Resources.Load("Menus/MenuLevelComplete"));
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Reloads the current gameplay scene
    /// </summary>
    public static void TryCurrentSceneAgain()
    {
        GotoMenu(sceneOrder[currentGameplayScene]);
    }

    /// <summary>
    /// Goes to the next scene in the ordered array
    /// </summary>
    public static void GoToNextGameplayScene()
    {
        currentGameplayScene++;
        GotoMenu(sceneOrder[currentGameplayScene]);
    }
}
