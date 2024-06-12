using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
	[SerializeField] private AudioSource _soundFXObject;
	
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
		// Create a new audio source for the sound
		AudioSource audioSource = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);
		audioSource.transform.SetParent(transform);
		
		// Set the specifications for the sound
		audioSource.clip = audioClip;
		audioSource.volume = volume;
		audioSource.Play();
		
		// Destroy the audio source after the clip has finished playing
		float clipLenght = audioSource.clip.length;
		Destroy(audioSource.gameObject, clipLenght);
	}
}