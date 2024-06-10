using UnityEngine;

public class BackgroundOffsetController : MonoBehaviour
{
	[SerializeField] private float scrollSpeed;
	private Vector2 offset;
	
	private Material material;
	
	private void Start()
	{
		material = GetComponent<SpriteRenderer>().material;
	}
	
	private void Update()
	{
		offset.y = scrollSpeed * Time.deltaTime * LocalTime.TimeScale;
		material.mainTextureOffset += offset;
		
		if (material.mainTextureOffset.y > 1)
		{
			material.mainTextureOffset = new Vector2(0, 0);
		}
	}
}