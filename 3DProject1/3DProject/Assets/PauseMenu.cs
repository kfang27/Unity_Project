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

    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider brightnessSlider;

    public PostProcessProfile brightnessProfile;
    private PostProcessLayer layer;
    private AutoExposure exposure;

    private PlayerCamera cam;

    private void Start() {
        brightnessProfile.TryGetSettings(out exposure);

        cam = FindFirstObjectByType<PlayerCamera>();

        UpdateSens();

        layer = FindFirstObjectByType<Camera>().GetComponent<PostProcessLayer>();
    }

    private void UpdateSens() {
        cam.leftAndRightRotationSpeed = 100 * ((float)PlayerPrefs.GetInt("masterSens") / 8);
        cam.upAndDownRotationSpeed = 100 * ((float)PlayerPrefs.GetInt("masterSens") / 8);

    }

    private void Update() {
        // Checks if escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // if the game is paused, resume it
            if (gamePaused) {
                ResumeGame();
            }
            // if the game is not paused, pause it
            else {
                PauseGame();
            }
        }
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
