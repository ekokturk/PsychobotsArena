// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class SelectionMenu : MonoBehaviour
{
    [SerializeField] private GameSettings   _gameSettings       = null;                 // Game Settings Scriptable Object
    [SerializeField] private PlayerSpawner  _playerSpawner      = null;                 // Player spawner
    [Header("Navigation")]
    [SerializeField] private string         _navigationInput    = "VerticalArrow_P1";   // User input for navigation
    [SerializeField] private float          _navigationDelay    = 0.2f;                 // Delay between each inpu
    [SerializeField] private int            _selectionIndex     = 0;                    // Index of the default selected item
    [SerializeField] private GameObject []  _menuItems          = null;                 // Menu items to navigate through
    [Header("Prefab Names")]
    [SerializeField] private TMPro.TextMeshProUGUI  _robotText          = null;
    [SerializeField] private TMPro.TextMeshProUGUI  _leftComponentText  = null;
    [SerializeField] private TMPro.TextMeshProUGUI  _rightComponentText = null;
    [Header("Audio")]
    [SerializeField] private AudioSource  _navigateSFX = null;

    // private int            _teamIndex              = 0;      // For team implementation if needed
    private int            _robotIndex             = 0;
    private int            _leftComponentIndex     = 0;
    private int            _rightComponentIndex    = 0;
    private bool           _isReady                = true;

    private bool           _isLocked               = false;

    private bool    _canNavigate            = true;
    private float   _verticalInputValue     = 0;
    // ================================== GETTER/SETTER ====================================
    public int RobotIndex           { get { return _robotIndex;          }}
    public int LeftComponentIndex   { get { return _leftComponentIndex;  }}
    public int RightComponentIndex  { get { return _rightComponentIndex; }}
    public bool IsReady             { get { return _isReady; }  set{ _isReady = value; }}  
    public bool IsLocked            { get { return _isLocked; } set{ _isLocked = value; }} 

    // ================================== MONOBEHAVIOUR ====================================
    private void Start()
    {
        _menuItems[_selectionIndex].GetComponent<SelectorButton>()?.Select();                           // Set default selection
        // Initialize Robot configuration labels
        ChangeOptionLabel(_robotText,           _gameSettings.Robots[RobotIndex].name);
        ChangeOptionLabel(_leftComponentText,   _gameSettings.Robots[RobotIndex].leftComponent.name);
        ChangeOptionLabel(_rightComponentText,  _gameSettings.Robots[RobotIndex].rightComponent.name);
    }

    private void Update() 
    {
        _verticalInputValue = Input.GetAxisRaw(_navigationInput);        // Read User input
        NavigateMenu();                                                  // Navigate between options depending on the input
    }

    // ================================== NAVIGATION ====================================
    private void NavigateMenu()
    {
        if(IsLocked == true) return;
        if(_verticalInputValue == 0)
        {
            StopCoroutine("InputDelay");
            _canNavigate = true;
        }
        else
        {
            if(_canNavigate == false) return;
            StartCoroutine("InputDelay");
            if(_navigateSFX != null) _navigateSFX.Play();
            _menuItems[_selectionIndex].GetComponent<SelectorButton>()?.Deselect();
            if(_verticalInputValue > 0)
                NavigateUp();
            if(_verticalInputValue < 0)
                NavigateDown();
        }
    }

    private void NavigateUp()
    {
        _selectionIndex++;
        if(_selectionIndex >= _menuItems.Length)
            _selectionIndex = _menuItems.Length - 1;
        _menuItems[_selectionIndex].GetComponent<SelectorButton>()?.Select();
    }

    private void NavigateDown()
    {
        _selectionIndex--;
        if(_selectionIndex < 0)
            _selectionIndex =  0;
        _menuItems[_selectionIndex].GetComponent<SelectorButton>()?.Select();
    }
    // ========================================== ROBOT =============================================
    public void UpdateRobot()
    {
        if(_gameSettings.Robots.Count == 0) return;                                          // Check for robot prefabs
        if(_playerSpawner == null)          return;
        // Check for default components depending on robot type
        GameObject leftComponent = _leftComponentIndex == 0 ?  _gameSettings.Robots[_robotIndex].leftComponent.prefab
                                                            :   _gameSettings.Components[_leftComponentIndex-1].prefab;
        GameObject rightComponent = _rightComponentIndex == 0 ? _gameSettings.Robots[_robotIndex].rightComponent.prefab
                                                            :   _gameSettings.Components[_rightComponentIndex-1].prefab;

        _playerSpawner.Spawn(_gameSettings.Robots[_robotIndex].prefab,
                             leftComponent,
                             rightComponent);        
        _playerSpawner.FreezeSpawned();
        _playerSpawner.GetSpawned().GetComponent<HitPoints>().Invincible = true;
    }
    

    // ================================== SELECTION MANIPULATION ====================================
    public void NextRobot()
    {
        if(IsLocked == true) return;
        Increment(ref _robotIndex, _gameSettings.Robots.Count);
        ChangeOptionLabel(_robotText, _gameSettings.Robots[_robotIndex].name);
    }
    public void PreviousRobot()
    {
        if(IsLocked == true) return;
        Decriment(ref _robotIndex, _gameSettings.Robots.Count);
        ChangeOptionLabel(_robotText, _gameSettings.Robots[_robotIndex].name);
    }

    public void NextLeftComponent()
    {
        if(IsLocked == true) return;
        Increment(ref _leftComponentIndex, _gameSettings.Components.Count + 1);
        string compName = LeftComponentIndex == 0 ? _gameSettings.Robots[_robotIndex].leftComponent.name
                            : _gameSettings.Components[_leftComponentIndex - 1].name;
        ChangeOptionLabel(_leftComponentText, compName);
    }
    public void PreviousLeftComponent()
    {
        if(IsLocked == true) return;
        Decriment(ref _leftComponentIndex, _gameSettings.Components.Count + 1);
        string compName = LeftComponentIndex == 0 ? _gameSettings.Robots[_robotIndex].leftComponent.name
                                    : _gameSettings.Components[_leftComponentIndex - 1].name;
        ChangeOptionLabel(_leftComponentText, compName);
    }

    public void NextRightComponent()
    {
        if(IsLocked == true) return;
        Increment(ref _rightComponentIndex, _gameSettings.Components.Count + 1);
        string compName = RightComponentIndex == 0 ? _gameSettings.Robots[_robotIndex].rightComponent.name
                                            : _gameSettings.Components[_rightComponentIndex - 1].name;
        ChangeOptionLabel(_rightComponentText, compName);
    }
    public void PreviousRightComponent()
    {
        if(IsLocked == true) return;
        Decriment(ref _rightComponentIndex, _gameSettings.Components.Count+ 1);
        string compName = RightComponentIndex == 0 ? _gameSettings.Robots[_robotIndex].rightComponent.name
                                                    : _gameSettings.Components[_rightComponentIndex - 1].name;
        ChangeOptionLabel(_rightComponentText, compName);
    }

    // ================================== SELECTION STATES ====================================
    private void Increment(ref int index, int optionsCount)
    {
        index ++;
        if(index >= optionsCount)
            index = 0;
    }
    private void Decriment(ref int index, int optionsCount)
    {
        index --;
        if(index < 0)
            index = optionsCount - 1;
    }

    private void ChangeOptionLabel(TMPro.TextMeshProUGUI label, string newLabel)
    {
        if(label == null) return;
        label.text = newLabel;
    }

    // Delay Input by a certain duration
     IEnumerator InputDelay() 
     {
        _canNavigate = false;
        yield return new WaitForSeconds(_navigationDelay);
        _canNavigate = true;
    }

    // ================================== OTHER ====================================
    public void TriggerPlayerTaunt()
    {
        if(_playerSpawner != null) 
            _playerSpawner.GetSpawned().GetComponent<PlayerController>().DoTaunt();
    }

}
