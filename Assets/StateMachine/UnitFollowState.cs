using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitFollowState : StateMachineBehaviour
{
  private UnitAttackController _Controller;
  private NavMeshAgent _Agent;
  private float _MaxAttackDistance = 3.0f;

  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
  {
    _Controller = animator.GetComponent<UnitAttackController>();
    _Agent = animator.GetComponent<NavMeshAgent>();
  }

  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if (_Controller._Target == null)
    {
      animator.SetBool("IsFollowing", false);
    }

    if (_Controller._Target != null)
    {
      _Agent.SetDestination(_Controller._Target.position);
      animator.transform.LookAt(_Controller._Target);

      float distance_from_target = Vector3.Distance(animator.transform.position, _Controller._Target.position);

      if (distance_from_target <= _MaxAttackDistance)
      {
        animator.SetBool("IsAttacking", true);
      }
    }
  }

  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    _Agent.SetDestination(animator.transform.position);
  }
}
