using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pausePanel;
    public Slider yawSensitivity;
    public Slider pitchSensitivity;
    public Slider fieldOfView;
    public Camera GameCamera;
    public Camera ROS2Camera;
    // Start is called before the first frame update
    private void Start()
    {

    }
    // Update is called once per frame
    private void Update()
    {
        if (GimbalController.inputActions.Gameplay.Pause.WasPressedThisFrame())
            PauseGame();
        if (GimbalController.inputActions.UI.Cancel.WasPressedThisFrame())
            UnpauseGame();
    }
    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        GimbalController.inputActions.Gameplay.Disable();
        GimbalController.inputActions.UI.Enable();
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void UnpauseGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        GimbalController.inputActions.UI.Disable();
        GimbalController.inputActions.Gameplay.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void LoadScene(string scene)
    {
        Time.timeScale = 1;
        if (scene != "")
        {
            StartCoroutine(LoadAsynchronously(scene));
        }
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
        GimbalController.sensitivity.x = yawSensitivity.value;
    }
    public void PitchSensitivitySlider()
    {
        GimbalController.sensitivity.y = pitchSensitivity.value;
    }
    public void fieldOfViewSlider()
    {
        GameCamera.fieldOfView = fieldOfView.value;
        ROS2Camera.fieldOfView = fieldOfView.value;
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
                operation.allowSceneActivation = true;
            yield return null;
        }
    }
}
