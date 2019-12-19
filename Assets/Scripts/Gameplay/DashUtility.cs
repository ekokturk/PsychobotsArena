// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class DashUtility : ARobotUtility
{
    [Header("Dash Configuration")]
    [Tooltip("Multiplies speed of the robot with this value (Movement Speed * Speed Multiplier)")]
    [SerializeField] [Range(1,4)] private float   _speedMultiplier          = 1.2f;
    [Tooltip("Particle effect that will spawn for dash")]
    [SerializeField] private GameObject           _dashParticleEffect       = null;
    [Tooltip("Location where the particle effect will spawn")]
    [SerializeField] private Transform            _dashParticleTransform    = null;
    [Tooltip("Delay between each particle spawn")]
    [SerializeField] private float                _particleSpawnInterval    = 0.5f;

    private float       _defaultMultiplier  = 1.0f;
    private float       _particleLifetime   = 3f;
    private Coroutine   _particleCoroutine  = null;
    private bool        _canSpawnParticle   = true;

    public override void Activate()
    {
        base.Activate();
        if(_isUsable && _duration != 0)
        {
            if(_canSpawnParticle == true)
                _particleCoroutine = StartCoroutine(SpawnParticle());
            GetComponent<PlayerController>().SpeedMultiplier = _speedMultiplier;
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        StopCoroutine(_particleCoroutine);
        GetComponent<PlayerController>().SpeedMultiplier = _defaultMultiplier;
        _canSpawnParticle = true;

    }

    private IEnumerator SpawnParticle()
    {
        _canSpawnParticle = false;
        yield return new WaitForSeconds(_particleSpawnInterval);
        GameObject particle = Instantiate(_dashParticleEffect, 
                                          _dashParticleTransform.position,
                                          _dashParticleTransform.rotation);
        particle.transform.parent = transform;
        Destroy(particle, _particleLifetime);
        _canSpawnParticle = true;

    }
}
