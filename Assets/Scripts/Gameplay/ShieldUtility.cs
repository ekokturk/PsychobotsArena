// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HitPoints))]
public class ShieldUtility : ARobotUtility
{
    [Header("Shield Configuration")]
    [Tooltip("Shield object which visually represents damage reduction")]
    [SerializeField] private GameObject             _shield             = null;
    [Tooltip("Damage reduction that shield provides")]
    [SerializeField] [Range(0,1)] private float     _damageReduction    = 0.5f;

    private float   _defaultMultiplier   = 1.0f;


    public override void Activate()
    {
        base.Activate();
        if(_isUsable && _duration != 0)
        {
            _shield.SetActive(_isActive);
            _defaultMultiplier = GetComponent<HitPoints>().Multiplier;
            GetComponent<HitPoints>().Multiplier = _damageReduction;
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        _shield.SetActive(_isActive);
        GetComponent<HitPoints>().Multiplier = _defaultMultiplier;

    }
}
