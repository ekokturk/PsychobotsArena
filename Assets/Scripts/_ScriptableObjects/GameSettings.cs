// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "NewGameSettings", menuName="Game Settings")]
[System.Serializable]
public class GameSettings : ScriptableObject
{
    public enum GameMode
    {
        FreeForAll = 0,
        TeamDeathmatch
    }

    [System.Serializable] public struct Component
    {
        public GameObject   prefab;
        public string       name;
    }  

    [System.Serializable] public struct Robot
    {
        public GameObject   prefab;
        public string       name;
        public Component    leftComponent;
        public Component    rightComponent;
    }

    [System.Serializable] public struct Player 
    {
        public int          index;
        public int          team;
        public GameObject   robotPrefab;
        public GameObject   leftComponentPrefab;
        public GameObject   rightComponentPrefab;
    }

    public int gameMode = 0;
    [SerializeField] private string[]         _controllers      = new string[4];
    [SerializeField] private List<Component>  _components       = new List<Component>(4);
    [SerializeField] private List<Robot>      _robots           = new List<Robot>(4);
    [SerializeField] private List<GameObject> _layouts          = new List<GameObject>();
    [SerializeField] private List<GameObject> _environments     = new List<GameObject>();
    [SerializeField] private Player[]         _players          = new Player[4];
    [SerializeField] private Color[]          _teamColors       = new Color[4];
    [SerializeField] private AudioMixer       _audioMixer       = null;

    // ================================== GETTERS/SETTERS====================================   
    public List<Component>   Components      { get { return _components;   } }
    public List<Robot>       Robots          { get { return _robots;       } }
    public List<GameObject>  Layouts         { get { return _layouts;      } }
    public List<GameObject>  Environments    { get { return _environments; } }
    public Color[]           TeamColors      { get { return _teamColors;   } }
    public Player[]          Players         { get { return _players; } set{ _players = value; }}
    public string[]          Controllers
    {
        get { return _controllers; }
        set 
        {
            if(value.Length > 0 )
            {
                for(int i = 0; i < value.Length; i++)
                {
                    if(i > 3) break;
                    _controllers[i] = value[i];
                }
            // TODO Development
            }
            // if(System.String.IsNullOrEmpty(_controllers[3]))
            //     _controllers[3] = "Keyboard";
        }
    }

    // Clear settings from the object
    public void Clear()
    {
        _players = new Player[4];
        Controllers = new string[4];
    }

    public void MuteAudio()
    {
        AudioMixerSnapshot pausedSnapshot = _audioMixer.FindSnapshot("Paused");
        pausedSnapshot.TransitionTo(0);
    }

    public void UnMuteAudio()
    {
        AudioMixerSnapshot unpausedSnapshot = _audioMixer.FindSnapshot("Unpaused");
        unpausedSnapshot.TransitionTo(0);
    }
}

