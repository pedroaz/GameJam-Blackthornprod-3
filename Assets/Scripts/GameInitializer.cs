using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    /// <summary>
    /// Awake is called before Start
    /// </summary>
	void Awake()
    {
        // initialize utils
        ScreenUtils.Initialize();
        ObjectPool.Initialize();
    }
}
