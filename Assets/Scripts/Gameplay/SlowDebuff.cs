// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDebuff : MonoBehaviour
{
    private float _slowRatio   = 0.5f;      // How much slow will be applier
    private float _duration    = 2.0f;      // Duration of the slow
    private float _durationCounter = 0;     // Duration Counter

    private PlayerController _playerController = null;  // Player movement controller

     // ================================== MONOBEHAVIOUR ====================================   

    private void Start() 
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void Update() 
    {
        if(_playerController == null) return;
        _playerController.SpeedMultiplier = _slowRatio;
        if(_durationCounter >= _duration) StopDebuff();
        _durationCounter += Time.deltaTime;
    }

    // ================================== SLOW FUNCTIONALITY ====================================
    // Stop Debuff
    private void StopDebuff()
    {
        _playerController.SpeedMultiplier = 1.0f;
        Destroy(this);
    }
    // Reset debuff counter
    public void ResetDebuff(float duration, float slowRatio)
    {
        _duration = duration;
        _durationCounter = 0;
        _slowRatio = slowRatio;
    }

}
