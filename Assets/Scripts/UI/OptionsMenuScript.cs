using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsMenuScript : MonoBehaviour
{
	static int volume = 100;
	static float mouseSensitivity = 2;
	static int fov = 90;
	
	[SerializeField]
	TextMeshProUGUI volumeText, mouseText, fovText;
	
	[SerializeField]
	Slider volumeSlider, mouseSlider, fovSlider;
	
	public void UpdateVolume()
	{
		volume = Mathf.RoundToInt(volumeSlider.value);
		volumeText.text = volume.ToString();
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
		UpdateVolume();
		UpdateSensitivity();
		UpdateFOV();
	}
	
	public static int GetVolume()
	{
		return volume;
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
		PlayerPrefs.SetInt("Volume", volume);
		PlayerPrefs.SetFloat("Sensitivity", mouseSensitivity);
		PlayerPrefs.SetInt("FOV", fov);
		PlayerPrefs.Save();
	}
	
	public void LoadSettings()
	{
		if(PlayerPrefs.HasKey("Volume"))
		{
			volume = PlayerPrefs.GetInt("Volume");
		}
		if(PlayerPrefs.HasKey("Sensitivity"))
		{
			mouseSensitivity = PlayerPrefs.GetFloat("Sensitivity");
		}
		if(PlayerPrefs.HasKey("FOV"))
		{
			fov = PlayerPrefs.GetInt("FOV");
		}
		
		volumeSlider.value = volume;
		mouseSlider.value = mouseSensitivity;
		fovSlider.value = fov;
	}
}
