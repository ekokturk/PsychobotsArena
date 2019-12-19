// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTexture : MonoBehaviour
{
    [SerializeField] private float _scrollSpeedX = 0;
    [SerializeField] private float _scrollSpeedY = 0;

    private Renderer _renderer;

    private void Awake() 
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        _renderer.material.mainTextureOffset = new Vector2(Time.time * _scrollSpeedX, Time.time * _scrollSpeedY);
    }
}
