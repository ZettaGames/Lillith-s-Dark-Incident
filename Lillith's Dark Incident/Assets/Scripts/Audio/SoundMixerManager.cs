using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
	[Header("Audio Mixer")]
	[SerializeField] private AudioMixer audioMixer;
	
	[Header("Text")]
	[SerializeField] private TMP_Text masterVolumeText;
	[SerializeField] private TMP_Text soundFXVolumeText;
	[SerializeField] private TMP_Text musicVolumeText;

	[Header("Sliders")]
	[SerializeField] private Slider masterSlider;
	[SerializeField] private Slider soundFXSlider;
	[SerializeField] private Slider musicSlider;

	private void Start()
	{
		// Set the sliders to the saved volume
		masterSlider.value = PlayerPrefs.GetFloat("masterVolume", 0.8f);
		soundFXSlider.value = PlayerPrefs.GetFloat("soundFXVolume", 0.8f);
		musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.8f);

		// Convert the loaded linear volume to decibels
		SetMasterFXVolume(masterSlider.value);
		SetSoundFXVolume(soundFXSlider.value);
		SetMusicVolume(musicSlider.value);
	}

	// Update master volume
	public void SetMasterFXVolume(float volume)
	{
		masterVolumeText.text = ((int)(volume * 100)).ToString();
		float fixedVolume = Mathf.Log10(volume) * 20f;
		audioMixer.SetFloat("masterVolume", fixedVolume);
		PlayerPrefs.SetFloat("masterVolume", volume);
	}

	// Update FX volume
	public void SetSoundFXVolume(float volume)
	{
		soundFXVolumeText.text = ((int)(volume * 100)).ToString();
		float fixedVolume = Mathf.Log10(volume) * 20f;
		audioMixer.SetFloat("soundFXVolume", fixedVolume);
		PlayerPrefs.SetFloat("soundFXVolume", volume);
	}

	// Update music volume
	public void SetMusicVolume(float volume)
	{
		musicVolumeText.text = ((int)(volume * 100)).ToString();
		float fixedVolume = Mathf.Log10(volume) * 20f;
		audioMixer.SetFloat("musicVolume", fixedVolume);
		PlayerPrefs.SetFloat("musicVolume", volume);
	}
}