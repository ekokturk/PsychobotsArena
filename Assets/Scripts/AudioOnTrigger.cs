// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOnTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource _triggerSFX = null;
    private void OnTriggerEnter(Collider other) 
    {
        if(_triggerSFX != null && _triggerSFX.isPlaying == false)
        {
            _triggerSFX.Play();
        }
    }
}
