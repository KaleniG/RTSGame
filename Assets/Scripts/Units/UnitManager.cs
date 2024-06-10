using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class UnitManager : MonoBehaviour
{
  public static UnitManager _Instance { get; set; }

  private Camera _Camera;
  public List<GameObject> _AllUnits = new List<GameObject>();
  public List<GameObject> _SelectedUnits = new List<GameObject>();

  [SerializeField] private LayerMask _ClickableMask;
  [SerializeField] private LayerMask _GroundMask;
  [SerializeField] private GameObject _PathLandMark;

  private void Awake()
  {
    if (_Instance != null && _Instance != this)
    {
      Destroy(this.gameObject);
    }
    else
    {
      _Instance = this;
    }
  }

  private void Start()
  {
    _Camera = Camera.main;
  }

  private void Update()
  {
    if (Input.GetMouseButtonUp(0))
    {
      RaycastHit hit;
      Ray ray = _Camera.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(ray, out hit, Mathf.Infinity, _ClickableMask))
      {
        if (Input.GetKey(KeyCode.LeftShift))
          MultiSelect(hit.collider.gameObject); 
        else
          MonoSelect(hit.collider.gameObject);
      }
      else if (_SelectedUnits.Count > 0 && Physics.Raycast(ray, out hit, Mathf.Infinity, _GroundMask) && !UnitDragSelectionManger.IsDragging())
      {
        Destroy(Instantiate(_PathLandMark, hit.point, Quaternion.identity), 0.5f);
      }
    }

    if (Input.GetMouseButtonUp(1) && !CameraController.IsSpeedFloating())
      DeselectAll();
  }

  private void MonoSelect(GameObject unit)
  {
    DeselectAll();

    _SelectedUnits.Add(unit);

    unit.GetComponent<UnitMovement>().enabled = true;
    unit.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
  }

  private void MultiSelect(GameObject unit)
  {
    EnableAll(false);
    if (!_SelectedUnits.Contains(unit))
    {
      _SelectedUnits.Add(unit);
      unit.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
    }
    else
    {
      _SelectedUnits.Remove(unit);
      unit.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
    }
    EnableAll(true);
  }

  public void DragSelect(GameObject unit)
  {
    EnableAll(false);
    if (!_SelectedUnits.Contains(unit))
    {
      _SelectedUnits.Add(unit);
      unit.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
    }
    EnableAll(true);
  }

  public void DeselectAll()
  {
    foreach (GameObject unit in _SelectedUnits)
    {
      unit.GetComponent<UnitMovement>().enabled = false;
      unit.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
    }
    _SelectedUnits.Clear();
  }

  public void SelectGraphically(GameObject unit, bool select)
  {
    unit.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = select;
  }

  private void EnableAll(bool enable)
  {
    foreach (GameObject unit in _SelectedUnits)
      unit.GetComponent<UnitMovement>().enabled = enable;
  }
}
