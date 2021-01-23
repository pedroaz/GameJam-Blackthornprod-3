using UnityEngine;
using UnityEngine.Events;

public class HUD : MonoBehaviour
{
    //Game Ended support
    EventMessages.GameEnded gameEndedEvent = new EventMessages.GameEnded();
    bool gameEnded = false;

    /// <summary>
    /// Adds the given listener for the GameEnded event
    /// </summary>
    /// <param name="listener">listener</param>
    public void AddGameEndedListener(UnityAction listener)
    {
        gameEndedEvent.AddListener(listener);
    }
}
