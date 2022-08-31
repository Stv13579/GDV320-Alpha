﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using JetBrains.Annotations;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        // name of the audio clip
        public string name;

        // audio clip
        public AudioClip clip;

        // volume
        [Range(0.0f, 1.0f)]
        public float volume;

        // pitch
        [Range(0.1f, 3.0f)]
        public float pitch;

        // loop
        public bool loop;

        [HideInInspector]
        public AudioSource audioSource;
    }
    [SerializeField]
    private Sound[] sounds;
    [SerializeField]
    private Sound[] Musics;

    [SerializeField]
    private string initialMusic;

    [SerializeField]
    private float audioDistance;

    public string GetInitialMusic() { return initialMusic; }
    [SerializeField]
    public Sound[] GetMusics() { return Musics; }

    public bool IsSoundPlaying(string audio) 
    {
        Sound tempAudioList = Array.Find(Musics, item => item.name == audio);

        if(tempAudioList.audioSource.isPlaying)
        {
            return true;
        }
        else
        {
            return false;
        }
            
    }
enum FadeState
    {
        Idle,
        fadeIn,
        fadeOut,
    }
    private FadeState currentState = FadeState.Idle;
    public void SetCurrentStateToIdle() { currentState = FadeState.fadeIn; }
    public void SetCurrentStateToFadeIn() { currentState = FadeState.fadeIn; }
    public void SetCurrentStateToFadeOut() { currentState = FadeState.fadeIn; }
    // Start is called before the first frame update
    private void Start()
    {
        foreach (Sound i in sounds) // loop through the sounds
        {
            i.audioSource = gameObject.AddComponent<AudioSource>();
            i.audioSource.clip = i.clip;

            i.audioSource.volume = i.volume;
            i.audioSource.pitch = i.pitch;

            i.audioSource.loop = i.loop;
        }

        foreach (Sound j in Musics) // loop through the sounds
        {
            j.audioSource = gameObject.AddComponent<AudioSource>();
            j.audioSource.clip = j.clip;

            j.audioSource.volume = j.volume;
            j.audioSource.pitch = j.pitch;

            j.audioSource.loop = j.loop;
        }

        PlayMusic(initialMusic);
        DontDestroyOnLoad(gameObject);
    }
    public void PlaySFX(string soundName, Transform playerPos = null, Transform enemyPos = null) // play sound 
    {
        Sound s = Array.Find(sounds, item => item.name == soundName);

        if (s == null) // if no sound, dont try play one 
        {
            Debug.LogWarning("Sound: " + name + " was not found!"); // error message
            return;
        }
        if (playerPos != null && enemyPos != null)
        {
            float positionDistance = Vector3.Distance(playerPos.position, enemyPos.position);
            s.audioSource.volume = (1 - (positionDistance / audioDistance)) > 0 ? (1 - (positionDistance / audioDistance)) * 0.1f : 0;
        }
        if (!s.audioSource.isPlaying)
        {
            s.audioSource.Play();
        }

    }


    public void StopSFX(string soundName) // play sound 
    {
        Sound s = Array.Find(sounds, item => item.name == soundName);

        if (s == null) // if no sound, dont try play one 
        {
            Debug.LogWarning("Sound: " + name + " was not found!"); // error message
            return;
        }

        s.audioSource.Stop();

    }

    public void PlayMusic(string soundName) // play sound 
    {
        Sound s = Array.Find(Musics, item => item.name == soundName);

        if (s == null) // if no sound, dont try play one 
        {
            Debug.LogWarning("Sound: " + name + " was not found!"); // error message
            return;
        }
        if (!s.audioSource.isPlaying)
        {
            s.audioSource.Play();
        }

    }


    public void StopMusic(string soundName) // play sound 
    {
        Sound s = Array.Find(Musics, item => item.name == soundName);

        if (s == null) // if no sound, dont try play one 
        {
            Debug.LogWarning("Sound: " + name + " was not found!"); // error message
            return;
        }

        s.audioSource.Stop();

    }

    // takes in the string of the audio that you want to fade in
    // takes in the string of the audio that you want to fade out
    // takes in an int which changes state
    public void FadeOutAndPlayMusic(string fadeIn, string fadeOut)
    {
        Sound soundFadeIn = Array.Find(Musics, item => item.name == fadeIn);
        Sound soundFadeOut = Array.Find(Musics, item => item.name == fadeOut);

        if (!soundFadeOut.audioSource.isPlaying || soundFadeOut.audioSource.volume <= 0.00f)
        {
            return;
        }

        switch (currentState)
        {
            case FadeState.Idle:
                {

                    break;
                }
            case FadeState.fadeOut:
                {
                    soundFadeOut.audioSource.volume -= Time.deltaTime;
                    if (soundFadeOut.audioSource.volume <= 0.00f)
                    {
                        currentState = FadeState.fadeIn;
                    }
                    break;
                }
            case FadeState.fadeIn:
                {
                    soundFadeOut.audioSource.volume = 0.05f;
                    soundFadeOut.audioSource.Stop();
                    soundFadeIn.audioSource.Play();
                    currentState = FadeState.Idle;
                    break;
                }
        }
    }
}
