// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrap : MonoBehaviour
{
    [Header("Movement Configuration")]
    [Tooltip("Delay before object starts moving")]
    [SerializeField] private float _intialDelay         = 0f;
    [Tooltip("Wait time at each of the target locations")]
    [SerializeField] private float _stayDuration        = 1f;
    [Tooltip("Translation speed of the object")]
    [SerializeField] private float _movementSpeed       = 3f;
    [Tooltip("Set it to true for non-linear interpolation motion for the object")]
    [SerializeField] private bool  _isInterpolating     = false;
    [Header("Position Offsets")]
    [Tooltip("Target location in positive direction in Z Axis relative to the object")]
    [SerializeField,Range(0,15)]  private float _positiveOffset  = 5f;
    [Tooltip("Target location in negative direction in Z Axis relative to the object")]
    [SerializeField,Range(-15,0)] private float _negativeOffset  = -5f;

    private Vector3 _initialPosition    = Vector3.zero;
    private Vector3 _targetPosition     = Vector3.zero;
    private float   _targetReachOffset   = 0.2f;
    private bool    _isGoingPositive    = true;
    private bool    _canMove            = false;
    private bool    _targetReached      = false;

    // ================================== MONOBEHAVIOUR ====================================
    private void Start()
    {
        _initialPosition = transform.position;
        StartCoroutine(BeginInitialDelay()); 
        SetTarget();
    }

    private void Update()
    {
        if(_canMove == false) return;
        if(_targetReached == false)
        {
            if(_isInterpolating)
                transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _movementSpeed);
            else 
                transform.position += Time.deltaTime * (_targetPosition - transform.position).normalized * _movementSpeed; 
            if(Vector3.Distance(_targetPosition, transform.position) < _targetReachOffset) StartCoroutine(WaitAtTarget());
        }
        

    }
    // ================================== FUNCTIONALITY ====================================
    // Switch target for final position
    private void SetTarget()
    {
        if(_isGoingPositive == true)
            _targetPosition = _initialPosition + transform.forward * _negativeOffset;
        else
            _targetPosition = _initialPosition + transform.forward * _positiveOffset;
    }

    // ================================= COROUTINE DELAYS ===================================

    // Time delay before starting to move
    private IEnumerator BeginInitialDelay()
    {
        yield return new WaitForSeconds(_intialDelay);
        _canMove = true;
    }

    // Wait at the target position then go to opposite target
    private IEnumerator WaitAtTarget()
    {
        _targetReached = true;
        yield return new WaitForSeconds(_stayDuration);
        _isGoingPositive = !_isGoingPositive;
        SetTarget();
        _targetReached = false;
    }

    // ================================= DEBUGGING ===================================

    // Show locations for debugging
    #if UNITY_EDITOR
    [Header("DEBUG")]
    [SerializeField] private bool  _isDebug      = true;
    [SerializeField] private Color _debugColor   = Color.magenta;
    private void OnDrawGizmos() 
    {
        if(_isDebug == false) return;
        if(Application.isPlaying == false) _initialPosition = transform.position;
        DebugExtension.DrawArrow(_initialPosition + transform.forward * _negativeOffset, Vector3.up*3, _debugColor);
        DebugExtension.DrawArrow(_initialPosition + transform.forward * _positiveOffset, Vector3.up*3, _debugColor);
    }
    #endif
}
