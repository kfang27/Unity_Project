using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject confirmationPanel;
    private bool gamePaused = false;

    [SerializeField] private GameOverUI gameOverUI;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Slider sensSliderValue;
    [SerializeField] private Slider volumeSliderValue;

    public PostProcessProfile brightnessProfile;
    private PostProcessLayer layer;
    private AutoExposure exposure;

    private CameraHandler cam;

    private void Start() {
        brightnessProfile.TryGetSettings(out exposure);
        SetSavedValues();

        cam = FindFirstObjectByType<CameraHandler>();

        UpdateSens();

        layer = FindFirstObjectByType<Camera>().GetComponent<PostProcessLayer>();
    }

    private void UpdateSens() {
        if (cam == null)
        {
            FindFirstObjectByType<CameraHandler>();
        }
        else
        {
            cam.lookSpeed = 0.05f * ((float)sensSliderValue.value / 8);
            cam.pivotSpeed = 0.05f * ((float)sensSliderValue.value / 8);
        }

    }

    private void Update() {
        // Checks if escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverUI.isActiveAndEnabled) {
            // if the game is paused, resume it
            if (gamePaused) {
                ResumeGame();
            }
            // if the game is not paused, pause it
            else {
                PauseGame();
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
    private void SetSavedValues()
    {
        sensSliderValue.value = PlayerPrefs.GetInt("masterSens");
        volumeSliderValue.value = PlayerPrefs.GetFloat("masterVolume");
        brightnessSlider.value = PlayerPrefs.GetFloat("masterBrightness");
    }

    public void SetBrightness(float brightness)
    {
        if (brightness != 0) {
            exposure.keyValue.value = brightness;
        } else {
            // Makes brightness very dark instead of pitch black when slider is set to 0
            exposure.keyValue.value = 0.05f;
        }
        PlayerPrefs.SetFloat("masterBrightness", brightness);
    }

    public void SetSensitivity(float sens) {
        PlayerPrefs.SetInt("masterSens", Mathf.RoundToInt(sens));
        UpdateSens();
    }

    public void SetVolume(float vol)
    {
        PlayerPrefs.SetFloat("masterVolume", vol);
    }

    public void OpenSettings() {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings() {
        settingsPanel.SetActive(false);
    }

    public void PauseGame() {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void ResumeGame() {
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ConfirmExit() {
        confirmationPanel.SetActive(true);
    }

    public void Exit(bool exit) {
        if (exit) {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }    
        else {
            confirmationPanel.SetActive(false);
        }
    }
}
