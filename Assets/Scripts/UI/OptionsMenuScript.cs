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
	TextMeshProUGUI volumeText, mouseText, fovText;
	
	[SerializeField]
	Slider soundVolumeSlider, musicVolumeSlider, mouseSlider, fovSlider;
	
	public void UpdateSoundVolume()
	{
		soundVolume = Mathf.RoundToInt(soundVolumeSlider.value);
		volumeText.text = soundVolume.ToString();
	}
	public void UpdateMusicVolume()
    {
		soundVolume = Mathf.RoundToInt(musicVolumeSlider.value);
		volumeText.text = soundVolume.ToString();
	}
	public void UpdateSensitivity()
	{
		mouseSensitivity = mouseSlider.value;
		mouseText.text = mouseSensitivity.ToString("f1");
	}
	
	public void UpdateFOV()
	{
		fov = Mathf.RoundToInt(fovSlider.value);
		fovText.text = fov.ToString();
		PlayerLook.GetPlayerLook().SetFOV(fov);
		PlayerMovement.GetPlayerMovement().SetFOV(fov);
	}
	
	// Update is called every frame, if the MonoBehaviour is enabled.
	protected void Update()
	{
		UpdateSoundVolume();
		UpdateMusicVolume();
		UpdateSensitivity();
		UpdateFOV();
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
		PlayerPrefs.SetFloat("Volume", soundVolume);
		PlayerPrefs.SetFloat("Volume", musicVolume);
		PlayerPrefs.SetFloat("Sensitivity", mouseSensitivity);
		PlayerPrefs.SetInt("FOV", fov);
		PlayerPrefs.Save();
	}
	
	public void LoadSettings()
	{
		if(PlayerPrefs.HasKey("Volume"))
		{
			soundVolume = PlayerPrefs.GetInt("Volume");
		}
		if (PlayerPrefs.HasKey("Volume"))
		{
			musicVolume = PlayerPrefs.GetInt("Volume");
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
