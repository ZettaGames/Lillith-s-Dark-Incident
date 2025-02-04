using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _inkSpots = new List<GameObject>();
    [SerializeField] private float _timeToShowInk;
    private float _timer;

    // Instance
    public static InkManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowInkSpot()
    {
        // Pick a random ink spot to show
        int index = Random.Range(0, _inkSpots.Count);

        var spriteRenderer = _inkSpots[index].GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        color.a = 1f; // Set alpha to fully visible
        spriteRenderer.color = color;

        // Take the ink spot off the list
        var inkSpot = _inkSpots[index];

        // Start fading out this ink spot
        StartCoroutine(FadeOutInk(spriteRenderer, inkSpot));
    }

    private IEnumerator FadeOutInk(SpriteRenderer spriteRenderer, GameObject inkSpot)
    {
        // Smoothly fade out the ink spot
        Color color = spriteRenderer.color;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime / _timeToShowInk;
            spriteRenderer.color = color;
            yield return null;
        }

        // Return the ink spot to the list
        _inkSpots.Add(inkSpot);
    }
}