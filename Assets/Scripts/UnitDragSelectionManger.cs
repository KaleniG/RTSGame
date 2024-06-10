using UnityEngine;

public class UnitDragSelectionManger : MonoBehaviour
{
  [SerializeField] private RectTransform _BoxVisual;
  [SerializeField] private Vector2 _MinBoxSize;

  static private Rect _SelectionBox;

  private Camera _Camera;
  private Vector2 _StartPosition;
  private Vector2 _EndPosition;

  private void Start()
  {
    _Camera = Camera.main;
    _StartPosition = Vector2.zero;
    _EndPosition = Vector2.zero;
    DrawVisual();
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      _StartPosition = Input.mousePosition;
      _SelectionBox = new Rect();
    }

    if (Input.GetMouseButton(0))
    {
      SelectUnitsGraphically();
      _EndPosition = Input.mousePosition;
      DrawVisual();
      DrawSelection();
    }

    if (Input.GetMouseButtonUp(0))
    {
      SelectUnits();
      _StartPosition = Vector2.zero;
      _EndPosition = Vector2.zero;
      DrawVisual();
    }
  }

  private void DrawVisual()
  {
    Vector2 boxStart = _StartPosition;
    Vector2 boxEnd = _EndPosition;
    _BoxVisual.position = (boxStart + boxEnd) / 2;
    _BoxVisual.sizeDelta = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));
  }

  private void DrawSelection()
  {
    if (Input.mousePosition.x < _StartPosition.x)
    {
      _SelectionBox.xMin = Input.mousePosition.x;
      _SelectionBox.xMax = _StartPosition.x;
    }
    else
    {
      _SelectionBox.xMin = _StartPosition.x;
      _SelectionBox.xMax = Input.mousePosition.x;
    }

    if (Input.mousePosition.y < _StartPosition.y)
    {
      _SelectionBox.yMin = Input.mousePosition.y;
      _SelectionBox.yMax = _StartPosition.y;
    }
    else
    {
      _SelectionBox.yMin = _StartPosition.y;
      _SelectionBox.yMax = Input.mousePosition.y;
    }
  }

  private void SelectUnitsGraphically()
  {
    bool contains_any = false;

    foreach (var unit in UnitManager._Instance._AllUnits)
      contains_any = _SelectionBox.Contains(_Camera.WorldToScreenPoint(unit.transform.position)) || contains_any;

    foreach (var unit in UnitManager._Instance._AllUnits)
    {
      if (_SelectionBox.Contains(_Camera.WorldToScreenPoint(unit.transform.position)))
        UnitManager._Instance.SelectGraphically(unit, true);
      else if (IsDragging() && contains_any && !Input.GetKey(KeyCode.LeftShift))
        UnitManager._Instance.SelectGraphically(unit, false);
    }
  }

  private void SelectUnits()
  {
    bool contains_any = false;

    foreach (var unit in UnitManager._Instance._AllUnits)
      contains_any = _SelectionBox.Contains(_Camera.WorldToScreenPoint(unit.transform.position)) || contains_any;

    if (IsDragging() && contains_any && !Input.GetKey(KeyCode.LeftShift))
      UnitManager._Instance.DeselectAll();

    foreach (var unit in UnitManager._Instance._AllUnits)
      if (_SelectionBox.Contains(_Camera.WorldToScreenPoint(unit.transform.position)))
        UnitManager._Instance.DragSelect(unit);
  }

  static public bool IsDragging()
  {
    if (_SelectionBox.size.x > 0.0f && _SelectionBox.size.y > 0.0f)
      return true;
    return false;
  }
}