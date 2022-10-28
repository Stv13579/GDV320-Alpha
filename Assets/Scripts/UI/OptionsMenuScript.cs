using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsMenuScript : MonoBehaviour
{
	static float soundVolume = 100.0f;
	static float mouseSensitivity = 2;
	static int fov = 90;
	static float musicVolume = 100.0f;
	[SerializeField]
	TextMeshProUGUI soundVolumeText, MusicVolumeText, mouseText, fovText;
	
	[SerializeField]
	Slider soundVolumeSlider, musicVolumeSlider, mouseSlider, fovSlider;
	
	public void UpdateSoundVolume()
	{
		soundVolume = Mathf.RoundToInt(soundVolumeSlider.value);
		soundVolumeText.text = soundVolume.ToString();
		if (AudioManager.GetAudioManager())
		{
			foreach (AudioManager.Sound i in AudioManager.GetAudioManager().GetSounds())
			{
				i.SetAudioSourceAudioVolume(i.GetAudioVolume() * OptionsMenuScript.GetSoundVolume() / 10.0f);
			}
		}
	}
	public void UpdateMusicVolume()
    {
		musicVolume = Mathf.RoundToInt(musicVolumeSlider.value);
		MusicVolumeText.text = musicVolume.ToString();
		if (AudioManager.GetAudioManager())
		{
			foreach (AudioManager.Sound j in AudioManager.GetAudioManager().GetMusics())
			{
				j.SetAudioSourceAudioVolume(OptionsMenuScript.GetMusicVolume() / 100.0f);
			}
		}
	}
	public void UpdateSensitivity()
	{
		mouseSensitivity = mouseSlider.value;
		mouseText.text = mouseSensitivity.ToString("f1");
		if (PlayerLook.GetPlayerLook())
		{
			PlayerLook.GetPlayerLook().SetSensitivity(mouseSensitivity);
		}
	}
	
	public void UpdateFOV()
	{
		fov = Mathf.RoundToInt(fovSlider.value);
		fovText.text = fov.ToString();
		if (PlayerLook.GetPlayerLook())
		{
			PlayerLook.GetPlayerLook().SetFOV(fov);
		}
		if (PlayerMovement.GetPlayerMovement())
		{
			PlayerMovement.GetPlayerMovement().SetFOV(fov);
		}
	}

    public static float GetSoundVolume()
	{
		return soundVolume;
	}
	public static float GetMusicVolume()
    {
		return musicVolume;
    }
	public static float GetSensitivity()
	{
		return mouseSensitivity;
	}
	
	public static int GetFOV()
	{
		return fov;
	}
	
	public void SaveSettings()
	{
		PlayerPrefs.SetFloat("SoundVolume", soundVolume);
		PlayerPrefs.SetFloat("MusicVolume", musicVolume);
		PlayerPrefs.SetFloat("Sensitivity", mouseSensitivity);
		PlayerPrefs.SetInt("FOV", fov);
		PlayerPrefs.Save();
	}
	
	public void LoadSettings()
	{
		if(PlayerPrefs.HasKey("SoundVolume"))
		{
			soundVolume = PlayerPrefs.GetFloat("SoundVolume");
		}
		if (PlayerPrefs.HasKey("MusicVolume"))
		{
			musicVolume = PlayerPrefs.GetFloat("MusicVolume");
		}
		if (PlayerPrefs.HasKey("Sensitivity"))
		{
			mouseSensitivity = PlayerPrefs.GetFloat("Sensitivity");
		}
		if(PlayerPrefs.HasKey("FOV"))
		{
			fov = PlayerPrefs.GetInt("FOV");
		}
		
		soundVolumeSlider.value = soundVolume;
		musicVolumeSlider.value = musicVolume;
		mouseSlider.value = mouseSensitivity;
		fovSlider.value = fov;
	}
}
