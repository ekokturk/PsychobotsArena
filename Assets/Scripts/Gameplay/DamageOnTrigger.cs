// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Apply damage to the other object while it stays in trigger collider.</summary>
public class DamageOnTrigger : MonoBehaviour
{
    [Tooltip("Damage that will be applied on trigger, indirect damage applies as damage per second")]
    [SerializeField] private float  _damage             = 1.0f;
    [Tooltip("Force that will be applied on trigger enter")]
    [SerializeField] private float  _hitForce           = 200f;
    [Tooltip("Add an offset to normal vector of the force")]
    [SerializeField] private Vector3    _directionOffset  = Vector3.zero;
    [Tooltip("Switch between damage on 'Trigger Enter' or 'Trigger Stay' for instant or constant damage")]
    [SerializeField] private bool   _isDirectDamage      = false;
    [Tooltip("Set it true to destroy damager object on trigger enter")]
    [SerializeField] private bool   _isDestroyOnEnter   = false;
    [Header("Particle On Enter")]
    [SerializeField] private GameObject _hitParticle             = null;
    [Header("Audio")]
    [SerializeField] private AudioSource _hitSFX = null;
    public float Damage { get{ return _damage; } set{ _damage = value; }}

    // Damage other object if it has a different tag and apply force
    private void OnTriggerEnter(Collider other) 
    {
        if(_isDirectDamage == false) return;
        if(_hitSFX != null)
        {
            _hitSFX.Play();
            _hitSFX.gameObject.transform.parent = null;
        }
        if(_hitParticle != null)
        {
            GameObject particle = Instantiate(_hitParticle,transform.position, transform.rotation);
            Destroy(particle, 3);
        }
        if(other.gameObject.tag != gameObject.tag)
        {
            Vector3 normal = ((other.transform.position - transform.position) + _directionOffset).normalized;
            Rigidbody rigidbody = other.gameObject.GetComponent<Rigidbody>();
            if(rigidbody == null) return;
            rigidbody.AddForce(normal * _hitForce, ForceMode.Impulse);                      // Apply reaction force
            other.gameObject.GetComponent<HitPoints>()?.AddDamage(_damage);
        }
        if(_isDestroyOnEnter == true) Destroy(gameObject);  // TODO Might need to change location 
    }

    private void OnTriggerStay(Collider other) 
    {
        if(_isDirectDamage == true) return;
        if(other.gameObject.tag != gameObject.tag)
            other.gameObject.GetComponent<HitPoints>()?.AddDamage(_damage*Time.deltaTime);    
    }


}
