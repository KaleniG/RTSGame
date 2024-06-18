using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitAttackController : MonoBehaviour
{
  [NonSerialized] public Transform _Target;

  private void Start()
  {
    _Target = null;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Enemy") && _Target == null)
    {
      _Target = other.transform;
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.CompareTag("Enemy") && _Target != null)
    {
      _Target = null;
    }
  }
}
