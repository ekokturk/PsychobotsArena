// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeComponent : ARobotComponent
{
    [Header("Melee Attack")]
    [Tooltip("Location where overlap sphere spawns to detect collision")]
    [SerializeField] private Transform  _hitLocation             = null;
    [Tooltip("Radius size of the overlap sphere")]
    [SerializeField] private float      _hitRadius               = 0.4f;
    [Tooltip("Set this to true if the force should be relative to target location")]
    [SerializeField] private bool       _isHitDirectionRelative  = false;
    [SerializeField] private Vector3    _hitDirection            = Vector3.zero;   
    [Tooltip("Impulse force that will be applied to the target")]
    [SerializeField] private float      _hitForceMagnitude       = 200f;
    [Tooltip("Layers that this damage will be applied to")]
    [SerializeField] private LayerMask  _hitDamageMask           = 0;
    [Tooltip("Particle effect that will be spawned on hit")]
    [SerializeField] private GameObject _hitParticle             = null;
    [Header("Audio")]
    [SerializeField] private AudioSource _attackSFX = null;
    [SerializeField] private AudioSource _hitSFX    = null;

    private List<GameObject> _overlapTargets = new List<GameObject>();               // List of already collided objects
    
    private Animator _animator  = null;
    private float _particleDestroyTime = 3f;

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
    // Use melee attack
    public override void Use()
    {

        if(_isUsable)
        {
            base.Use();
            _overlapTargets.Clear();
            _currentDuration = 0;
            _animator?.SetTrigger("Attack");
            StartCoroutine(RechargeDelay());
        }
    }
    // Generate hit with overlap sphere
    public void Hit()
    {
        // Get colliders inside the overlay sphere
        Collider[]  hitColliders = Physics.OverlapSphere(_hitLocation.position, _hitRadius, _hitDamageMask);
        if(_attackSFX != null && _attackSFX.isPlaying == false) _attackSFX.Play();
        foreach (Collider collider in hitColliders)                              // Do for each collider
        {
            if(collider.gameObject == gameObject) continue;                      // Do not collide with self
            if(_overlapTargets.Contains(collider.gameObject) == true) continue;  // Do not collide with already collided object
            _overlapTargets.Add(collider.gameObject);                            // Add the collided object to already collided list
            if(collider.CompareTag(gameObject.tag) == false)                     // If it is
            {
                if(_hitParticle)
                {
                    GameObject particle = Instantiate(_hitParticle, _hitLocation.position, transform.rotation);
                    Destroy(particle, _particleDestroyTime);
                }
                collider.GetComponent<HitPoints>()?.AddDamage(_damage);         // Damage target
                Rigidbody rigidbody = collider.GetComponent<Rigidbody>();
                if(rigidbody != null)                                           // Push target back
                {
                    if(_isHitDirectionRelative)
                        rigidbody.AddForce(transform.forward * _hitForceMagnitude, ForceMode.Impulse);      
                    else
                        rigidbody.AddForce(_hitDirection.normalized * _hitForceMagnitude, ForceMode.Impulse);      

                }
            }
        }
        if(_overlapTargets.Count > 0 && _hitSFX != null && _hitSFX.isPlaying == false) _hitSFX.Play();

        DebugExtension.DebugWireSphere(_hitLocation.position, Color.red, _hitRadius);   // Show overlay sphere in viewport

    }

}
