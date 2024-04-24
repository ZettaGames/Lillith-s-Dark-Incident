using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
	[SerializeField] private AudioMixer audioMixer;
	
	// Update master volume
	public void SetMasterFXVolume(float volume)
	{
		audioMixer.SetFloat("masterFXVolume", Mathf.Log10(volume) * 20f);
	}
	
	// Update FX volume
	public void SetSoundFXVolume(float volume)
	{
		audioMixer.SetFloat("soundFXVolume", Mathf.Log10(volume) * 20f);
	}
	
	// Update music volume
	public void SetMusicVolume(float volume)
	{
		audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20f);
	}
}