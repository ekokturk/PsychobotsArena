// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentAttackBehaviour : StateMachineBehaviour
{
    [Tooltip("Interval where the overlap sphere for collision will spawn (0 to 1)")]
    [SerializeField] private Vector2 _attackTime = new Vector2(0,1);   // Time interval for collision detection

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float time = stateInfo.normalizedTime;                          // Get animation time that is mapped to 0-1
        if(time >= _attackTime.x && time <= _attackTime.y)              // Check if animation is at desired interval
        {
            animator.GetComponent<MeleeComponent>()?.Hit();
        }
    }


}
