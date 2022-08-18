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

        Play(initalMusic);
        DontDestroyOnLoad(gameObject);
    }

    public void Play(string soundName, Transform playerPos = null, Transform enemyPos = null) // play sound 
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


    public void Stop(string soundName) // play sound 
    {
        Sound s = Array.Find(sounds, item => item.name == soundName);

        if (s == null) // if no sound, dont try play one 
        {
            Debug.LogWarning("Sound: " + name + " was not found!"); // error message
            return;
        }

        s.audioSource.Stop();

    }
    
    // takes in the string of the audio that you want to fade in
    // takes in the string of the audio that you want to fade out
    // takes in ints for the audios in the sounds list
    // takes in an int which changes state
    public void FadeInAndOutMusic(string fadeIn, string fadeOut, int audioIn, int audioOut, int State)
    {
        if (State == 0)
        {
            return;
        }
        //state 1 is fade out music
        if (State == 1)
        {
            sounds[audioOut].audioSource.volume -= 0.01f * Time.deltaTime;
            // stops the music and then changes to state 2 and resets the audio volume
            if (sounds[audioOut].audioSource.volume <= 0)
            {
                Stop(fadeOut);
                State = 2;
                sounds[audioOut].audioSource.volume = 0.1f;
                sounds[audioIn].audioSource.volume = 0.0f;
            }
        }
        // state 2 is fade in music
        // plays the music thats at volume 0 and increase the volume to the max volume
        // after it changes to a different state to stop the fade in and out
        if (State == 2)
        {
            Play(fadeIn);
            sounds[audioIn].audioSource.volume += 0.01f * Time.deltaTime;
            if (sounds[audioIn].audioSource.volume >= 0.1f)
            {
                State = 0;
                sounds[audioIn].audioSource.volume = 0.1f;
            }
        }
    }
}
