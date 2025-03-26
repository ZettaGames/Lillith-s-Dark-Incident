using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class FinalManager : MonoBehaviour
{
    // Constant for credits scene
    private const string CreditsScene = "CreditsScene";

    [SerializeField] private TMP_Text _scoreText;

    private void Start()
    {
        var score = GameManager.Instance.TotalScore;
        if (score > PlayerPrefs.GetFloat("TotalScore"))
        {
            PlayerPrefs.SetFloat("TotalScore", score);
        }

        _scoreText.text = $"Your total score was: {score:0000000000}";
    }

    private void Update()
    {
        // Load the credits if any key is pressed
        if (Keyboard.current.anyKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.allControls.Any(control => control is ButtonControl button && button.wasPressedThisFrame)))
        {
            StartCoroutine(LoadCredits());
        }
    }

    private IEnumerator LoadCredits()
    {
        yield return new WaitForSeconds(1.5f);
        SceneTransitionManager.Instance.LoadLevel(CreditsScene);
    }
}
