using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    [Header("Settings Menu")]
    [SerializeField] private Button _fullScreenButton;
    [SerializeField] private Toggle _fullScreenToggle;
    [SerializeField] private Button _vSyncButton;
    [SerializeField] private Toggle _vSyncToggle;


    [Header("Input Actions")]
    [SerializeField] private InputActionAsset _inputActions;

    private void Start()
    {
        _fullScreenToggle.isOn = PlayerPrefs.GetInt("FullScreen", 1) == 1;
        _vSyncToggle.isOn = PlayerPrefs.GetInt("VSync", 1) == 1;
    }

    public void FullScreen()
    {
        // Check if the game is in full screen
        if (Screen.fullScreen)
        {
            PlayerPrefs.SetInt("FullScreen", 0);
            Debug.Log("Windowed");
            // Set the game to windowed mode
            Screen.fullScreen = false;
            _fullScreenToggle.isOn = false;
        }
        else
        {
            PlayerPrefs.SetInt("FullScreen", 1);
            Debug.Log("Full Screen");
            // Set the game to full screen
            Screen.fullScreen = true;
            _fullScreenToggle.isOn = true;
        }
    }

    public void VSync()
    {
        // Check if VSync is enabled
        if (QualitySettings.vSyncCount == 0)
        {
            PlayerPrefs.SetInt("VSync", 1);
            Debug.Log("VSync Enabled");
            // Enable VSync
            GameManager.Instance.SetFrameRate(true);
            _vSyncToggle.isOn = true;
        }
        else
        {
            PlayerPrefs.SetInt("VSync", 0);
            Debug.Log("VSync Disabled");
            // Disable VSync
            GameManager.Instance.SetFrameRate(false);
            _vSyncToggle.isOn = false;
        }
    }
}