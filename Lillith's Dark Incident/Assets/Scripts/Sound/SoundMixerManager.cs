using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
	[SerializeField] private AudioMixer audioMixer;
	
	public void SetMasterFXVolume(float volume)
	{
		audioMixer.SetFloat("masterFXVolume", Mathf.Log10(volume) * 20f);
	}
	
	public void SetSoundFXVolume(float volume)
	{
		audioMixer.SetFloat("soundFXVolume", Mathf.Log10(volume) * 20f);
	}
	
	public void SetMusicVolume(float volume)
	{
		audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20f);
	}
}