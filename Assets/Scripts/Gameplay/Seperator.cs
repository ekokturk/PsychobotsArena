// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seperator : MonoBehaviour
{
    [Header("Seperator Settings")]
    [Tooltip("Parts that will be detached (They are required to have a collider that is disabled)")]
    [SerializeField] private List<GameObject>   _mainParts              = null;
    [Tooltip("Force that will be applied to detached component in upwards direction")]
    [SerializeField] private float              _detachForce            = 20f;
    [SerializeField] private AudioSource        _partDetachSFX          = null;


    // ================================== REMOVE COMPONENTS ====================================
    public void RemoveMainParts()
    {
        foreach (GameObject part in _mainParts)
        {
            RemovePart(part);
        }
    }

    protected void RemovePart(GameObject part)
    {
        if(part == null)
            return;
        Vector3 forceDirection = (part.transform.position - transform.position).normalized;
        if(_partDetachSFX != null && _partDetachSFX.isPlaying == false) _partDetachSFX.Play();
        part.transform.parent = null;
        part.GetComponent<Collider>().enabled = true;
        Rigidbody rigidbody = (Rigidbody) part.AddComponent(typeof(Rigidbody)); 
        rigidbody?.AddForce(forceDirection * _detachForce, ForceMode.Impulse);
    }
}
