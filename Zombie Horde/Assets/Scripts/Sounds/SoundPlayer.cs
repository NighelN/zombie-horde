using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class SoundPlayer : MonoBehaviour
{
    public static SoundPlayer instance;

    [SerializeField] private AudioMixerGroup mixer;
    [SerializeField] List<AudioSource> audioSources = new List<AudioSource>();
    [SerializeField] List<AudioClip> sounds = new List<AudioClip>();

    void Start()
    {
        instance = this;
    }

    private void Update()
    {
        //Loops though all the possible audio sources
        foreach (var audioSource in audioSources)
        {
            //Loops though all the possible clips and checks if they match the audio source and the audio source isn't playing nymore
            foreach (var clip in sounds.Where(clip => audioSource.clip.Equals(clip) && !audioSource.isPlaying))
            {
                sounds.Remove(clip);
                break;
            }
        }
    }

    /// <summary>
    /// Handles playing a sound effect or song using the Sounds enum
    /// </summary>
    /// <param name="sound">One of the possibities from the Sounds enum</param>
    public void PlaySound(Sounds sound)
    {
        //Grabs the clip you are trying to play
        var clip = Resources.Load($"Sounds/{sound.ToString()}") as AudioClip;
        //Adds the sound to the list of playable sounds
        sounds.Add(clip);
        
        //Checks if there are not enough audio sources
        if (sounds.Count > audioSources.Count)
        {
            //Creates an new audio source on the game manager and adds them to the audio source list
            var newSource = GameManager.instance.gameObject.AddComponent<AudioSource>();
            audioSources.Add(newSource);
            //Sets the clip and plays it
            newSource.clip = clip;
            newSource.Play();
        }
        else
        {
            //Grabs the available audio source
            var source = audioSources[sounds.Count - 1];
            //Sets the clip and plays the clip
            source.clip = clip;
            source.Play();
        }
    }
}