using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenu, pauseButton;
    private GameSceneManager gsm;
    private UIAudioManager UI_audioManager;

    private void Awake()
    {
        gsm = GetComponent<GameSceneManager>();
        UI_audioManager = FindObjectOfType<UIAudioManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused == true)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        UI_audioManager.playSFX("Button");
        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void GoToMainMenu()
    {
        isPaused = false;
        Time.timeScale = 1f;
        gsm.SwitchScene(0);
    }
}
