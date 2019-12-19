// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Spawn")]
    [Tooltip("Indicator gameobject that will be spawned before the actual object is activated to notify players")]
    [SerializeField] private GameObject           _indicatorPrefab     = null;   // Prefab that is visible before activating object
    [Tooltip("Final scale of the indicator that will change over time from the initial scale")]
    [SerializeField] private Vector3              _indicatorFinalScale = new Vector3(0.1f,2,0.1f);
    [Tooltip("Spawned interactable object that will be set to active (This should be an object)")]
    [SerializeField] private GameObject           _spawningObject      = null;   // Interactible object
    [Tooltip("Delay to activate the object")]
    [SerializeField] [Range(0,60)] private float  _spawnTimer          = 5.0f;   // Delay for interactable object respawn
    [Tooltip("Particle effect that will be called when object is spawned")]
    [SerializeField] private GameObject           _particleSpawn       = null;   // Particles that are instantiated during spawn
    [Header("Death")]
    [Tooltip("Time to call death event of the object")]
    [SerializeField] [Range(0,300)] private float _deathTimer          = 20.0f;   // Time that object will be available
    [Tooltip("Delay to destroy the object after death timer is expired from the scene")]
    [SerializeField] private float                _objectDestroyDelay  = 5f;
    [Tooltip("Particle effect for objects death")]
    [SerializeField] private GameObject           _particleDeath       = null;
    [Tooltip("Events that will occur when death timer is expired")]
    [SerializeField] private UnityEvent           _onDeath             = null;

    private GameObject  _indicator          = null;
    private float       _spawnTimeCounter   = 0;
    private bool        _isSpawned          = false;
    private bool        _isDead             = false;

    private float _particleLifetime = 3f;

    // ================================== MONOBEHAVIOUR ====================================
    private void Update() 
    {
        if(_isDead == true) return;
        if(_isSpawned == false) StartCoroutine(DelaySpawn());   // Instantiate object if it is not spawned
        else ResizeIndicatorOvertime();                         // If indicator exists resize it
    }

    // ================================== FUNCTIONALITY ====================================

    private void Instantiate()
    {
        // Spawn particle
        if(_particleSpawn != null)
        {
            GameObject particles = Instantiate(_particleSpawn, transform.position, transform.rotation);
            Destroy(particles, _particleLifetime);
        }
        if(_spawningObject != null)
            _spawningObject.SetActive(true);
    }
    // Destroy 
    private void Destruction()
    {
        _onDeath?.Invoke();
        if(_particleDeath != null)
        {
            GameObject particles = Instantiate(_particleDeath, transform.position, transform.rotation);
            Destroy(particles, _particleLifetime);
        }
        Destroy(gameObject, _objectDestroyDelay); // Destroy object after it's death
    }

    // ============================== INDICATOR METHODS =================================
    // Create spawn indicator
    private void CreateIndicator()
    {
        // Create indicator if it exists
        if(_indicatorPrefab != null && _spawnTimer > 0)
        {
            _indicator = Instantiate(_indicatorPrefab, transform.position, transform.rotation);
            Destroy(_indicator, _spawnTimer);
        }
    }

    // Resize indicator to look like a timer
    private void ResizeIndicatorOvertime()
    {
        // Decrese indicator size 
        if(_indicator == null)  return;
        _spawnTimeCounter += Time.deltaTime;
        Vector3 indicatorScale = _indicatorPrefab.transform.localScale;
        _indicator.transform.localScale = Vector3.Lerp(indicatorScale, _indicatorFinalScale, _spawnTimeCounter/_spawnTimer);
    }

    // ================================== COROUTINES ====================================
    // Add a delay before spawn
    private IEnumerator DelaySpawn()
    {
        _isSpawned = true;
        CreateIndicator();
        yield return new WaitForSeconds(_spawnTimer);
        Instantiate();
        StartCoroutine(DelayDeath());
    }
    // Add a delay before death
    private IEnumerator DelayDeath()
    {
        yield return new WaitForSeconds(_deathTimer);
        _isDead = true;
        Destruction();
    }
}
