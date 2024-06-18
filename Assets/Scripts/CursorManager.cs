using Unity.VisualScripting;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
  public static CursorManager _Instance { get; set; }

  [SerializeField] private bool _Enabled;
  [Header("General")]
  [SerializeField] private Texture2D _Default;
  [Header("Movement")] 
  [SerializeField] private Texture2D _Left;
  [SerializeField] private Texture2D _LeftForward;
  [SerializeField] private Texture2D _LeftBack;
  [SerializeField] private Texture2D _Right;
  [SerializeField] private Texture2D _RightForward;
  [SerializeField] private Texture2D _RightBack;
  [SerializeField] private Texture2D _Back;
  [SerializeField] private Texture2D _Forward;

  private void Awake()
  {
    if (_Instance != null && _Instance != this)
      Destroy(this.gameObject);
    else
      _Instance = this;
  }

  private void Start()
  {
    if (_Enabled)
      SetDefault();
  }

  public void SetDefault()
  {
    if (_Enabled)
      Cursor.SetCursor(_Default, new Vector2(13, 13), CursorMode.ForceSoftware);
  }

  public void SetMovement(Vector3 direction)
  {
    if (!_Enabled)
      return;

    if (direction.x > 0.0f && direction.z > 0.0f)
    {
      Cursor.SetCursor(_RightForward, new Vector2(32, 0), CursorMode.ForceSoftware);
      return;
    }
    else if (direction.x > 0.0f && direction.z < 0.0f)
    {
      Cursor.SetCursor(_RightBack, new Vector2(32, 32), CursorMode.ForceSoftware);
      return;
    }
    else if (direction.x > 0.0f)
    {
      Cursor.SetCursor(_Right, new Vector2(32, 16), CursorMode.ForceSoftware);
      return;
    }

    if (direction.x < 0.0f && direction.z > 0.0f)
    {
      Cursor.SetCursor(_LeftForward, new Vector2(0, 0), CursorMode.ForceSoftware);
      return;
    }
    else if (direction.x < 0.0f && direction.z < 0.0f)
    {
      Cursor.SetCursor(_LeftBack, new Vector2(0, 32), CursorMode.ForceSoftware);
      return;
    }
    else if (direction.x < 0.0f)
    {
      Cursor.SetCursor(_Left, new Vector2(0, 16), CursorMode.ForceSoftware);
      return;
    }

    if (direction.z > 0.0f)
    {
      Cursor.SetCursor(_Forward, new Vector2(16, 0), CursorMode.ForceSoftware);
      return;
    }
    if (direction.z < 0.0f)
    {
      Cursor.SetCursor(_Back, new Vector2(16, 32), CursorMode.ForceSoftware);
      return;
    }

    SetDefault();
  }
}
