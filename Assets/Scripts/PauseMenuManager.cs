using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    private bool isPaused;
    public GameObject pausePanel;
    public Slider yawSensitivity;
    public Slider pitchSensitivity;
    public Slider fieldOfView;
    public Camera targetCamera;
    // Start is called before the first frame update
    private void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            isPaused = !isPaused;
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.Confined;
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }
    public void ResumeGame()
    {
        isPaused = false;
    }
    public void ReturnMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }
    public void YawSensitivitySlider()
    {
        PlayerPrefs.SetFloat("XSensitivity", yawSensitivity.value);
    }

    public void PitchSensitivitySlider()
    {
        PlayerPrefs.SetFloat("YSensitivity", pitchSensitivity.value);
    }
    public void fieldOfViewSlider()
    {
        targetCamera.fieldOfView = fieldOfView.value;
    }
}
