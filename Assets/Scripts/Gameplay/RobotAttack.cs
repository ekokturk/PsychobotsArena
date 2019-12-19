// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HitPoints))]
[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
///<summary>Basic melee attack of a robot body</summary>
public class RobotAttack : MonoBehaviour
{
    [Tooltip("This is where overlap sphere for collision detection will spawn")]
    [SerializeField] private Transform          _part               = null;         // Overlap Sphere location on the gameobject
    [Tooltip("Radius of the overlap sphere that will detect colliders")]
    [SerializeField] private float              _hitRadius          = 0.3f;          // Hit sphere size
    [Space(10)]
    [Tooltip("Damage that will be applied to the target")]
    [SerializeField] private int                _hitDamage          = 1;             // damage of the attack
    [Tooltip("Hit force that will be applied to the target")]
    [SerializeField] private float              _hitForce           = 0;             // Force effect that is applied to the target
    [Tooltip("Verticality of the hit force (from 0 to 45 degrees)")]
    [SerializeField][Range(0,1)] private float  _forceVerticality   = 0;             //
    [Space(10)]
    [Tooltip("Force that will be applied to the object itself")]
    [SerializeField] private float              _selfImpulse        = 20f;          
    [Space(10)]
    [Tooltip("Layers that the damage is applied to")]
    [SerializeField] private LayerMask          _damageMask     = 0;                 // Collision mask
    [Tooltip("Particle effect that will spawn on the hit location")]
    [SerializeField] private GameObject         _hitParticle    = null;              // Collision particle
    [Header("Audio")]
    [SerializeField] private AudioSource _attackSFX     = null;
    [SerializeField] private AudioSource _hitSFX        = null;

    private List<GameObject> _overlapTargets = new List<GameObject>();               // List of already collided objects
    
    private float _particleDestroyTime = 5.0f;                                       // Delay to destroy particle gameobject

    // ===================================== ATTACK =====================================

    // Create a overlap sphere and strike enemies that are inside of it
    public void Attack()
    {
        // Get colliders inside the overlay sphere
        Collider[]  hitColliders = Physics.OverlapSphere(_part.position, _hitRadius, _damageMask);   
        if(_attackSFX != null && _attackSFX.isPlaying == false) _attackSFX.Play();
        foreach (Collider collider in hitColliders)                              // Do for each collider
        {
            if(collider.gameObject == gameObject) continue;                      // Do not repeat for the same gameobject
            if(_overlapTargets.Contains(collider.gameObject)) continue;          // Do not collide with already collided object
            _overlapTargets.Add(collider.gameObject);                            // Add the collided object to already collided list
            if(!collider.CompareTag(gameObject.tag))                             // If it is
            {
                if(_hitParticle)
                {
                    GameObject particle = Instantiate(_hitParticle, _part.position, transform.rotation);
                    Destroy(particle, _particleDestroyTime);
                }
                collider.GetComponent<HitPoints>()?.AddDamage(_hitDamage);       // Damage target
                if(_hitSFX != null && _hitSFX.isPlaying == false) _hitSFX.Play();
                if(collider.GetComponent<Rigidbody>() )                          // Push target back
                    collider.GetComponent<Rigidbody>()?.AddForce((transform.forward + new Vector3(0, _forceVerticality, 0)).normalized 
                                                                  * _hitForce, ForceMode.Impulse);
                
            }
        }
        gameObject.GetComponent<Rigidbody>()?.AddForce(transform.forward * _selfImpulse);      
        DebugExtension.DebugWireSphere(_part.position, Color.red, _hitRadius);   // Show overlay sphere in viewport
    }

    // ===================================== OTHER =====================================
    // Clear already collided objects list
    public void Clear()
    {
        _overlapTargets.Clear();
    }
}
