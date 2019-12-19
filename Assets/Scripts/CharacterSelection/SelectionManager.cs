// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class SelectionManager : MonoBehaviour
{
    [SerializeField] private GameSettings       _gameSettings       = null;                     // Game Settings Object
    [Header("Spawners")]
    [SerializeField] private PlayerSpawner []   _playerSpawners     = new PlayerSpawner[4];     // Spawners
    [Header("Menus")]
    [SerializeField] private SelectionMenu []   _robotMenus         = new SelectionMenu[4];     // Character Menus
    [Header("Team Spotlights")]
    [SerializeField] private Light[]            _spotLights         = new Light[4];             // Character spotlights
    [Header("Timer")]
    [SerializeField] private Countdown          _finalCountdown     = null;                     // Game begin countdown

    // ================================== MONOBEHAVIOUR ====================================
    private void Start() 
    {
        SetupRobots();
    }

    // ================================== MAIN ====================================

    // Setup default robots
    public void SetupRobots()
    {
        _gameSettings.Players = new GameSettings.Player[4];                                 // Reinitialize robots
        if(_gameSettings.Robots.Count == 0)                                                 // Check for robot prefabs
            return;
        for(int i = 0; i < _gameSettings.Controllers.Length; i++)                           // Do for each spawn point
        {
            if(_playerSpawners[i] == null) continue;                                        // If spawn point is null stop
            if(System.String.IsNullOrEmpty(_gameSettings.Controllers[i])) continue;         // Check for available joysticks
            _gameSettings.Players[i].index = i+1;                                           // Set player index
            _gameSettings.Players[i].team = i+1;                                            // Set team
            if(_spotLights[i] != null) _spotLights[i].color = _gameSettings.TeamColors[_gameSettings.Players[i].team - 1];
            _playerSpawners[i].Spawn(_gameSettings.Robots[0].prefab,
                                     _gameSettings.Robots[0].leftComponent.prefab,
                                     _gameSettings.Robots[0].rightComponent.prefab);        // Spawn default robot
            _playerSpawners[i].FreezeSpawned();                                             // Disable movement
            _playerSpawners[i].GetSpawned().GetComponent<HitPoints>().Invincible = true;
            SetupMenu(i);                                                                   // Setup menu for the robot
        }
    }

    // Lock in all characters and setup final robot configurations
    public void LockInCharacters()
    {
        SetPlayerCharacters();    
        HideMenus();
    }

    // Set final character configurations to gamesettings
    private void SetPlayerCharacters()
    {
        for(int i=0; i < _gameSettings.Players.Length; i++)
        {
            if(_gameSettings.Players[i].index == 0) continue;               // Check if player is unavailable (index 0 means no player)
            // Set player robot in game settings
            _gameSettings.Players[i].robotPrefab = _gameSettings.Robots[_robotMenus[i].RobotIndex].prefab;
            // Check if selection is default arm or not for left component
            GameObject leftPrefab = _robotMenus[i].LeftComponentIndex == 0 ? _gameSettings.Robots[_robotMenus[i].RobotIndex].leftComponent.prefab
                                                                           : _gameSettings.Components[_robotMenus[i].LeftComponentIndex - 1].prefab;

            // Check if selection is default arm or not for right component
            GameObject rightPrefab = _robotMenus[i].RightComponentIndex == 0 ? _gameSettings.Robots[_robotMenus[i].RobotIndex].rightComponent.prefab
                                                                           : _gameSettings.Components[_robotMenus[i].RightComponentIndex - 1].prefab;

            _gameSettings.Players[i].leftComponentPrefab = leftPrefab;      // Set Player left component in game settings
            _gameSettings.Players[i].rightComponentPrefab = rightPrefab;    // Set Player right component in game settings
        }
    }

    // ================================== MENUS ====================================
    // Deactivate Menus
    private void HideMenus()
    {
        for(int i = 0; i < _robotMenus.Length; i++)
        {
            if(!_robotMenus[i]) continue;                               // Check for null
            _robotMenus[i].gameObject.SetActive(false);                 // Deactivate
        }
    }

    // Show menu and populate it with available data
    private void SetupMenu(int index)
    {
        if(!_robotMenus[index]) return;                                 // Check for null
        _robotMenus[index].gameObject.SetActive(true);                  // Show Menu
        _robotMenus[index].IsReady = false;
    }


    // ================================== OTHER ====================================

    public void CheckReady()
    {
        for(int i = 0; i < _robotMenus.Length; i++)
        {
            if(_robotMenus[i] == null)          continue;  
            if(_robotMenus[i].IsReady == false) return;
        }
        if(_finalCountdown) _finalCountdown.IsCounting = true;
    }


    //TODO Remove this
    public void StartMatch()
    {
        SceneManager.LoadScene("GameplayScene");
    }

}
