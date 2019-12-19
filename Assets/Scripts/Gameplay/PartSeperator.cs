// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HitPoints))]
///<summary>Detach gameobject from the parent when the hit points decreases</summary>
public class PartSeperator : Seperator
{
    [Header("Seperate based on HP")]
    [Tooltip("Parts that will be removed when damaged")]
    [SerializeField] private List<GameObject>   _basicParts             = null;
    [Tooltip("Particle effect that will be added to the detach part locations")]
    [SerializeField] private GameObject         _leftoverParticles      = null;
    [Header("Detachable Components")]
    [Tooltip("Left component that will be disassembled")]
    [SerializeField] private GameObject         _leftComponent          = null;
    [Tooltip("Right component that will be disassembled")]
    [SerializeField] private GameObject         _rightComponent         = null;

    private HitPoints   _hp                 = null;
    private float       _seperateHealth     = 0;
    private float       _healthStep         = 0;

    // ================================== MONOBEHAVIOUR ====================================
    private void Start()
    {
        // Setup parts based on health
        _hp = GetComponent<HitPoints>();
        if(_hp == null) return;
        if(_basicParts.Count > 0)
            _healthStep = 100.0f / (_basicParts.Count + 1);

        _seperateHealth = 100.0f - _healthStep;
        
    }

    private void Update() 
    {
        // Remove parts based on health
        if(_hp == null) return;
        if(_basicParts.Count > 0)
        {
            if(_seperateHealth >= _hp.HealthPercentage)
            {
                GameObject part = _basicParts[Random.Range(0, _basicParts.Count)];
                RemovePart(part);
                if(_leftoverParticles)
                {
                    GameObject particles = Instantiate(_leftoverParticles, part.transform.position, part.transform.rotation);
                    particles.transform.parent = gameObject.transform;
                }
                _basicParts.Remove(part);
                _seperateHealth -= _healthStep;
            }
        }
    }

    // Detach pieces from left and right components
    public void RemoveComponents()
    {
        Seperator leftSeperator  = _leftComponent.transform.GetChild(0).GetComponent<Seperator>();
        Seperator rightSeperator = _rightComponent.transform.GetChild(0).GetComponent<Seperator>();
        leftSeperator?.RemoveMainParts();
        rightSeperator?.RemoveMainParts();
    }

    

}
