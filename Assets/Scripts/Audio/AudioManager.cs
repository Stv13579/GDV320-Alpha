using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using JetBrains.Annotations;

public class AudioManager : MonoBehaviour
{
	static AudioManager currentAudioManager;
	
    [System.Serializable]
    public class Sound
    {
        // name of the audio clip
        [SerializeField]
        string name;

        // audio clip
        [SerializeField]
        AudioClip clip;

        // volume
        [SerializeField]
        [Range(0.0f, 1.0f)]
        float volume;

        // pitch
        [SerializeField]
        [Range(0.1f, 3.0f)]
        float pitch;

        // loop
        [SerializeField]
        bool loop;

        [HideInInspector]
        AudioSource audioSource;

        public string GetAudioName() { return name; }
        public void SetAudioName(string tempName) { name = tempName; }
        public AudioClip GetAudioClip() { return clip; }
        public void SetAudioClip(AudioClip tempAudioClip) { clip = tempAudioClip; }
        public float GetAudioVolume() { return volume; }
        public void SetAudioVolume(float tempVolume) { volume = tempVolume; }
        public float GetAudioPitch() { return pitch; }
        public void SetAudioPitch(float tempAudioPitch) { pitch = tempAudioPitch; }
        public bool GetAudioLoop() { return loop; }
        public void SetAudioLoop(bool tempAudioLoop) { loop = tempAudioLoop; }


        public AudioSource GetAudioSource() { return audioSource; }
        public void SetAudioSource(AudioSource tempAudioSource) { audioSource = tempAudioSource; }
        public void SetAudioSourceAudioClip(AudioClip tempAudioClip) { audioSource.clip = tempAudioClip; }
        public void SetAudioSourceAudioVolume(float tempVolume) { audioSource.volume = tempVolume; }
        public void SetAudioSourceAudioPitch(float tempAudioPitch) { audioSource.pitch = tempAudioPitch; }
        public void SetAudioSourceAudioLoop(bool tempAudioLoop) { audioSource.loop = tempAudioLoop; }
    }

    [SerializeField]
    private Sound[] sounds;
    [SerializeField]
    private Sound[] Musics;

    [SerializeField]
    private string initialMusic;

    [SerializeField]
    private float audioDistance;

    private bool isMusicMuted;

    private bool isSoundMuted;

    public string GetInitialMusic() { return initialMusic; }
    [SerializeField]
    public Sound[] GetMusics() { return Musics; }
    public Sound[] GetSounds() { return sounds; }

    public bool GetIsMusicMuted() { return isMusicMuted; }
    public void SetIsMusicMuted(bool tempIsMusicMuted) { isMusicMuted = tempIsMusicMuted; }

    public bool GetIsSoundMuted() { return isSoundMuted; }
    public void SetIsSoundMuted(bool tempIsSoundMuted) { isSoundMuted = tempIsSoundMuted; }

    private enum FadeState
    {
        Idle,
        fadeOutAudio1,
        fadeInAudio2,
        fadeOutAudio2,
        fadeInAudio1
    }

    private FadeState currentState = FadeState.Idle;
    public void SetCurrentStateToIdle() { currentState = FadeState.Idle; }
    public void SetCurrentStateToFadeOutAudio1() { currentState = FadeState.fadeOutAudio1; }
    public void SetCurrentStateToFadeInAudio2() { currentState = FadeState.fadeInAudio2; }
    public void SetCurrentStateToFadeOutAudio2() { currentState = FadeState.fadeOutAudio2; }
    public void SetCurrentStateToFadeInAudio1() { currentState = FadeState.fadeInAudio1; }
    private void Awake()
    {
        currentAudioManager = this;
    }
    // Start is called before the first frame update
    void Start()
	{
        foreach (Sound i in sounds) // loop through the sounds
        {
            i.SetAudioSource(gameObject.AddComponent<AudioSource>());
            i.SetAudioSourceAudioClip(i.GetAudioClip());
            i.SetAudioSourceAudioVolume(i.GetAudioVolume());
            i.SetAudioSourceAudioPitch(i.GetAudioPitch());
            i.SetAudioSourceAudioLoop(i.GetAudioLoop());
        }

        foreach (Sound j in Musics) // loop through the sounds
        {
            j.SetAudioSource(gameObject.AddComponent<AudioSource>());
            j.SetAudioSourceAudioClip(j.GetAudioClip());
            j.SetAudioSourceAudioVolume(j.GetAudioVolume());
            j.SetAudioSourceAudioPitch(j.GetAudioPitch());
            j.SetAudioSourceAudioLoop(j.GetAudioLoop());
        }
        if(isMusicMuted)
        {

        }
        else
        {
            PlayMusic(initialMusic);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(string soundName, Transform playerPos = null, Transform enemyPos = null) // play sound 
    {
        Sound s = Array.Find(sounds, item => item.GetAudioName() == soundName);

        if (s == null) // if no sound, dont try play one 
        {
            Debug.LogWarning("Sound: " + name + " was not found!"); // error message
            return;
        }
        if (s.GetAudioSource() == null) // if no sound, dont try play one 
        {
            Debug.LogWarning("Sound source: " + name + " was not found!"); // error message
            return;
        }

        if (isSoundMuted)
        {

        }
        else
        {
            if (playerPos != null && enemyPos != null)
            {
                float positionDistance = Vector3.Distance(playerPos.position, enemyPos.position);
                s.GetAudioSource().volume = (1 - (positionDistance / audioDistance)) > 0 ? (1 - (positionDistance / audioDistance)) * 0.1f : 0;
            }
        }
        if (!s.GetAudioSource().isPlaying)
        {
            s.GetAudioSource().Play();
        }
    }


    public void StopSFX(string soundName) // play sound 
    {
        Sound s = Array.Find(sounds, item => item.GetAudioName() == soundName);

        if (s == null) // if no sound, dont try play one 
        {
            Debug.LogWarning("Sound: " + name + " was not found!"); // error message
            return;
        }
        if (s.GetAudioSource() == null) // if no sound, dont try play one 
        {
            Debug.LogWarning("Sound source: " + name + " was not found!"); // error message
            return;
        }

        s.GetAudioSource().Stop();

    }

    public void PlayMusic(string soundName) // play sound 
    {
        Sound s = Array.Find(Musics, item => item.GetAudioName() == soundName);

        if (s == null) // if no sound, dont try play one 
        {
            Debug.LogWarning("Sound: " + name + " was not found!"); // error message
            return;
        }
        if (!s.GetAudioSource().isPlaying)
        {
            s.GetAudioSource().Play();
        }
    }


    public void StopMusic(string soundName) // play sound 
    {
        Sound s = Array.Find(Musics, item => item.GetAudioName() == soundName);

        if (s == null) // if no sound, dont try play one 
        {
            Debug.LogWarning("Sound: " + name + " was not found!"); // error message
            return;
        }

        s.GetAudioSource().Stop();

    }

    // takes in the string of the audio that you want to fade in
    // takes in the string of the audio that you want to fade out
    // takes in an int which changes state
    public void FadeOutAndPlayMusic(string fadeIn, string fadeOut)
    {
        Sound soundFadeIn = Array.Find(Musics, item => item.GetAudioName() == fadeIn);
        Sound soundFadeOut = Array.Find(Musics, item => item.GetAudioName() == fadeOut);

        switch (currentState)
        {
            case FadeState.Idle:
                {
                    if (isMusicMuted)
                    {

                    }
                    else
                    {
                        foreach (Sound i in Musics)
                        {
                            soundFadeIn.GetAudioSource().volume = i.GetAudioVolume();
                            soundFadeOut.GetAudioSource().volume = i.GetAudioVolume();
                        }
                    }
                    break;
                }
            case FadeState.fadeOutAudio1:
                {
                    if (!soundFadeOut.GetAudioSource().isPlaying || isMusicMuted)
                    {
                        currentState = FadeState.Idle;
                        break;
                    }
                    soundFadeOut.GetAudioSource().volume -= 0.05f * Time.deltaTime;
                    if (soundFadeOut.GetAudioSource().volume <= 0.0f)
                    {
                        currentState = FadeState.fadeInAudio2;
                    }
                    break;
                }
            case FadeState.fadeInAudio2:
                {
                    if(isMusicMuted)
                    {
                        currentState = FadeState.Idle;
                        break;
                    }
                    soundFadeIn.GetAudioSource().Play();
                    soundFadeOut.GetAudioSource().Stop();

                    currentState = FadeState.Idle;
                    break;
                }
            case FadeState.fadeOutAudio2:
                {
                    if (!soundFadeIn.GetAudioSource().isPlaying || isMusicMuted)
                    {
                        currentState = FadeState.Idle;
                        break;
                    }
                    soundFadeIn.GetAudioSource().volume -= 0.05f * Time.deltaTime;
                    if (soundFadeIn.GetAudioSource().volume <= 0.0f)
                    {
                        currentState = FadeState.fadeInAudio1;
                    }
                    break;
                }
            case FadeState.fadeInAudio1:
                {
                    if (isMusicMuted)
                    {
                        currentState = FadeState.Idle;
                        break;
                    }
                    soundFadeOut.GetAudioSource().Play();
                    soundFadeIn.GetAudioSource().Stop();

                    currentState = FadeState.Idle;
                    break;
                }
        }
    }
    
	public static AudioManager GetAudioManager()
	{
		return currentAudioManager;
	}
}
