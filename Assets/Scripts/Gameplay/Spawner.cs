// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Tooltip("Pool of objects that can be spawned in a random order")]
    [SerializeField] private GameObject []      _spawnables         = null;      // Spawning objects             
    [Tooltip("Spawn at a random location in a circular area (radius > 0) / Spawn object at location (radius == 0) ")]
    [SerializeField,Range(0,15)] private float  _areaRadius         = 10.0f;     // Outer radius of the spawn circle
    [Tooltip("Delay before object spawns when the spawner is activated")]
    [SerializeField] private float              _initialSpawnDelay  = 0.0f;      // Delay before starting to spawn
    [Tooltip("Spawn after this interval time when there is no spawned object (Do not spawn when there is)")]
    [SerializeField] private float              _spawnInterval      = 1.0f;      // Time interval between each spawns

    private GameObject  _spawnedObject      = null;      // Tracked object which was spawned by this
    private bool        _canSpawn           = false;     // Check if spawner can spawn
    // Coroutines for delays
    private Coroutine _initialDelayCoroutine  = null;  
    private Coroutine _spawnIntervalCoroutine = null;

    // ================================== MONOBEHAVIOUR ====================================

    private void Start()
    {
        _initialDelayCoroutine = StartCoroutine(DelayInitialSpawn());
    }

    private void Update() 
    {
        if(_spawnedObject == null && _canSpawn)                             // Only spawn when track object is null
            _spawnIntervalCoroutine = StartCoroutine(SpawnOnInterval());    // Spawn with interval
    }

    // ================================== SPAWN FUNCTIONALITY ====================================
    // Spawn a random object 
    private void SpawnRandom()
    {
        if(_spawnables.Length > 0)
        {
            int randomNo = Random.Range(0, _spawnables.Length);
            _spawnedObject = Instantiate(_spawnables[randomNo], SpawnInsideCircle(), transform.rotation);
        }
    }
    // Spawn an object at a random place in circular area
    private Vector3 SpawnInsideCircle()
    {
        float x = Mathf.Sin(Random.Range(0, 360)) * Random.Range(0, _areaRadius);
        float z = Mathf.Sin(Random.Range(0, 360)) * Random.Range(0, _areaRadius);

        return new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
    }

    // ================================== SPAWN DELAYS ====================================

    // Initial delay before starting to spawn an object
    private IEnumerator DelayInitialSpawn()
    {
        yield return new WaitForSeconds(_initialSpawnDelay);
        _canSpawn = true;
    }
    // Delay betwen each spawn
    private IEnumerator SpawnOnInterval()
    {
        _canSpawn = false;
        yield return new WaitForSeconds(_spawnInterval);
        SpawnRandom();
        _canSpawn = true;
    }

    // ================================== EDITOR DEBUG ====================================

    #if UNITY_EDITOR
    [Header("DEBUG")]
    [SerializeField] private bool  _isDebug = true;
    [SerializeField] private Color _debugColor = Color.cyan;
    private void OnDrawGizmos()
    {
        if(_isDebug == false) return;
        if(_areaRadius > 0)
            DebugExtension.DrawCylinder(transform.position,transform.position + new Vector3(0,1,0), _debugColor, _areaRadius);
        else
            DebugExtension.DrawArrow(transform.position,new Vector3(0,1,0), _debugColor);

    }
    #endif
}
