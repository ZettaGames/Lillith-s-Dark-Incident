using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
	// Audio mixer reference
	[Header("Audio Mixer")]
	[SerializeField] private AudioMixer _audioMixer;
	
	// Text for the volume percentage
	[Header("Text")]
	[SerializeField] private TMP_Text _masterVolumeText;
	[SerializeField] private TMP_Text _musicVolumeText;
	[SerializeField] private TMP_Text _soundFXVolumeText;

	// Sliders for the volume control
	[Header("Sliders")]
	[SerializeField] private Slider _masterSlider;
	[SerializeField] private Slider _musicSlider;
	[SerializeField] private Slider _soundFXSlider;

	private void Start()
	{
		// Set the sliders to the saved volume
		_masterSlider.value = PlayerPrefs.GetFloat("masterVolume", 0.8f);
		_soundFXSlider.value = PlayerPrefs.GetFloat("soundFXVolume", 0.8f);
		_musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.8f);

        // Set the saved volume for every slider
        SetMasterVolume(_masterSlider.value);
		SetSoundFXVolume(_soundFXSlider.value);
		SetMusicVolume(_musicSlider.value);
	}

	#region volume_controllers
	// Update master volume
	public void SetMasterVolume(float volume)
	{
		// Convert the linear volume to decibels
		float fixedVolume = Mathf.Log10(volume) * 20f;
		
		// Update the text and the audio mixer
		_masterVolumeText.text = ((int)(volume * 100)).ToString();
		_audioMixer.SetFloat("masterVolume", fixedVolume);
		
		// Save the volume to the player preferences
		PlayerPrefs.SetFloat("masterVolume", volume);
	}

	// Update FX volume
	public void SetSoundFXVolume(float volume)
	{
		// Convert the linear volume to decibels
		float fixedVolume = Mathf.Log10(volume) * 20f;
		
		// Update the text and the audio mixer
		_soundFXVolumeText.text = ((int)(volume * 100)).ToString();
		_audioMixer.SetFloat("soundFXVolume", fixedVolume);
		
		// Save the volume to the player preferences
		PlayerPrefs.SetFloat("soundFXVolume", volume);
	}

	// Update music volume
	public void SetMusicVolume(float volume)
	{
		// Convert the linear volume to decibels
		float fixedVolume = Mathf.Log10(volume) * 20f;
		
		// Update the text and the audio mixer
		_musicVolumeText.text = ((int)(volume * 100)).ToString();
		_audioMixer.SetFloat("musicVolume", fixedVolume);
		
		// Save the volume to the player preferences
		PlayerPrefs.SetFloat("musicVolume", volume);
	}
	#endregion
}