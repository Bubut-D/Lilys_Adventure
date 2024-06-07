using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;

    public void Settings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    public void Return()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
        
    }
}
