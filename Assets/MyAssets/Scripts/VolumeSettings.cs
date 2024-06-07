using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private SFXManager sfx;

    private void Start()
    {
        if (PlayerPrefs.HasKey("BGMVolume"))
            LoadVolume();
        else
        {
            SetBGMVolume();
            SetSFXVolume();
        }
    }
    public void SetBGMVolume()
    {
        float volume = BGMSlider.value;
        mixer.SetFloat("BGM", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume()
    {
        sfx.Play((int)SFXManager.Clip.Success);

        float volume = SFXSlider.value;
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolume()
    {
        BGMSlider.value = PlayerPrefs.GetFloat("BGMVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        SetBGMVolume();
        SetSFXVolume();
    }
}
