// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowOnTrigger : MonoBehaviour
{
    [SerializeField] private float                  _slowDuration = 1.0f;
    [SerializeField][Range(0,0.999f)] private float _slowAmount   = 0.5f;

    private void OnTriggerStay(Collider other) 
    {
        SlowDebuff slowDebuff = other.gameObject.GetComponent<SlowDebuff>();
        if(slowDebuff == null) 
        {
            slowDebuff = (SlowDebuff) other.gameObject.AddComponent(typeof(SlowDebuff)); 
        }
        slowDebuff.ResetDebuff(_slowDuration, _slowAmount);
    }
}
