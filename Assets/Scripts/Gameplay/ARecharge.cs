// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ARecharge : MonoBehaviour
{
    [Header("Recharge Configuration")]
    [Tooltip("Cooldown duration")]
    [SerializeField] protected float                _duration            = 2.0f;
    [Tooltip("How fast cooldown will recharge (duration * Recharge Multiplier)")]
    [SerializeField, Range(0,2)] protected float    _rechargeMultiplier  = 1.0f;
    [Tooltip("At what ration it will be reusable again (0.8: reusable when recharged to 80%)")]
    [SerializeField, Range(0,1)] protected float    _rechargeUseRatio    = 0.5f;
    [Tooltip("Delay after it is used to start recharging again")]
    [SerializeField] protected float                _rechargeDelay       = 0.0f;

    protected float   _currentDuration              = 0;
    protected float   _rechargeCooldownCounter      = 0;
    protected bool    _isActive                     = false;
    protected bool    _isUsable                     = true;
    protected bool    _isRecharging                 = false;

    // =============================== GETTER/SETTERS ===================================

    public float RechargeRatio      { get{ return _currentDuration / _duration;} }
    public float UsabilityRatio     { get{ return _rechargeUseRatio; }}
    public bool  IsUsable           { get{ return _isUsable; }}

    // =============================== GETTER/SETTERS ===================================
    /// <summary> Initialize recharging functionality </summary>
    protected virtual void InitializeRecharge()
    {
        _currentDuration = _duration;       // Initialize maximum duration
    }
    /// <summary> Start charging spent duration </summary>
    protected virtual void GoRecharge()
    {
        if(_isRecharging == true)
            _currentDuration = _currentDuration >= _duration ? _duration  
                                                             :  _currentDuration + Time.deltaTime * _rechargeMultiplier;
    }
    /// <summary> Spend recharged duration </summary>
    protected virtual void UseRecharged()
    {
        _currentDuration = _currentDuration <= 0 ? 0 : _currentDuration - Time.deltaTime;
    }
    /// <summary> Check if recharged for using </summary>
    protected virtual void SetRechargedUsability()
    {
        if(RechargeRatio >= _rechargeUseRatio)  _isUsable = true;
        else if(RechargeRatio < _rechargeUseRatio)  _isUsable = false;
    }

    protected IEnumerator RechargeDelay()
    {
        _isRecharging = false;
        yield return new WaitForSeconds(_rechargeDelay);
        _isRecharging = true;
    }

}
