using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
	[SerializeField] private AudioSource soundFXObject;
	
	// Create instance of the manager
	public static SoundFXManager Instance { get; private set; }
	private void Awake()
	{
		if (Instance == null)
		{
			Instance =  this;
			DontDestroyOnLoad(Instance);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	// Play selected sound at a certain position
	public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
	{
		AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
		audioSource.clip = audioClip;
		audioSource.volume = volume;
		audioSource.Play();
		float clipLenght = audioSource.clip.length;
		Destroy(audioSource, clipLenght);
	}
}