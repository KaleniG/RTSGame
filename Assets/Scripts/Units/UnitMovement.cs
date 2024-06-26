using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
  [SerializeField] private LayerMask _GroundMask;

  private Camera _Camera;
  private NavMeshAgent _NavAgent;
  private GameObject _Traget;

  private void Start()
  {
    _Camera = Camera.main;
    _NavAgent = GetComponent<NavMeshAgent>();
  }

  private void Update()
  {
    if (Input.GetMouseButtonUp(0))
    {
      RaycastHit hit;
      Ray move_position = _Camera.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(move_position, out hit, Mathf.Infinity, _GroundMask) && !UnitDragSelectionManger._Instance.IsDragging())
      {
        _NavAgent.SetDestination(hit.point);
      }
    }

    if (_Traget != null)
      _NavAgent.SetDestination(_Traget.transform.position);
  }

  public void SetTarget(GameObject traget)
  {
    _Traget = traget;
  }
}
