// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ArenaManager : MonoBehaviour
{
    [SerializeField] private GameSettings           _gameSettings     = null;
    [SerializeField] private PlayerSpawner[]        _playerSpawners   = null;
    [Header("Main Camera")]
    [SerializeField] private DynamicCamera          _camera           = null;
    [Header("GUI")]
    [SerializeField] private TMPro.TextMeshProUGUI  _matchOverText    = null;
    [SerializeField] private PlayerHUD[]            _playerDisplays   = null;
    [Header("Audio")]
    [SerializeField] private AudioSource[]          _playerWinSFX     = null;
    [Header("Events")]
    [SerializeField] private UnityEvent             _onMatchStarted   = null;
    [SerializeField] private UnityEvent             _onMatchOver      = null;
    
    private bool _isMatchOver = false;
    // ================================== MONOBEHAVIOUR ====================================
    private void Awake() 
    {
        // Instantiate a random environment
        if(_gameSettings.Environments.Count > 0)
            Instantiate(_gameSettings.Environments[Random.Range(0, _gameSettings.Environments.Count)]);    
        // Instantiate a random trap layout Game Settings
        if(_gameSettings.Layouts.Count > 0)
            Instantiate(_gameSettings.Layouts[Random.Range(0, _gameSettings.Layouts.Count)]);    
    }

    private void Start() 
    {
        SetupPlayers();
        ResetCameraTargets();
    }

    private void Update()
    {
        if(_isMatchOver == false)
            _isMatchOver = CheckGameOver(); // TODO Optimize !!!
    }
    // ================================== MATCH STATES ==================================

    public void DoMatchStart()
    {
        _onMatchStarted?.Invoke();
    }

    public void DoMatchOver()
    {
        _onMatchOver?.Invoke();
    }

    // ================================== WIN CONDITION ==================================
    // Check if only one robot is standing
    public bool CheckGameOver()
    {
        int defeatedPlayers = 0;
        for(int i = 0; i< _playerSpawners.Length; i++)
        {
            if(_playerSpawners[i]?.GetSpawned() == null)
                defeatedPlayers++;
            else if(_playerSpawners[i]?.GetSpawned().GetComponent<HitPoints>()?.HealthPercentage == 0)
                defeatedPlayers++;
        }

        if(defeatedPlayers == 3) 
        {
            DoMatchOver();
            return true;
        }
        else return false;
    }

    // Win the game for the robot which has the highest hp
    public void WinForHighestHP()
    {
        GameObject highestHP = null;
        for(int i = 0; i< _playerSpawners.Length; i++)
        {
            if(_playerSpawners[i]?.GetSpawned() == null) continue;
            if(highestHP == null)
                highestHP = _playerSpawners[i].GetSpawned();
            else
            {
                if(highestHP.GetComponent<HitPoints>().HealthPercentage >=
                   _playerSpawners[i].GetSpawned().GetComponent<HitPoints>().HealthPercentage)
                {
                    _playerSpawners[i].GetSpawned().GetComponent<HitPoints>().Death();
                }
                else
                {
                    highestHP.GetComponent<HitPoints>().Death();
                    highestHP = _playerSpawners[i].GetSpawned();
                }
            }
        }
        _isMatchOver = true;
        SetForWinner(highestHP);            // TODO Set this as MatchOver Event
        
    }

    // Set the UI and configuration for the robot that is won
    public void FocusOnWinner()
    {
        for(int i = 0; i< _playerSpawners.Length; i++)
        {
            if(_playerSpawners[i]?.GetSpawned() == null) continue;
            GameObject player = _playerSpawners[i].GetSpawned();
            SetForWinner(player);
            return;
        }
    }

    // Set the UI and configuration for the robot that is won
    private void SetForWinner(GameObject player)
    {
        if(player != null)
        {

            player.GetComponent<HitPoints>().Invincible = true;
            int playerNo = player.GetComponent<PlayerController>().InputType;
            if(_playerWinSFX[playerNo] != null) _playerWinSFX[playerNo].Play();
            _matchOverText.text = $"Bot {playerNo+1} Wins";
        } 
        else  _matchOverText.text = $"DRAW!";
    }

    // ================================== ARENA CAMERA ====================================    
    public void ResetCameraTargets()
    {
        List<Transform> targets = new List<Transform>();
        for(int i = 0; i < _playerSpawners.Length; i++)
        {
            if(_playerSpawners[i]?.GetSpawned() == false) continue;
            targets.Add(_playerSpawners[i]?.GetSpawned().transform);
        }
        _camera.SetTargets(targets);
    }

    // ================================== OTHER ====================================    
    // Initialize players
    private void SetupPlayers()
    {
        for(int i = 0; i < _gameSettings.Players.Length; i++)
        {
            if(_playerSpawners[i] == null) continue;
            if(_gameSettings.Players[i].index == 0) continue; 
            if(_gameSettings.Players[i].robotPrefab == null) continue;
            _playerSpawners[i].Spawn(_gameSettings.Players[i].robotPrefab,
                                     _gameSettings.Players[i].leftComponentPrefab,
                                     _gameSettings.Players[i].rightComponentPrefab);
            _playerSpawners[i].SetTeam(_gameSettings.Players[i].team);
            _playerDisplays[i].gameObject.SetActive(true);
            _playerDisplays[i].SetTeamColor(_gameSettings.TeamColors[_gameSettings.Players[i].team-1]);
            _playerSpawners[i].SetIndicator(_camera.GetComponent<Camera>(),i+1,_gameSettings.TeamColors[i]);
        }
    }
}
