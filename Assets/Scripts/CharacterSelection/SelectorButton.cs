// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SelectorButton : MonoBehaviour
{
    [Header("Navigation")]
    [SerializeField] private string     _horizontalInput    = "HorizontalArrow_P1";
    [SerializeField] private float      _navigationDelay    = 0.2f;
    [Header("Styles")]
    [SerializeField] private Sprite     _highlightSprite    = null;            
    [SerializeField] private Color      _highlightColor     = Color.gray;
    [Header("Selection Events")]
    [SerializeField] private UnityEvent _onSelect           = null;
    [SerializeField] private UnityEvent _onDeselect         = null;
    [Header("Option Events")]
    [SerializeField] private UnityEvent _onAllOptions       = null;
    [SerializeField] private UnityEvent _onPreviousOption   = null;
    [SerializeField] private UnityEvent _onNextOption       = null;


    private bool    _canNavigate    = true;
    private int     _inputValue     = 0;
    private bool    _isSelected     = false;        // State of selection
    private Image   _buttonImage    = null;         // Gameobject image
    private Color   _defaultColor   = Color.gray;   // Gameobject default color
    private Sprite  _defaultSprite  = null;

    // ================================== MONOBEHAVIOUR ====================================
    private void Awake() 
    {
        _buttonImage = GetComponent<Image>();
        _defaultColor = _buttonImage.color;
        _defaultSprite = _buttonImage.sprite;    
    }

    private void Update()
    {
        if(_isSelected)
        {
            _inputValue = (int)Input.GetAxisRaw(_horizontalInput);
            NavigateOptions();
        }    
    }
    // ================================== NAVIGATION ====================================
    private void NavigateOptions()
    {
        if(_inputValue != 0)
        {
            if(!_canNavigate) return;
            StartCoroutine("DelayInput");
            if(_inputValue > 0)
                _onNextOption.Invoke();
            else if(_inputValue < 0)
                _onPreviousOption.Invoke();
            _onAllOptions.Invoke();
        }
    }

    // ================================== BUTTON STATES ====================================
    public void Select()
    {
        if(_highlightSprite != null) _buttonImage.sprite = _highlightSprite;
        _buttonImage.color = _highlightColor;
        _isSelected = true;
        _onSelect.Invoke();
    }

    public void Deselect()
    {
        if(_highlightSprite != null) _buttonImage.sprite = _defaultSprite;
        _buttonImage.color = _defaultColor;
        _isSelected = false;
        _onDeselect.Invoke();
    }

    // Delay Input by a certain duration
    IEnumerator DelayInput() 
    {
        _canNavigate = false;
        yield return new WaitForSeconds(_navigationDelay);
        _canNavigate = true;
    }

}
