using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamaInitializer : MonoBehaviour
{
    private ScreenManager screenManager;

    private void Awake()
    {
        screenManager = FindObjectOfType<ScreenManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        screenManager.HideAll();
        screenManager.GoToScene(0);
    }
}
