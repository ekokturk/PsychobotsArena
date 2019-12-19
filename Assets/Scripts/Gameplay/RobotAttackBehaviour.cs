// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAttackBehaviour : StateMachineBehaviour
{
    [Tooltip("Interval where the overlap sphere for collision will spawn (0 to 1)")]
    [SerializeField] private Vector2 _attackTime = new Vector2(0,1);   // Time interval for collision detection

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float time = stateInfo.normalizedTime;                          // Get animation time that is mapped to 0-1
        if(time >= _attackTime.x && time <= _attackTime.y)              // Check if animation is at desired interval
        {
            animator.GetComponent<RobotAttack>()?.Attack();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<RobotAttack>()?.Clear();
        animator.GetComponent<AController>()?.StopRobotAttack();
    }

}
