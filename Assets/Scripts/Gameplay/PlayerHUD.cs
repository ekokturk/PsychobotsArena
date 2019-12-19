// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [Header("HUD from Spawner or Character")]
    [SerializeField] private PlayerSpawner      _spawner                = null;            
    [SerializeField] private GameObject         _playerCharacter        = null;
    [SerializeField] private float              _smoothChange           = 5f;               // Interpolation magnitude
    [Header("Main Display")]
    [SerializeField] private Image              _mainDisplay            = null;
    [SerializeField] private Color              _mainDisplayColor       = Color.white;        
    [SerializeField] private TextMeshProUGUI    _playerIndexText        = null;
    [Header("Hit Points")]
    [SerializeField] private Image              _hitPointsDisplay       = null;             // Robot HP HUD element
    [SerializeField] private Color              _hitPointsColor         = Color.red;        
    [Header("Robot Utility")]
    [SerializeField] private Image              _utilityDisplay         = null;             // Robot Utility HUD element
    [SerializeField] private Color              _utilityEnabledColor    = Color.blue;       
    [SerializeField] private Color              _utilityDisabledColor   = Color.gray;
    [Header("Left Component")]
    [SerializeField] private Image              _leftComponentDisplay   = null;             // Left Component HUD element
    [SerializeField] private Color              _leftEnabledColor       = Color.yellow;     
    [SerializeField] private Color              _leftDisabledColor      = Color.gray;
    [Header("Right Component")]
    [SerializeField] private Image              _rightComponentDisplay  = null;             // Right Component HUD element 
    [SerializeField] private Color              _rightEnabledColor      = Color.green;
    [SerializeField] private Color              _rightDisabledColor     = Color.gray;

    private GameObject        _robot        = null;
    private HitPoints         _robotHP      = null;
    private ARobotUtility     _robotUtility = null;
    private ARobotComponent   _robotLeft    = null;
    private ARobotComponent   _robotRight   = null;

    // ================================== MONOBEHAVIOUR ====================================
    private void Start() 
    {
         // Get spawned player, if there is no spawner set attached gameobject for HUD setup
        if(_spawner == null) 
            _robot = _playerCharacter;                   // Player character reference
        else                 
        {
            _robot = _spawner.GetSpawned();              // Spawner tracker reference
            _playerCharacter = null;
        }
    }

    private void Update()
    {
        if(_robot == null)                              // Check if player exists
        {
            if(_spawner != null)
                _robot = _spawner.GetSpawned();         // Get Player from spawner
            ClearDisplays();
            return;
        }
        // Show HUD for players
        DisplayHitPoints();
        DisplayUtility();
        DisplayLeftComponent();
        DisplayRightComponent();
    }

    // ================================== HUD DISPLAY ====================================

    // Show HUD for hitpoints with it's current state
    private void DisplayHitPoints()
    {
        if(_hitPointsDisplay == null) return;                                           // Check if HUD for hitpoints exists
        if(_robotHP == null)                                                            // Check if hitpoints exists
            _robotHP = _robot?.GetComponent<HitPoints>();             
        else
        {                                                                               // Display health
            _hitPointsDisplay.fillAmount = Mathf.Lerp(_hitPointsDisplay.fillAmount,
                                                      _robotHP.HealthPercentage / 100.0f,
                                                      Time.deltaTime * _smoothChange);
            _hitPointsDisplay.color = _hitPointsColor;                                  // Set health color
        }
    }
    // Show HUD for Robot utillities with it's current state
    private void DisplayUtility()
    {
        if(_utilityDisplay == null) return;                                             // Check if HUD for utilities exists
        if(_robotUtility   == null)                                                     // Check if robot utilites exist
            _robotUtility = _robot.GetComponent<ARobotUtility>();
        else
        {
            _utilityDisplay.fillAmount = Mathf.Lerp(_utilityDisplay.fillAmount,         // Display robot utility
                                                    _robotUtility.RechargeRatio,
                                                    Time.deltaTime * _smoothChange);

            if(_robotUtility.RechargeRatio < _robotUtility.UsabilityRatio)              // Change utility color           
                _utilityDisplay.color = _utilityDisabledColor;
            else
                _utilityDisplay.color = _utilityEnabledColor;
        }
    }

    // Show HUD for left component with it's current state
    private void DisplayLeftComponent()
    {
        if(_leftComponentDisplay == null) return;                                       // Check if HUD for left component exists
        if(_robotLeft == null) 
            _robotLeft = _robot.GetComponent<PlayerController>().LeftComponent;
        else
        {   // Display robot left component HUD
            _leftComponentDisplay.fillAmount = Mathf.Lerp(_leftComponentDisplay.fillAmount,
                                                          _robotLeft.RechargeRatio,
                                                          Time.deltaTime * _smoothChange);
            // Change Left Component color                                        
            if(_robotLeft.RechargeRatio < _robotLeft.UsabilityRatio)
                _leftComponentDisplay.color = _leftDisabledColor;
            else
                _leftComponentDisplay.color = _leftEnabledColor;
        }
    }
    // Show HUD for right component with it's current state
    private void DisplayRightComponent()
    {
        if(_rightComponentDisplay == null) return;                                      // Check if HUD for right component exists
        if(_robotRight == null) 
            _robotRight = _robot.GetComponent<PlayerController>().RightComponent;
        else
        {   // Display robot right component HUD
            _rightComponentDisplay.fillAmount = Mathf.Lerp(_rightComponentDisplay.fillAmount,
                                                           _robotRight.RechargeRatio,
                                                           Time.deltaTime * _smoothChange);
            // Change Right Component color                                        
            if(_robotRight.RechargeRatio < _robotRight.UsabilityRatio)
                _rightComponentDisplay.color = _rightDisabledColor;
            else
                _rightComponentDisplay.color = _rightEnabledColor;
        }

    }

    public void SetTeamColor(Color teamColor)
    {
        if(_mainDisplay     != null) _mainDisplay.color     = teamColor;
        if(_playerIndexText != null) _playerIndexText.color = teamColor;
    }

    // Clear displays to zero if gameobject is not found 
    private void ClearDisplays()
    {
        if(_hitPointsDisplay        != null) _hitPointsDisplay.fillAmount       = 0;
        if(_utilityDisplay          != null) _utilityDisplay.fillAmount         = 0;
        if(_leftComponentDisplay    != null) _leftComponentDisplay.fillAmount   = 0;
        if(_rightComponentDisplay   != null) _rightComponentDisplay.fillAmount  = 0;
        if(_mainDisplay             != null) _mainDisplay.color = _mainDisplayColor;
        if(_playerIndexText         != null) _playerIndexText.color = _mainDisplayColor;
    }

}
