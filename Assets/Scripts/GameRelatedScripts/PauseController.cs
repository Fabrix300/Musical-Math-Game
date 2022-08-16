using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public event Action OnPauseGame;
    public event Action OnResumeGame;

    private PlayerControls playerControls;

    private void Start()
    {
        playerControls = GameManager.instance.playerControls;
    }

    private void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
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
        playerControls.gameObject.SetActive(true);
        Time.timeScale = 1;
        GameIsPaused = false;
    }
}
