using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsMenuScript : MonoBehaviour
{
	static int volume;
	static float mouseSensitivity;
	static int fov;
	
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
}
