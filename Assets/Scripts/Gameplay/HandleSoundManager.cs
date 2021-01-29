using UnityEngine;

public class HandleSoundManager : MonoBehaviour
{
    [SerializeField]
    AudioClipNames audioToPlay;

    /// <summary>
    /// Processes trigger collisions with other game objects
    /// </summary>
    /// <param name="other">information about the other collider</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only play sounds when the game is running
        if (MenuManager.IsGamePaused) return;

        // if colliding with the handle, play the appropriate sound
        if (collision.gameObject.CompareTag("ShieldHandle"))
        {
            AudioManager.PlaySFX(audioToPlay);
        }
    }
}
