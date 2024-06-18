using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdleState : StateMachineBehaviour
{
  private UnitAttackController _Controller;

  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
  {
    _Controller = animator.GetComponent<UnitAttackController>();
  }

  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
  {
    if (_Controller._Target != null)
    {
      animator.SetBool("IsFollowing", true);
    }
  }
}
