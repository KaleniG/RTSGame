using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
  private void Start()
  {
    UnitManager._Instance._AllUnits.Add(this.gameObject);
  }

  private void OnDestroy()
  {
    UnitManager._Instance._AllUnits.Remove(this.gameObject);
  }
}
