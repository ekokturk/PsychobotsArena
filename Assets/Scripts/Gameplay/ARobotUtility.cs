// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class ARobotUtility : ARecharge
{
    [Header("Audio")]
    [SerializeField] private AudioSource _utilitySFX  = null;
    // ================================== MONOBEHAVIOUR ====================================
    private void Start()
    {
        InitializeRecharge();
    }

    protected virtual void Update() 
    {
        // ____________RECHARGE____________
        if(_isActive == true)           // Spend utility
        {
            UseRecharged();             
            if(_currentDuration == 0)
            _isActive = false;
        }
        SetRechargedUsability();    // Enable usability after a certain point
        GoRecharge();               // Recharge utility
    }

    // ================================== FUNCTIONALITY ====================================

    public virtual void Activate()
    {
        if(_isUsable == true)
        {
            _isActive = true;
            _isRecharging = false;
            if(_utilitySFX != null && _utilitySFX.isPlaying == false) _utilitySFX.Play();
        }   
    }

    public virtual void Deactivate()
    {
        _isActive = false;
        _isRecharging = true;
        if(_utilitySFX != null && _utilitySFX.isPlaying == true) _utilitySFX.Stop();
    }

}
