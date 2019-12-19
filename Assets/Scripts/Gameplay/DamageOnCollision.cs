// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Apply damage and reaction force to the other object on collision. (Damage is applied when collision is over)</summary>
public class DamageOnCollision : MonoBehaviour
{
    [Tooltip("Damage that will be applied on collision")]
    [SerializeField] private float          _damage                 = 1.0f;                     // Damage to be applied to the other object
    [Tooltip("Force that will be applied on collision enter to other gameobject")]
    [SerializeField] private float          _hitForce               = 100f;                     // Reaction force magnitude
    [Tooltip("Force that will be applied on collision enter to this gameobject")]
    [SerializeField] private float          _reactionForce          = 0;                        // Reaction force magnitude
    [Tooltip("Add an offset to normal vector of the force")]
    [SerializeField] private Vector3        _directionOffset        = Vector3.zero;
    [Tooltip("Set it true to destroy damager object on trigger enter")]
    [SerializeField] private bool           _isDestroyedOnCollision = false;                    // Destroy self on collision
    [Header("Audio")]
    [SerializeField] private AudioSource    _hitSFX = null;

    private Rigidbody _rigidbody = null;

    private void OnCollisionEnter(Collision other) 
    {
        Vector3 normal = ((other.transform.position - transform.position) + _directionOffset).normalized;
        Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody>();
        if(otherRigidbody == null) return;
        if(_hitSFX != null) _hitSFX.Play();                                                                     // Play sound
        otherRigidbody.AddForce(normal * _hitForce, ForceMode.Impulse);                                         // Apply reaction force
        if(_rigidbody == null) return;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.AddForce(-normal * _reactionForce, ForceMode.Impulse);                                       // Apply reaction force

    }

    private void OnCollisionExit(Collision other) 
    {
        other.gameObject.GetComponent<HitPoints>()?.AddDamage(_damage);     // Add damage to the other object if it has HP
        if(_isDestroyedOnCollision) Destroy(gameObject);                    // Destroy self on collision
    }

    private void Start() 
    {
        _rigidbody = GetComponent<Rigidbody>();    
    }
}
