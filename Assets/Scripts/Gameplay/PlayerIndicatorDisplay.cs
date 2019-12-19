// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicatorDisplay : MonoBehaviour
{
    [SerializeField] private Canvas                 _displayCanvas  = null;
    [SerializeField] private Camera                 _cameraToLookAt = null;
    [SerializeField] private Image                  _indicatorArrow = null;
    [SerializeField] private TMPro.TextMeshProUGUI  _playerName     = null;

    // =============================== GETTER/SETTERS ===================================

    public Camera CameraToLookAt { set{ _cameraToLookAt = value; }}

    // ================================ MONOBEHAVIOUR ===================================

    private void Update() 
    {
        if(_cameraToLookAt != null)
            _displayCanvas.transform.LookAt(_cameraToLookAt.transform.position);
    }

    // ============================ CONFIGURE INDICATOR ==================================

    public void SetPlayerIndicator(int playerNo, Color teamColor)
    {
        if(_playerName  != null)
        {
            _playerName.text = $"P{playerNo}";
            _playerName.color = teamColor;
        }
        if(_indicatorArrow != null)
            _indicatorArrow.color = teamColor;
    }

    public void ShowDisplay()
    {
        _displayCanvas.gameObject.SetActive(true);
    }

}
