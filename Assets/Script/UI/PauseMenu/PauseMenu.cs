using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject Buttons;
    public CinemachineInputProvider cinemachineInput;
    private void Start()
    {
        pauseMenuUI.SetActive(false);
        Buttons.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt)) 
        {
            if (gamePaused) 
            {
                Resume();
            }
            else 
            {
                Pause();
            }
        }
    }
    public void Resume() 
    {
        pauseMenuUI.SetActive(false);
        Buttons.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cinemachineInput.enabled = true;

    }
    void Pause() 
    {
        pauseMenuUI.SetActive(true);
        Buttons.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        cinemachineInput.enabled = false;
    }
}
