using UnityEngine;

public class CameraController : MonoBehaviour
{
  public static CameraController _Instance { get; set; }

  [SerializeField] private Camera _Camera;
  [SerializeField] private LayerMask _GroundMask;
  [SerializeField] private float _Speed;
  [SerializeField] private float _BorderPanWidth;
  [SerializeField] private float _ZoomSpeed;
  [SerializeField] private float _RotationSpeed;
  [SerializeField] private float _MinDoubleClickTime;

  private Vector2 _SpeedFloatSpeed;
  private Vector2 _SpeedFloatStart;
  private Vector3 _RotationPoint;
  private float _RotationStart;
  private float _CurrentRotation;
  private float _LastWheelClickedTime;
  private float _CurrentZoom;

  private void Awake()
  {
    if (_Instance != null && _Instance != this)
      Destroy(this.gameObject);
    else
      _Instance = this;
  }

  private void Start()
  {
    _SpeedFloatStart = Vector2.zero;
    _SpeedFloatSpeed = Vector2.zero;

    Physics.Raycast(_Camera.transform.position, _Camera.transform.TransformDirection(Vector3.forward), out var hit, Mathf.Infinity, _GroundMask);
    _RotationPoint = hit.point;

    _RotationStart = 0;
    _CurrentRotation = 0;
    _LastWheelClickedTime = 0;
    _CurrentZoom = 0;
  }

  void Update()
  {
    // [START] Arrow keys camera movement and Screen border camera movement 
    /*
    Vector3 movement_direction = Vector3.zero;
    if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x > Screen.width - _BorderPanWidth)
    {
      Vector3 movement = Vector3.right * _Speed * Time.deltaTime;
      movement_direction += movement;
      transform.Translate(movement, Space.Self);
    }
    if (Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x < _BorderPanWidth)
    {
      Vector3 movement = Vector3.left * _Speed * Time.deltaTime;
      movement_direction += movement;
      transform.Translate(movement, Space.Self);
    }
    if (Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y > Screen.height - _BorderPanWidth)
    {
      Vector3 movement = Vector3.forward * _Speed * Time.deltaTime;
      movement_direction += movement;
      transform.Translate(Vector3.forward * _Speed * Time.deltaTime, Space.Self);
    }
    if (Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y < _BorderPanWidth)
    {
      Vector3 movement = Vector3.back * _Speed * Time.deltaTime;
      movement_direction += movement;
      transform.Translate(Vector3.back * _Speed * Time.deltaTime, Space.Self);
    }
    CursorManager._Instance.SetMovement(movement_direction);
    */
    // [END] Arrow keys camera movement and Screen border camera movement 

    // [START] Speedfloating camera movement
    if (Input.GetMouseButtonDown(1))
      _SpeedFloatStart = Input.mousePosition;

    if (Input.GetMouseButton(1))
    {
      _SpeedFloatSpeed.x = _SpeedFloatStart.x - Input.mousePosition.x;
      _SpeedFloatSpeed.y = _SpeedFloatStart.y - Input.mousePosition.y;

      transform.Translate(new Vector3(-1.0f, 0.0f, 0.0f) * _SpeedFloatSpeed.x * Time.deltaTime, Space.Self);
      transform.Translate(new Vector3(0.0f, 0.0f, -1.0f) * _SpeedFloatSpeed.y * Time.deltaTime, Space.Self);
    }

    if (Input.GetMouseButtonUp(1))
      _SpeedFloatStart = Vector2.zero;
    // [END] Speedfloating camera movement

    // [START] Camera zooming
    if (Input.mouseScrollDelta.y > 0)
    {
      float zoom_level = _ZoomSpeed * Time.deltaTime;
      _Camera.transform.Translate(Vector3.forward * zoom_level, Space.Self);
      _CurrentZoom += zoom_level;
    }
    if (Input.mouseScrollDelta.y < 0)
    {
      float zoom_level = _ZoomSpeed * Time.deltaTime;
      _Camera.transform.Translate(Vector3.back * _ZoomSpeed * Time.deltaTime, Space.Self);
      _CurrentZoom -= zoom_level;
    }
    // [END] Camera zooming

    // [START] Camera rotation and reset
    Physics.Raycast(_Camera.transform.position, _Camera.transform.TransformDirection(Vector3.forward), out var hit, Mathf.Infinity, _GroundMask);
    _RotationPoint = hit.point;

    if (Input.GetMouseButtonDown(2))
    {
      if ((Time.time - _LastWheelClickedTime) <= _MinDoubleClickTime)
      {
        if (_CurrentRotation > 0)
          transform.RotateAround(_RotationPoint, new Vector3(0.0f, -1.0f, 0.0f), Mathf.Abs(_CurrentRotation));
        if (_CurrentRotation < 0)
          transform.RotateAround(_RotationPoint, new Vector3(0.0f, 1.0f, 0.0f), Mathf.Abs(_CurrentRotation));
        _CurrentRotation = 0;
      }
      _LastWheelClickedTime = Time.time;
      _RotationStart = Input.mousePosition.x;
    }

    if (Input.GetMouseButton(2))
    {
      float rotation_angle = Mathf.Abs(Input.mousePosition.x - _RotationStart) * _RotationSpeed * Time.deltaTime;
      if (Input.mousePosition.x > _RotationStart)
      {
        _CurrentRotation += rotation_angle;
        transform.RotateAround(_RotationPoint, new Vector3(0.0f, 1.0f, 0.0f), rotation_angle);
        _RotationStart = Input.mousePosition.x;
      }
        
      if (Input.mousePosition.x < _RotationStart)
      {
        _CurrentRotation -= rotation_angle;
        transform.RotateAround(_RotationPoint, new Vector3(0.0f, -1.0f, 0.0f), rotation_angle);
        _RotationStart = Input.mousePosition.x;
      }
    }

    if (Input.GetMouseButtonUp(2))
      _RotationStart = 0;
    // [END] Camera rotation and reset

    // [START] Camera rotation and camera zooming reset
    if (Input.GetKeyDown(KeyCode.Space))
    {
      if (_CurrentRotation > 0)
        transform.RotateAround(_RotationPoint, new Vector3(0.0f, -1.0f, 0.0f), Mathf.Abs(_CurrentRotation));
      if (_CurrentRotation < 0)
        transform.RotateAround(_RotationPoint, new Vector3(0.0f, 1.0f, 0.0f), Mathf.Abs(_CurrentRotation));

      _CurrentRotation = 0;

      if (_CurrentZoom > 0)
        _Camera.transform.Translate(Vector3.back * _CurrentZoom, Space.Self);
      if (_CurrentZoom < 0)
        _Camera.transform.Translate(Vector3.forward * _CurrentZoom, Space.Self);

      _CurrentZoom = 0;
    }
    // [END] Camera rotation and camera zooming reset
  }

  public bool IsSpeedFloating()
  {
    if (_SpeedFloatSpeed.x != 0 && _SpeedFloatSpeed.y != 0)
      return true;
    return false;
  }
}
