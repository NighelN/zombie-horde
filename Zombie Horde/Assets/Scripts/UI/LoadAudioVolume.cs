using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LoadAudioVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string[] volumeNames;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var volumeName in volumeNames)
        {
            if (PlayerPrefs.HasKey(volumeName))
            {
                audioMixer.SetFloat(volumeName, PlayerPrefs.GetFloat(volumeName));
            }
        }
    }
}
