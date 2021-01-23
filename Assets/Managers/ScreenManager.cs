using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public List<GameObject> screens;
    public int currentScreenNumber = 0;

    public void HideAll()
    {
        foreach (var screen in screens) {
            screen.SetActive(false);
        }
    }

    public void GoToScene(int screenNumber)
    {
        HideAll();
        currentScreenNumber = screenNumber;
        screens[screenNumber].SetActive(true);
    }
}
