using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] List<Button> buttonsToDisable;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] float maxVolume;
    [SerializeField] float minVolume;

    private void Start()
    {
        LoadOptions();
        UpdateSliders();
    }

    public void Pause() 
    {
        Debug.Log("Paused");
        pauseMenu.SetActive(true);
        Time.timeScale = 0;

    }

    public void Unpause()
    {
        Debug.Log("Unpause");
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
        SaveOptions();
        optionsMenu.SetActive(false);
    }


    public void SaveOptions()
    {
        PlayerPrefs.SetFloat("musicVolume", ((maxVolume - minVolume) * musicSlider.value) + minVolume);
        PlayerPrefs.SetFloat("sfxVolume", ((maxVolume - minVolume) * sfxSlider.value) + minVolume);
        LoadOptions();
    }

    void LoadOptions()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            audioMixer.SetFloat("musicVolume", PlayerPrefs.GetFloat("musicVolume"));
        }
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            audioMixer.SetFloat("sfxVolume", PlayerPrefs.GetFloat("sfxVolume"));
        }
    }

    void UpdateSliders()
    {
        if (!PlayerPrefs.HasKey("musicVolume")) { return; }
        float newVolume = PlayerPrefs.GetFloat("musicVolume");
        float newVolumeRelative = (newVolume - minVolume) / (maxVolume - minVolume);
        musicSlider.value = newVolumeRelative;
        if (!PlayerPrefs.HasKey("sfxVolume")) { return; }
        newVolume = PlayerPrefs.GetFloat("sfxVolume");
        newVolumeRelative = (newVolume - minVolume) / (maxVolume - minVolume);
        sfxSlider.value = newVolumeRelative;
    }

}
