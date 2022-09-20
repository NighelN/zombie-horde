using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetAudioVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [Space]
    [SerializeField] private Slider slider;
    [SerializeField] private string volumeName = "MasterVolume";

    private void OnEnable()
    {
        float volume = 0;
        audioMixer.GetFloat(volumeName, out volume);
        slider.value = volume;
    }

    public void SetVolume()
    {
        audioMixer.SetFloat(volumeName, slider.value);
        PlayerPrefs.SetFloat(volumeName, slider.value);
    }
}
