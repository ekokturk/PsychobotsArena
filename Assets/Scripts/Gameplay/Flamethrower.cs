// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : ARobotComponent
{
    [Header("Flamethrower")]
    [Tooltip("Location where flame particles will spawn")]
    [SerializeField] private Transform  _endPoint            = null;
    [Tooltip("Particle object for the flame")]
    [SerializeField] private GameObject _flameParticle       = null;
    [Tooltip("Delay between each particle spawn")]
    [SerializeField] private float      _particleSpawnDelay  = 0.5f;
    [Tooltip("Timer for detaching the particle effect from the parent")]
    [SerializeField] private float      _particleDetachDelay = 0.5f;
    [Header("Audio")]
    [SerializeField] private AudioSource _flameSFX  = null;

    private bool        _canFire              = true;
    private float       _particleDestroyDelay = 3f;
    private Coroutine   _rechargeCoroutine    = null;

    // ================================== MONOBEHAVIOUR ====================================

    private void Update()
    {
        GoRecharge();
        SetRechargedUsability();
        
    }

    // ================================== FUNCTIONALITY ====================================
    // Use flamethrower if it is usable
    public override void Use()
    {
        if(_isUsable)
        {
            UseRecharged();                                                         // Spend recharged duration
            base.Use();

            if(_rechargeCoroutine != null  )  StopCoroutine(_rechargeCoroutine);    // Stop recharge delay
            _isRecharging = false;                                                  // Stop rechargin
            if(_flameSFX != null && _flameSFX.isPlaying == false) _flameSFX.Play();
            FireParticle();                                                         // Spawn fire particle
            _rechargeCoroutine = StartCoroutine(RechargeDelay());                   // Start recharge delay
        }
        else if(_isUsable == true && _flameSFX != null && _flameSFX.isPlaying == true) _flameSFX.Stop();
    }
    
    // Fire a flame particle
    private void FireParticle()
    {
        if(_canFire)
        {
            GameObject particle = Instantiate(_flameParticle, _endPoint.position, _endPoint.rotation);
            particle.transform.parent = gameObject.transform;
            DamageOnTrigger triggerDamage = particle.GetComponent<DamageOnTrigger>();
            if(triggerDamage) particle.GetComponent<DamageOnTrigger>().Damage = _damage;
            particle.tag = gameObject.tag;
            StartCoroutine( DetachParticle(particle) );
            StartCoroutine( ParticleDelay() );
            Destroy(particle, _particleDestroyDelay);
        }
    }

    // ================================== DELAYS ====================================
    
    private IEnumerator ParticleDelay()
    {
        _canFire = false;
        yield return new WaitForSeconds(_particleSpawnDelay);
        _canFire = true;
    }

    private IEnumerator DetachParticle(GameObject particle)
    {
        yield return new WaitForSeconds(_particleDetachDelay);
        if(particle != null) 
        {
            particle.transform.parent = null;
            // ParticleSystem.MainModule main = particle.GetComponent<ParticleSystem>().main;
            // main.simulationSpace = ParticleSystemSimulationSpace.Local;
        }
    }

    


}
