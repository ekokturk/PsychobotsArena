// PsychobotsArena @Copyright (C) 2019, Eser Kokturk. All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotUtilityBehaviour : StateMachineBehaviour
{
   private string        _utilityParameter   = "RobotUtility";
   private ARobotUtility _utility             = null;

   // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      _utility =  animator.GetComponent<ARobotUtility>();
      _utility?.Activate();
   }

   // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      if(_utility?.RechargeRatio == 0) animator.SetBool(_utilityParameter, false);
   }

   // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      _utility?.Deactivate();
   }


}
