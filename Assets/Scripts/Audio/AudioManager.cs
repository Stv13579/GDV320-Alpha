using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

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

    public Sound[] sounds;

    public Sound[] Musics;
    public string initalMusic;
    
    [SerializeField]
    float audioDistance;
    
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (Sound i in sounds) // loop through the sounds
        {
            i.audioSource = gameObject.AddComponent<AudioSource>();
            i.audioSource.clip = i.clip;

            i.audioSource.volume = i.volume;
            i.audioSource.pitch = i.pitch;

            i.audioSource.loop = i.loop;
        }

        foreach (Sound i in Musics) // loop through the sounds
        {
            i.audioSource = gameObject.AddComponent<AudioSource>();
            i.audioSource.clip = i.clip;

            i.audioSource.volume = i.volume;
            i.audioSource.pitch = i.pitch;

            i.audioSource.loop = i.loop;
        }

        PlayMusic(initalMusic);
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
    public void FadeInAndOutMusic(string fadeIn, string fadeOut, int State)
    {
        Sound soundFadeIn = Array.Find(Musics, item => item.name == fadeIn);
        Sound soundFadeOut = Array.Find(Musics, item => item.name == fadeOut);

        if (State == 0)
        {
            return;
        }
        //state 1 is fade out music
        if (State == 1)
        {
            soundFadeOut.audioSource.volume -= 0.01f * Time.deltaTime;
            //sounds[audioOut].audioSource.volume -= 0.01f * Time.deltaTime;
            // stops the music and then changes to state 2 and resets the audio volume
            if (soundFadeOut.audioSource.volume <= 0)
            {
                soundFadeOut.audioSource.Stop();
                State = 2;
                soundFadeOut.audioSource.volume = 0.1f;
                soundFadeIn.audioSource.volume = 0.0f;
            }
        }
        // state 2 is fade in music
        // plays the music thats at volume 0 and increase the volume to the max volume
        // after it changes to a different state to stop the fade in and out
        if (State == 2)
        {
            soundFadeIn.audioSource.Play();
            soundFadeIn.audioSource.volume += 0.01f * Time.deltaTime;
            if (soundFadeIn.audioSource.volume >= 0.1f)
            {
                State = 0;
                soundFadeIn.audioSource.volume = 0.1f;
            }
        }
    }
}
