// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : ARobotComponent
{
    [Header("Laser Gun")]
    [Tooltip("Representation of laser beam as a cylinder object which will be rescaled")]
    [SerializeField] private Transform  _laserBeam          = null;
    [Tooltip("Width of the laser")]
    [SerializeField] private float      _laserWidth         = 1;     
    [Tooltip("Layers that willl be damaged by the laser")]
    [SerializeField] private LayerMask  _damageMask         = 0;       
    [Header("Particle")]
    [Tooltip("Particle that will be spawned on the hit location")]
    [SerializeField] private GameObject _laserHitParticle   = null;
    [Tooltip("Offset of the particle from the end location")]
    [SerializeField] private float      _particleOffset     = 0;
    [Tooltip("Disable visual representation of the laser after a delay")]
    [SerializeField] private float     _laserResetDelay     = 0.1f;

    [Header("Audio")]
    [SerializeField] private AudioSource _laserSFX = null;

    private Coroutine _laserResetCoroutine    = null;

    private float     _particleEffectDelay    = 0.3f;
    private float     _particleEffectLifetime = 0.5f;
    private bool      _canShootParticle       = true;

    // ================================== MONOBEHAVIOUR ====================================

    private void Update()
    {
        GoRecharge();
        SetRechargedUsability();
    }

    // ================================== FUNCTIONALITY ====================================
    // Use laser gun if it is usable
    public override void Use()
    {
        UseRecharged();                                   // Spend recharged duration
        if(_isUsable)
        {
            if(_laserResetCoroutine != null) StopCoroutine(_laserResetCoroutine);
            base.Use();

            _isRecharging = false;                       // Stop rechargin
            CastLaser();
            StartCoroutine(RechargeDelay());             // Start recharge delay
            _laserResetCoroutine = StartCoroutine(DelayLaserReset());
        }
    }

    // Change the size of the laser beam mesh according to hit distance
    private void CastLaser()
    {
        RaycastHit hit;
        if(Physics.Raycast(_laserBeam.position, _laserBeam.forward, out hit,Mathf.Infinity,_damageMask))
        {
            // Debug.Log(hit.collider.name);
            Debug.DrawLine(_laserBeam.position, hit.point, Color.blue);
            _laserBeam.localScale = new Vector3(_laserWidth,
                                                _laserWidth,
                                                Vector3.Distance(hit.point, _laserBeam.position));
            if(_laserSFX != null && _laserSFX.isPlaying == false) _laserSFX.Play();
            if(hit.collider.gameObject.tag == gameObject.tag) return;
            hit.collider?.GetComponent<HitPoints>()?.AddDamage(_damage*Time.deltaTime);

            SpawnParticles();
        }
    }
    // Reset the size of laser beam mesh
    private void ResetLaser()
    {
        _laserBeam.localScale = new Vector3(0, 0, 0);
        if(_laserSFX != null) _laserSFX.Stop();
    }

    // Spawn particles at the end of the laser
    private void SpawnParticles()
    {
        if(_canShootParticle == true)
        {
            GameObject particle = Instantiate(_laserHitParticle,
                                              _laserBeam.position,
                                              _laserBeam.rotation,
                                              _laserBeam);
            particle.transform.localPosition = new Vector3(0,0,1-_particleOffset);
            Destroy(particle, _particleEffectLifetime);
            StartCoroutine(DelayParticleSpawn());
        }
    }

    // ================================== COROUTINES ====================================
    private IEnumerator DelayLaserReset()
    {
        yield return new WaitForSeconds(_laserResetDelay);
        ResetLaser();
    }

    private IEnumerator DelayParticleSpawn()
    {
        _canShootParticle = false;
        yield return new WaitForSeconds(_particleEffectDelay);
        _canShootParticle = true;

    }
}
