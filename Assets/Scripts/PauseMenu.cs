using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] List<Button> buttonsToDisable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
    }

    public void CloseOptions() 
    {
        foreach (var button in buttonsToDisable)
        {
            button.interactable = true;
        }
    }
}
