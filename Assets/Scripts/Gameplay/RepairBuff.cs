// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairBuff : MonoBehaviour
{
    [Tooltip("Amount of hit points that the robot will regerate")]
    [SerializeField] private float       _repairAmount        = 10.0f;
    [Tooltip("If this is true, repair will be done over time rather than instantly")]
    [SerializeField] private bool        _isRepairOverTime    = false;
    [SerializeField] private float       _repairDuration      = 3.0f;
    [SerializeField] private GameObject  _repairParticles     = null;
    [Header("Audio")]
    [SerializeField] private AudioSource _repairSound         = null;   

    private HitPoints   _hp                 = null;
    private bool        _canRepair           = true;
    private float       _repairCounter      = 0;    
    private float       _particleLifetime   = 2.0f;
    private bool        _repairStarted      = false;

    // Repair robot on trigger or over a certain duration
    private void OnTriggerEnter(Collider other) 
    {
        _hp = other.GetComponent<HitPoints>();
        if(_hp == null) return;
        if(_canRepair == false) return;
        _canRepair = false;
        GameObject particles = Instantiate(_repairParticles, other.transform.position, _repairParticles.transform.rotation, other.transform);
        Destroy(particles, _particleLifetime);
        if(_repairSound != null) _repairSound.Play();
        if(_isRepairOverTime == false)
        {
            _hp.AddHitPoints(_repairAmount);
            Destroy(gameObject);
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
            _repairStarted = true;            
        }
    }
    // ================================== MONOBEHAVIOUR ====================================

    private void Update() 
    {
        if(_hp == null) return;
        if(_repairStarted == true) RepairOverTime();
    }

    // ================================== REPAIR ====================================

    private void RepairOverTime()
    {

        if(_repairCounter < _repairDuration)
        {
            _repairCounter += Time.deltaTime;
            _hp.AddHitPoints((Time.deltaTime/_repairDuration) * _repairAmount); 
        }
        else
            Destroy(gameObject);
    }
}
