using UnityEngine;

public class UnitDragSelectionManger : MonoBehaviour
{
  public static UnitDragSelectionManger _Instance { get; set; }

  [SerializeField] private RectTransform _BoxVisual;
  [SerializeField] private Vector2 _MinBoxSize;

  private Rect _SelectionBox;

  private Camera _Camera;
  private Vector2 _StartPosition;
  private Vector2 _EndPosition;

  private void Awake()
  {
    if (_Instance != null && _Instance != this)
      Destroy(this.gameObject);
    else
      _Instance = this;
  }

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

  private void SelectUnits()
  {
    if (IsDragging() && IsDragBoxContaining() && !(Input.GetKey(KeyCode.LeftShift)))
      UnitManager._Instance.DeselectAll();

    foreach (GameObject unit in UnitManager._Instance._AllUnits)
      if (_SelectionBox.Contains(_Camera.WorldToScreenPoint(unit.transform.position)))
        UnitManager._Instance.DragSelect(unit);
  }

  public bool IsDragBoxContaining()
  {
    bool contains_any = false;

    foreach (GameObject unit in UnitManager._Instance._AllUnits)
      contains_any = _SelectionBox.Contains(_Camera.WorldToScreenPoint(unit.transform.position)) || contains_any;

    return contains_any;
  }

  public bool IsDragging()
  {
    if (_SelectionBox.size.x > 0.0f && _SelectionBox.size.y > 0.0f)
      return true;
    return false;
  }
}