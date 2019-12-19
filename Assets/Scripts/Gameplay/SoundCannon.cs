// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCannon : ARobotComponent
{
    [Header("Sound Cannon Configuration")]
    [Tooltip("Location where the sound blast will spawn")]
    [SerializeField] private Transform  _endPoint        = null;
    [Tooltip("Projectile object that will spawn")]
    [SerializeField] private GameObject _soundBullet     = null;
    [Tooltip("Speed of the projectile object")]
    [SerializeField] private float      _bulletSpeed     = 100f;
    [Tooltip("Delay before bullet is destroyed")]
    [SerializeField] private float      _bulletLifetime  = 5f;

    private Animator _animator = null;
    // ================================== MONOBEHAVIOUR ====================================

    private void Start() 
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        GoRecharge();
        SetRechargedUsability();
    }

    // ================================== FUNCTIONALITY ====================================
    // Use sound cannon if it is usable
    public override void Use()
    {
        if(_isUsable)
        {
            base.Use();
            _currentDuration = 0;
            _isRecharging = false;                  // Stop rechargin
            
            FireBullet();                           // Spawn fire particle
            StartCoroutine(RechargeDelay());        // Start recharge delay
        }
        
    }
    // Fire a flame particle
    private void FireBullet()
    {
        if(_animator != null) _animator.SetTrigger("Attack");
        GameObject bullet = Instantiate(_soundBullet, _endPoint.position, _endPoint.rotation);
        bullet?.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * _bulletSpeed, ForceMode.Impulse);
        Destroy(bullet.gameObject, _bulletLifetime);
    }


}
