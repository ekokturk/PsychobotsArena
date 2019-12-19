// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameSettings               _gameSettings       = null;         // Game Settings Scriptable Object
    [Header("Player Connection")]
    [SerializeField] private TMPro.TextMeshProUGUI[]    _connectionText     = null;
    [SerializeField] private Color                      _connectedColor     = Color.green;
    [SerializeField] private Color                      _notConnectedColor  = Color.gray;
    [Header("Error")]
    [SerializeField] private TMPro.TextMeshProUGUI      _errorMessage           = null;
    [SerializeField] private string                     _notEnoughPlayersError  = "You need at least two players in order to play";

    private string  _characterSelection     = "CharacterSelectionScene";
    private int     _availablePlayers       = 0;

    // ================================== MONOBEHAVIOUR ====================================
    private void Start() 
    {
        _gameSettings.Clear();                                              // Clear previous settings
    }
    private void Update() 
    {
        _gameSettings.Controllers = Input.GetJoystickNames();               // Get connected joysticks
        ShowPlayerConnections();
        ShowErrorMessage();
    }

    // ================================= SCENES AND GAME ====================================
    // Setup game mode 
    public void SetGameMode(int gameMode)
    {
        _gameSettings.gameMode = gameMode;
    }

    // Change scene depending on the scene name
    public void GoCharacterSelection()
    {
        if(_availablePlayers > 1)
            SceneManager.LoadScene(_characterSelection);
    }

    // Show available players in the main menu
    private void ShowPlayerConnections()
    {
        _availablePlayers = 0;
        for(int i = 0; i < _gameSettings.Controllers.Length; i++)
        {
            if(System.String.IsNullOrEmpty(_gameSettings.Controllers[i]))
            {
                _connectionText[i].color = _notConnectedColor;
            }
            else
            {
                _connectionText[i].color = _connectedColor;
                _availablePlayers++;
            }
        }
    }

    private void ShowErrorMessage()
    {
        if(_errorMessage == null)   return;
        if(_availablePlayers < 2)
            _errorMessage.text = _notEnoughPlayersError;
        else
            _errorMessage.text = "";
    }

    // Quit application
    public void QuitGame()
    {
        Application.Quit();
    }

}
