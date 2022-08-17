using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject configUI;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Text musicVolumeSliderText;
    public Text sfxVolumeSliderText;

    public event Action OnPauseGame;
    public event Action OnResumeGame;

    private AudioManager audioManager;
    private PlayerControls playerControls;
    private float musicVolume;
    private float sfxVolume;

    private void Start()
    {
        audioManager = AudioManager.instance;
        musicVolume = audioManager.musicVolume;
        sfxVolume = audioManager.sfxVolume;
        playerControls = GameManager.instance.playerControls;
    }

    private void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                if (GameIsPaused) Resume();
                else Pause();
            }
        }
    }

    public void Pause()
    {
        OnPauseGame?.Invoke();
        pauseMenuUI.SetActive(true);
        playerControls.gameObject.SetActive(false);
        Time.timeScale = 0;
        GameIsPaused = true;
    }

    public void Resume()
    {
        OnResumeGame?.Invoke();
        pauseMenuUI.SetActive(false);
        configUI.SetActive(false);
        playerControls.gameObject.SetActive(true);
        Time.timeScale = 1;
        GameIsPaused = false;
    }

    public void ToggleConfigUI()
    {
        if (configUI.activeSelf)
        {
            configUI.SetActive(false);
        }
        else
        {
            configUI.SetActive(true);
            musicVolumeSlider.value = audioManager.musicVolume;
            musicVolumeSliderText.text = audioManager.musicVolume.ToString("N2");
            sfxVolumeSlider.value = audioManager.sfxVolume;
            sfxVolumeSliderText.text = audioManager.sfxVolume.ToString("N2");
        }
    }

    public void AdjustMusicVolume(float _musicVolume)
    {
        musicVolume = _musicVolume;
        musicVolumeSliderText.text = musicVolume.ToString("N2");
    }

    public void AdjustSfxVolume(float _sfxVolume)
    {
        sfxVolume = _sfxVolume;
        sfxVolumeSliderText.text = sfxVolume.ToString("N2");
    }

    public void SaveChanges()
    {
        audioManager.AdjustMusicVolume(musicVolume);
        audioManager.AdjustSfxVolume(sfxVolume);
        configUI.SetActive(false);
    }
}
