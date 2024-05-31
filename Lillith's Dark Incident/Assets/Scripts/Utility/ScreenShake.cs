using UnityEngine;

public class ScreenShake : MonoBehaviour
{
	private float _timeLeft;
	private float _intensity;
	private float _time;
	private float _rotation;
	private float _rotationQuantity;
	private float _intensityQuantity;
	
	private Vector3 _initialPosition;
	private bool _isShaking = false;
	
	private static ScreenShake _instance;
	public static ScreenShake Instance { get { return _instance; } }
	
	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = this;
		}
	}
	
	private void LateUpdate()
	{
		if (_isShaking)
		{
			if (_timeLeft > 0)
			{
				_timeLeft -= Time.deltaTime;
				
				float xQuantity = _initialPosition.x + Random.Range(-_intensityQuantity, _intensityQuantity) * _intensity;
				float yQuantity = _initialPosition.y + Random.Range(-_intensityQuantity, _intensityQuantity) * _intensity;
				xQuantity = Mathf.MoveTowards(xQuantity, _initialPosition.x, _time * Time.deltaTime);
				yQuantity = Mathf.MoveTowards(yQuantity, _initialPosition.y, _time * Time.deltaTime);
				
				transform.position = new Vector3(xQuantity, yQuantity, _initialPosition.z);
				
				_rotation = Mathf.MoveTowards(_rotation, 0, _time * _rotationQuantity * Time.deltaTime);
				transform.rotation = Quaternion.Euler(0, 0, _rotation * Random.Range(-1f, 1f));
			}
		}
		else
		{
			transform.position = _initialPosition;
			_isShaking = false;
		}
	}
	
	public void Shake(float time, float intensity)
	{
		_initialPosition = transform.position;
		_isShaking = true;
		_timeLeft = time;
		_intensity = intensity;
		
		_rotation = Random.Range(-_intensity * 0.5f, _intensity * 0.5f);
		
		_time = time / _intensity;
	}
}