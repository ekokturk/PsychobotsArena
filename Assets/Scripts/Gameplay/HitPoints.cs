// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitPoints : MonoBehaviour
{
    [Tooltip("Current hp of the object")]
    [SerializeField] private float      _currentHp          = 10;       // Current hit points
    [Tooltip("Hitpoints will not be affected by damage when this is set to false")]
    [SerializeField] private bool       _invincible         = false;
    [SerializeField] private bool       _isDestroyOnDeath   = true;
    [Header("Events")]
    [Tooltip("Event that will be triggered when hitpoints reach zero")]
    [SerializeField] private UnityEvent _onDeath            = null;     // Death Event 
    [Header("Audio")]
    [Tooltip("Min damage to trigger sound")]
    [SerializeField] private float       _damagedSoundTrigger   = 5f; 
    [SerializeField] private bool        _isPitchRandomized     = true;
    [SerializeField] private AudioSource _damagedSFX            = null;
    [SerializeField] private AudioSource _deathSFX              = null;

    private float      _damageMultiplier   = 1;         // Damage multiplier
    private float      _maxHp = 0;                      // Maximum hit points

     // =============================== GETTER/SETTERS ===================================
    public float    Multiplier          { get{ return _damageMultiplier; }  set{ _damageMultiplier = value; }}
    public bool     Invincible          { get{ return _invincible; }        set{ _invincible = value; }}
    public float    HealthPercentage
    {
        get{ return _currentHp * 100 / _maxHp;  }       // Get health percentage
    }

    // ================================== MONOBEHAVIOUR ====================================
    private void Start()
    {
        _maxHp = _currentHp;                            // Set maximum hit points
        if(_isPitchRandomized == true)
        {
            float pitch = Random.Range(0.8f, 1);
            _damagedSFX.pitch = pitch;        
            _deathSFX.pitch = pitch;
        }
    }

    // ====================================== DAMAGE =======================================
    ///<summary> Reduce hit points according to 'damage' parameter down until zero. </summary>
    public void AddDamage(float damage)
    {
        if(_invincible) return;                         // Don't add damage if character is invincible
        _currentHp -= damage * _damageMultiplier;       // Reduce hit points
        if(damage > _damagedSoundTrigger && _damagedSFX != null && _damagedSFX.isPlaying == false) _damagedSFX.Play();
        if(_currentHp <= 0)                             // When hit points are down to zero
        {
            _currentHp = 0;                             // Set to zero if negative
            Death();                                    // Kill game object
            Destroy(gameObject);
        }
    }

    // ======================================= HEAL =========================================
    ///<summary> Increase hit points up until maximum hit points according to 'hitPoints' parameter </summary>
    public void AddHitPoints(float hitPoints)
    {
        if(_currentHp <= _maxHp && _currentHp > 0)      // Check if gameobject is alive or at max health
        {
            _currentHp += hitPoints;                    // Increase hit points
            if(_currentHp > _maxHp)                     // When you exceed max health
                _currentHp = _maxHp;                    // Set it to max health
        }
    }

    // ======================================= EVENTS =========================================
    // This is used when hit points is set to zero to kill the gameobject
    public void Death()
    {
        if(_deathSFX != null) _deathSFX.Play();
        _onDeath?.Invoke();
        if(_isDestroyOnDeath) Destroy(gameObject);
    }

}
