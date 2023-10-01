using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] List<Button> buttonsToDisable;
    [SerializeField] GameObject optionsMenu;

    public void Pause() 
    { 
        Time.timeScale = 0;
        pauseMenu.SetActive(true);

    }

    public void Unpause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void OpenOptions()
    {
        foreach (var button in buttonsToDisable)
        {
            button.interactable = false;
        }
        optionsMenu.SetActive(true);
    }

    public void CloseOptions() 
    {
        foreach (var button in buttonsToDisable)
        {
            button.interactable = true;
        }
        optionsMenu?.SetActive(false);
    }
}
