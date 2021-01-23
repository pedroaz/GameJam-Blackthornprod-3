using UnityEngine;

public class GameAudioSource : MonoBehaviour
{
    #region Private Methods

    /// <summary>
    /// Awake is called before Start
    /// </summary>
    void Awake()
    {
        //Checks if the audio manager was already initialized
        if (!AudioManager.Initialized)
        {
            //Add audio sources to the game object
            AudioSource audioSourceBGM = gameObject.AddComponent<AudioSource>();
            AudioSource audioSourceSFX = gameObject.AddComponent<AudioSource>();

            //Initializes the manager
            AudioManager.Initialize(audioSourceBGM, audioSourceSFX);

            //Persist this object throuughout the scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Duplicate object was created
            Destroy(gameObject);
        }
    }

    #endregion
}
