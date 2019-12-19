// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class ARobotComponent : ARecharge
{
    [Header("Component")]
    [Tooltip("Damage of the component")]
    [SerializeField] protected float  _damage;
    [Tooltip("Component cannot be used when this is set to true")]
    [SerializeField] protected bool   _isDisabled = false;

    // ================================== MONOBEHAVIOUR ====================================

    protected void Awake()
    {
        InitializeRecharge();
    }

    // ================================== COMPONENT FUNCTIONALITY ====================================

    public virtual void Use()
    {
        if( _isDisabled == false ) return;
    }

}
