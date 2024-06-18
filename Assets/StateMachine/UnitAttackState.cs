using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttackState : StateMachineBehaviour
{
  private NavMeshAgent _Agent;
  private UnitAttackController _Controller;
  private float _MaxAttackDistance = 3.0f;

  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    _Agent = animator.GetComponent<NavMeshAgent>();
    _Controller = animator.GetComponent<UnitAttackController>();
  }

  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if (_Controller._Target != null)
    {
      animator.transform.LookAt(_Controller._Target);

      float distance_from_target = Vector3.Distance(animator.transform.position, _Controller._Target.position);
      if (distance_from_target > _MaxAttackDistance)
        animator.SetBool("IsAttacking", false);

    }
    if (_Controller._Target == null)
    {
      animator.SetBool("IsAttacking", false);
      animator.SetBool("IsFollowing", false);
    }
  }

  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {

  }
}
