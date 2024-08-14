using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.Rendering.PostProcessing;
using System.Diagnostics;

public class MenuScript : MonoBehaviour
{
    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSliderValue = null;
    [SerializeField] private float volumeScale = 1.0f;

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text sensTextValue = null;
    [SerializeField] private Slider sensSliderValue = null;
    [SerializeField] private int sensIndex = 3;
    public int mainSens = 3;

    [Header("Graphic Settings")]
    [SerializeField] private Slider brightnessSliderValue = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private float brightnessScale = 1;
    public PostProcessProfile brightnessProfile;
    private AutoExposure exposure;


    private int QualityLevel;
    private float brightnessScaleValue;

    [Header("Resolutions Dropdowns")]
    public TMP_Dropdown resolutionsDropdown;
    private Resolution[] resolutions;
    public Toggle fullscreenToggle;

    [Header("New Level")]
    public string newGameLevel;
    private string NewGame;

    public MenuScript(string newGame)
    {
        NewGame = newGame;
    }

    private void Start()
    {
        InitializeSettings();
        InitializeResolutionDropdown();
        brightnessProfile.TryGetSettings(out exposure);
    }

    private void InitializeSettings()
    {
        if (sensSliderValue != null)
        {
            sensSliderValue.value = mainSens;
            sensTextValue.text = mainSens.ToString("0");
        }

        if (volumeSliderValue != null)
        {
            volumeSliderValue.value = volumeScale;
            volumeTextValue.text = volumeScale.ToString("0.0");
        }

        if (brightnessSliderValue != null)
        {
            brightnessSliderValue.value = brightnessScale;
            brightnessTextValue.text = brightnessScale.ToString("0.0");
        }
    }

    private void InitializeResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        resolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentResolutionIndex;
        resolutionsDropdown.RefreshShownValue();
    }

    public void NewGameMessageYES()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void NewGameMessageNO() => Application.Quit();

    public void ExitButton() => Application.Quit();

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    }

    public float GetVolumeScale()
    {
        return volumeScale;
    }

    public void SetGameSens(float sens)
    {
        mainSens = Mathf.RoundToInt(sens);
        sensTextValue.text = mainSens.ToString("0");
    }

    public void GameplayApply()
    {
        PlayerPrefs.SetInt("masterSens", mainSens);
    }

    public void SetBrightness(float brightness)
    {
        brightnessScale = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");

        if (brightnessScale != 0) {
            exposure.keyValue.value = brightness;
        } else {
            // Makes brightness very dark instead of pitch black when slider is set to 0
            exposure.keyValue.value = 0.05f;
        }
    }

    public void SetQuality(int quality)
    {
        QualityLevel = quality;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", brightnessScale);
        PlayerPrefs.SetInt("masterQuality", QualityLevel);
        QualitySettings.SetQualityLevel(QualityLevel);
    }

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = volumeScale;
            volumeSliderValue.value = volumeScale;
            volumeTextValue.text = volumeScale.ToString("0.0");
            VolumeApply();
        }
        else if (MenuType == "Gameplay")
        {
            sensIndex = 3;
            mainSens = sensIndex;
            sensSliderValue.value = sensIndex;
            sensTextValue.text = sensIndex.ToString("0");
            GameplayApply();
        }
        else if (MenuType == "Graphics")
        {
            brightnessScale = 1;
            brightnessSliderValue.value = brightnessScale;
            brightnessTextValue.text = brightnessScale.ToString("0.0");
            GraphicsApply();
        }
    }

    public void ChangeFullscreen() {
        Screen.SetResolution(Screen.width, Screen.height, fullscreenToggle.isOn);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, fullscreenToggle.isOn);
    }
}

