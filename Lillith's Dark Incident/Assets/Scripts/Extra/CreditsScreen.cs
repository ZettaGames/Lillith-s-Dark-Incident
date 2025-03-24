using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreditsScreen : MonoBehaviour
{
    [Header("Credits")]
    [SerializeField] private RectTransform _creditsPanel;
    [SerializeField] private float _speed;
    private float _doubleSpeed;
    private float _speedToUse;
    private bool _start = false;

    private void Start()
    {
        _speedToUse = _speed;
        _doubleSpeed = _speed * 2;
        StartCoroutine(StartCredits());
    }

    private void Update()
    {
        if (!_start)
        {
            return;
        }

        // Constantly move the credits panel upwards
        _creditsPanel.anchoredPosition += Vector2.up * _speedToUse * Time.deltaTime;

        // If the credits reaches the end (y = 3500), load the main menu
        if (_creditsPanel.anchoredPosition.y >= 3500)
        {
            StartCoroutine(GoMenu());
        }

        // Speed up while the player keeps pressing "Z" or the "A" button
        if (Keyboard.current.zKey.isPressed || (Gamepad.current != null && Gamepad.current.buttonSouth.IsPressed()))
        {
            _speedToUse = _doubleSpeed;
        }
        else
        {
            _speedToUse = _speed;
        }

        // Load the main menu if the player presses "Escape" or the "B" button
        if (Keyboard.current.escapeKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame))
        {
            StartCoroutine(GoMenu());
        }
    }

    private IEnumerator StartCredits()
    {
        yield return new WaitForSeconds(1f);
        _start = true;
    }

    private IEnumerator GoMenu()
    {
        _start = false;
        SceneTransitionManager.Instance.LoadLevel(2);
        yield return null;
    }
}