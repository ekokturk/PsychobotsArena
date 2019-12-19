// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class PopupMenuController : MonoBehaviour
{
    // [SerializeField] private GameObject _popupMenu      = null;
    [SerializeField] private string     _popupMenuInput = "Menu";
    [Header("Events")]
    [SerializeField] private UnityEvent _onShowPopup    = null;
    [SerializeField] private UnityEvent _onClosePopup   = null;

    private bool _isPopupActive = false;

    // ================================== MONOBEHAVIOUR ====================================

    private void Update() 
    {
        if(Input.GetButtonDown(_popupMenuInput))
        {
            if(_isPopupActive == false)
                ShowPopup();
            else
                ClosePopup();
        }
    }

    // ============================== MENU FUNCTIONALITY ==================================

    public void FreezeGame()
    {
        Time.timeScale = 0;
    }

    public void UnFreezeGame()
    {
        Time.timeScale = 1;
    }

    public void ShowPopup()
    {
        _onShowPopup?.Invoke();
        _isPopupActive = true;
    }

    public void ClosePopup()
    {
        _isPopupActive = false;
        _onClosePopup?.Invoke();
    }

    // ============================== SCENE MANAGEMENT ==================================

    public void ChangeScene(string sceneName)
    {
        ClosePopup();
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene()
    {
        ClosePopup();
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
    }

}
