// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;        // Enum Descriptions
using UnityEngine;

public class PlayerController : AController
{
    // Player input types for four different inputs
    public enum PlayerInputTypes
    {
        [Description("_P1")] Player1,
        [Description("_P2")] Player2,
        [Description("_P3")] Player3,
        [Description("_P4")] Player4
    };

    [Header("Player Input")]
    [SerializeField] private PlayerInputTypes _inputType = 0;       // Player input selected
    [Header("Taunt")]
    [SerializeField] private AudioSource    _tauntSFX = null;

    private float     _movementInputValue     = 0;      // Vertical value from input axis
    private float     _rotationInputValue     = 0;      // Horizontal value from input axis

    // Player Inputs for all the actions
    private string _movementInput       = "";
    private string _rotationInput       = "";
    private string _robotAttackInput    = "";
    private string _robotUtilityInput   = "";
    private string _leftComponentInput  = "";
    private string _rightComponentInput = "";

    public int InputType 
    {
        get{ return (int)_inputType; } set{_inputType = (PlayerInputTypes)value;}
    }

    // ================================== MONOBEHAVIOUR ====================================
    private void Start()
    {
        SetupPlayerInputs();
    }

    private void Update()
    {
        if(Time.timeScale == 0) return;
        CheckGround();
        GetMovementInputs();
        SetAnimatorParameters();
        UseRobotActionsOnInput();
        UseComponentsOnInput();
    }

    private void FixedUpdate()
    {
        if(_movementInputValue != 0 || _rotationInputValue !=0)
            Move(_movementInputValue,_rotationInputValue);
    }

    // ================================== MOVEMENT ====================================

    private void GetMovementInputs()
    {
        _movementInputValue = Input.GetAxis(_movementInput);      // Get Vertical input for movement
        _rotationInputValue = Input.GetAxis(_rotationInput);      // Get Horizontal input for rotation
    }

    // =================================== ACTIONS =====================================
    // Use robot attack and utility function on player inputs
    private void UseRobotActionsOnInput()
    {
        // Robot Attack
        if (Input.GetButtonDown(_robotAttackInput))
            UseRobotAttack();
        
        // Robot Utility
        if (Input.GetButton(_robotUtilityInput))
            UseRobotUtility();
        else 
            StopRobotUtility();
    }

    // ================================== COMPONENTS ====================================
    // Use robot attack and utility function on player inputs
    private void UseComponentsOnInput()
    {
        if (Input.GetButton(_rightComponentInput))                   // Use right component on user input
            UseRightComponent();
        else if (Input.GetButton(_leftComponentInput))               // Use left component on user input
            UseLeftComponent();
    }

    public void AttachLeftComponent(GameObject component)
    {
        Transform leftTransform = _leftComponent.transform;
        for(int i = leftTransform.childCount - 1; 0 <= i; i--)
        {
            Destroy(leftTransform.GetChild(i).gameObject);
        }
        if(component)
        {
            GameObject newComponent = Instantiate(component, leftTransform.position, leftTransform.rotation);
            newComponent.transform.parent = _leftComponent.transform;
            newComponent.transform.localScale = component.transform.localScale;
            _leftAttachment = newComponent.GetComponent<ARobotComponent>();
        }
        
    }

    public void AttachRightComponent(GameObject component)
    {
        Transform rightTransform = _rightComponent.transform;
        for(int i = rightTransform.childCount - 1; 0 <= i; i--)
        {
            Destroy(rightTransform.GetChild(i).gameObject);
        }
        if(component)
        {
            GameObject newComponent = Instantiate(component, rightTransform.position, rightTransform.rotation);
            newComponent.transform.parent = _rightComponent.transform;
            newComponent.transform.localScale = component.transform.localScale;
            _rightAttachment = newComponent.GetComponent<ARobotComponent>();
        }
    }

    // ================================== ANIMATION ====================================
    // Set parameter value in the animator
    protected override void SetAnimatorParameters()
    {
        base.SetAnimatorParameters();
        _animator.SetFloat("Movement", _movementInputValue);              
    }

    // =================================== INPUTS =====================================
    // Setup player inputs for available mechanics according to selected input type
    private void SetupPlayerInputs()
    {
        // Get descriptions from
        object[] descriptionAttributes = _inputType.GetType().GetField(_inputType.ToString())
                                        .GetCustomAttributes(typeof(DescriptionAttribute), false);

        // Set description of enum as a suffix to be used for inputs
        string inputName = descriptionAttributes.Length > 0 ? (descriptionAttributes[0] as DescriptionAttribute).Description 
                                                              : "";

        // Setup inputs for that certain player
        _movementInput       = $"Vertical{inputName}";
        _rotationInput       = $"Horizontal{inputName}";
        _robotAttackInput    = $"RobotAttack{inputName}";
        _robotUtilityInput   = $"RobotUtility{inputName}";
        _leftComponentInput  = $"LeftComponent{inputName}";
        _rightComponentInput = $"RightComponent{inputName}";
    }

    public void DoTaunt()
    {
        if(_tauntSFX != null && _tauntSFX.isPlaying == false) _tauntSFX.Play();
    }


}
