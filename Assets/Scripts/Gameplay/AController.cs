// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public abstract class AController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected float _movementSpeed   = 100;          // Movement force magnitude
    [SerializeField] protected float _rotationSpeed   = 100;          // Rotation torque magnitude

    [Header("Ground Check")]
    [SerializeField] protected bool      _isGrounded        = false;
    [SerializeField] protected float     _groundCheckRadius = 0.3f;
    [SerializeField] protected LayerMask _groundMask        = 0;
    [SerializeField] protected Transform _groundLocation    = null;
    [SerializeField] protected float     _groundCheckOffset = 0;

    [Header("Components")]
    [SerializeField] protected GameObject _leftComponent  = null;     // Left component
    [SerializeField] protected GameObject _rightComponent = null;     // Right component


    /// <summary> Animator 'Attack' animation trigger parameter (default = "BodyAttack") </summary>
    protected string    _bodyAttackParam = "RobotAttack";
    /// <summary> Animator 'Utility' animation boolean parameter (default = "BodyUtility") </summary>
    protected string    _bodyUtilityParam = "RobotUtility";

    protected Rigidbody     _rigidbody              = null;
    protected Animator      _animator               = null;
    protected ARobotUtility _robotUtility           = null;
    
    protected float     _speedMultiplier   = 1f; 

    // Action states
    protected bool      _canMove                = true;
    protected bool      _canRotate              = true;
    protected bool      _canAttack              = true;
    protected bool      _canUtility             = true;

    // Components that are attached to the left and right connectors
    protected ARobotComponent   _leftAttachment;
    protected ARobotComponent   _rightAttachment;

     // =============================== GETTER/SETTERS ===================================

    public bool CanMove     { get{ return _canMove;    }    set{ _canMove    = value; } }
    public bool CanRotate   { get{ return _canRotate;  }    set{ _canRotate  = value; } }
    public bool CanAttack   { get{ return _canAttack;  }    set{ _canAttack  = value; } }
    public bool CanUtility  { get{ return _canUtility; }    set{ _canUtility = value; } }
    public bool IsGrounded  { get{ return _isGrounded; }    set{ _isGrounded = value; } }

    public ARobotComponent LeftComponent  { get{ return _leftAttachment; } }
    public ARobotComponent RightComponent { get{ return _rightAttachment; } }

    public float SpeedMultiplier  { get{ return _speedMultiplier; }  set{ _speedMultiplier = value; } }


    // ================================== MONOBEHAVIOUR ====================================

    private void Awake() 
    {
        _rigidbody = GetComponent<Rigidbody>();             // Initialize Rigidbody component
        _animator  = GetComponent<Animator>();              // Initialize Animator component
        _robotUtility = GetComponent<ARobotUtility>();
        SetComponentAttachments();
    }

    // ====================================== MOTION ======================================

    /// <summary> Move gameobject forward with the available speed force </summary>
    protected virtual void Move()
    {
        _rigidbody.AddForce(transform.forward * _movementSpeed * _speedMultiplier);
    } 
    /// <summary> Move gameobject forward with the available speed force multiplied by 'magnitude' parameter </summary>
    protected virtual void Move(float magnitude)
    {
        _rigidbody.AddForce(transform.forward * magnitude * _movementSpeed * _speedMultiplier);
    }
    protected virtual void Move(float magnitudeX, float magnitudeY)
    {
        float angle = Mathf.Atan2(magnitudeX, magnitudeY) ;
        Quaternion rot = Quaternion.Euler(0,angle* 180.0f / 3.14f, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, _rotationSpeed * Time.deltaTime);
        _rigidbody.AddForce(transform.forward  * _movementSpeed * _speedMultiplier);
    }  
    /// <summary> Change angular velocity with the available rotation speed</summary>
    protected virtual void Rotate()
    {
        _rigidbody.angularVelocity = transform.up * _rotationSpeed;
    }
    /// <summary> Change angular velocity with the available rotation speed multiplied by 'magnitude' parameter</summary>
    protected virtual void Rotate(float magnitude)
    {
        _rigidbody.angularVelocity = transform.up * magnitude * _rotationSpeed;
    } 
    // =================================== GROUND CHECK ======================================
    protected void CheckGround()
    {
        if(_groundLocation == null)                                 // Check if ground check transform is added
        {
            Debug.LogWarning("Ground check target is not added");
            return;
        }

        Vector3 checkLocation = new Vector3(0, _groundCheckOffset, 0) + _groundLocation.position;               // Location for the ground check           
        DebugExtension.DebugWireSphere(checkLocation, Color.green, _groundCheckRadius);                         // Show it as a sphere for debugging
        Collider[]  groundColliders = Physics.OverlapSphere(checkLocation, _groundCheckRadius, _groundMask);    // Get colliders for ground check
        foreach (Collider collider in groundColliders)                                                          // Do for each collider
        {
            if(collider.gameObject == gameObject) continue;                                                     // Do not collide with your own collider
            _isGrounded = true;                                                                                 // Check for ground
            return;
        }
        _isGrounded = false;
    }

    // =================================== ROBOT FUNCTIONS ======================================
    public virtual void UseRobotAttack()
    {
        _animator.SetTrigger(_bodyAttackParam);
    }
    public virtual void StopRobotAttack()
    {
        _animator.ResetTrigger(_bodyAttackParam);
    }
    public virtual void UseRobotUtility()
    {
        if(_robotUtility.RechargeRatio != 0)
            _animator.SetBool(_bodyUtilityParam, true);
    }
    public virtual void StopRobotUtility()
    {
        _animator.SetBool(_bodyUtilityParam, false);

    }
    // =================================== COMPONENT FUNCTIONS ======================================
    // Get component attachments from gameobject
    public void SetComponentAttachments()
    {
        _leftAttachment  =  _leftComponent.GetComponentInChildren<ARobotComponent>();
        _rightAttachment =  _rightComponent.GetComponentInChildren<ARobotComponent>();
    }
    
    public virtual void UseLeftComponent()
    {
        _leftAttachment?.Use();
    }

    public virtual void UseRightComponent()
    {
        _rightAttachment?.Use();
    }

    // ====================================================================================
    protected virtual void SetAnimatorParameters(){}

}
