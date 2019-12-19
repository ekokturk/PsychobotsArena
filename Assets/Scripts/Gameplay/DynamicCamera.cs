// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DynamicCamera : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private Transform[] _robots   = null;            // Playable characters
    [Header("Transform Settings")]
    [SerializeField] private Vector3     _offset   = Vector3.zero;    // Camera offset from the target location
    [SerializeField] private bool        _isLookAt = false;
    [Header("Field of View")]
    [SerializeField] private float   _minView     = 30.0f;          // Minimum field of view
    [SerializeField] private float   _maxView     = 80.0f;          // Maximum field of view
    [SerializeField] private float   _viewChange  = 20.0f;          // Maximum field of view

    private Camera  _camera             = null;           // Camera component
    private Vector3 _defaultOffset      = Vector3.zero;   // Initial offset of the camera


    // ================================== MONOBEHAVIOUR ====================================

    private void Start()
    {
        _camera = GetComponent<Camera>();                       // Initialize camera component
        _defaultOffset = _offset;
    }

    private void Update()
    {
        Vector3 targetPosition = GetTarget();
        float view = GetFieldOfView(targetPosition);

        transform.position = Vector3.Lerp(transform.position, targetPosition + _offset, 0.5f*Time.deltaTime);
        _camera.fieldOfView = Mathf.Lerp(_minView, _maxView, view * _viewChange);           // TODO Soft Transition

        if(_isLookAt == true) transform.LookAt(targetPosition);
    }
    
    // ================================== MONOBEHAVIOUR ====================================

    public void SetTargets(List<Transform> targets)
    {
        if(targets.Count == 0) return;
        _robots = new Transform[targets.Count];
        for(int i = 0; i < _robots.Length; i++)
        {
            _robots[i] = targets[i];
        } 
    }

    private Vector3 GetTarget()
    {
        Vector3 centerPosition = Vector3.zero;                  // Initialize target location
        if(_robots.Length == 0)                                 // If there are no robots assigned
            return centerPosition;

        int robotCount = 0;
        foreach (Transform robot in _robots)           
        {
            if(!robot) continue;
            centerPosition += robot.position;
            robotCount++;
        }
        if( robotCount == 0) return Vector3.zero;
        return centerPosition / robotCount;
    }

    private float GetFieldOfView(Vector3 cameraTarget)
    {
        float viewDistance = 0;
        foreach (Transform robot in _robots)
        {
            if(!robot) continue;

            float distance = Vector3.Distance(cameraTarget, robot.position);
            if(viewDistance < distance)
                viewDistance = distance;
        }
        return viewDistance;
    }



}
