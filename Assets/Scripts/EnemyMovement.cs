using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
  private NavMeshAgent _Agent;
  [SerializeField] private float _Speed;

  private void Start()
  {
    _Agent = GetComponent<NavMeshAgent>();
  }

  private void Update()
  {
    Vector3 destination = transform.position;
    if (Input.GetKey(KeyCode.A))
      destination += Vector3.left * _Speed;
    if (Input.GetKey(KeyCode.D))
      destination += Vector3.right * _Speed;
    if (Input.GetKey(KeyCode.W))
      destination += Vector3.forward * _Speed;
    if (Input.GetKey(KeyCode.S))
      destination += Vector3.back * _Speed;

    _Agent.SetDestination(destination);
  }
}
