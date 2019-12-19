// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioMixer _audioMixer     = null;
    [SerializeField] Slider     _masterSlider   = null;
    [SerializeField] Slider     _musicSlider    = null;
    [SerializeField] Slider     _sfxSlider      = null;

    private void OnEnable() 
    {
        _audioMixer.GetFloat("masterVolume", out float masterLevel);
        _audioMixer.GetFloat("musicVolume", out float musicLevel);
        _audioMixer.GetFloat("sfxVolume", out float sfxLevel);
        if(_masterSlider != null)   _masterSlider.value = Mathf.Pow(10.0f, masterLevel/20.0f);
        if(_musicSlider != null)    _musicSlider.value  = Mathf.Pow(10.0f, musicLevel/20.0f);
        if(_sfxSlider != null)      _sfxSlider.value    = Mathf.Pow(10.0f, sfxLevel/20.0f);
    }

    public void SetMasterVolume(float level)
    {
        _audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        _audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
    }

    public void SetEffectsVolume(float level)
    {
        _audioMixer.SetFloat("sfxVolume", Mathf.Log10(level) * 20f);
    }
}
