using UnityEngine;

public class HelpMenuController : MonoBehaviour
{
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        //Check for the ESC key being pressed
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Called during the destruction of the prefab
    /// </summary>
    private void OnDestroy()
    {
        //Inform the game we are not in the help menu anymore
        MenuManager.IsInHelpMenu = false;
    }

    /// <summary>
    /// Closes the current popup menu
    /// </summary>
    public void ClosePopUpButtonClick()
    {
        Destroy(gameObject);
    }
}
