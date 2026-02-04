using System.Collections;
using UnityEngine;

public class BackgroundOffsetController : MonoBehaviour
{
	// ! This script is used to scroll the background texture of the level.
	[SerializeField] private float _scrollSpeed;
	private float _scrollSpeedMultiplier = 1.4f;
    private Vector2 _offset;
    private bool _isScrolling = true;

    // ! Material of the sprite renderer.
    private Material _material;
	
	private void Start()
	{
		// Assignment of the material.
		_material = GetComponent<SpriteRenderer>().material;
	}
	
	private void Update()
	{
        if (!_isScrolling)
        {
            _scrollSpeed -= 0.1f * Time.deltaTime * LocalTime.TimeScale;

            if (_scrollSpeed <= 0)
            {
                _scrollSpeed = 0;
                this.enabled = false;
            }
        }

        // Scroll the background down.
        _offset.y = _scrollSpeed * Time.deltaTime * LocalTime.TimeScale;
        _material.mainTextureOffset += _offset;

        // Reset the texture offset every time it reaches the end.
        if (_material.mainTextureOffset.y > 1)
        {
            _material.mainTextureOffset = new Vector2(0, 0);
        }
    }

	public void ApplyMultiplier()
	{
        _scrollSpeed *= _scrollSpeedMultiplier;
    }

    public void StopScrolling()
    {
        Debug.Log("Stop scrolling");

        _isScrolling = false;
    }
}