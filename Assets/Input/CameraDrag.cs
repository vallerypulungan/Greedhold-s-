using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class CameraDrag : MonoBehaviour
{
    [Header("Drag Settings")]
    [SerializeField] private TilemapCollider2D tilemapCollider;

    private Vector3 _origin;
    private Vector3 _difference;
    private bool _isDragging;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 200f;
    [SerializeField] private float minZoom = 1f;
    [SerializeField] private float maxZoom = 10f;
    [SerializeField] private float zoomSmoothTime = 0.1f;

    private float _zoomTarget;
    private float _zoomVelocity;

    private Bounds _cameraBounds;
    private Bounds _worldBounds;
    private Camera _mainCamera;
    private Vector3 _targetPosition;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _zoomTarget = _mainCamera.orthographicSize;
        _worldBounds = tilemapCollider.bounds;
        UpdateCameraBounds(); // Initial boundaries
    }

    private void LateUpdate()
    {
        ApplyZoom();
        UpdateCameraBounds(); // Update boundary after zoom change
        HandleDrag();
    }

    private void HandleDrag()
    {
        if (!_isDragging) return;

        _difference = GetMouseWorldPosition - transform.position;
        _targetPosition = _origin - _difference;
        _targetPosition = ClampToBounds(_targetPosition);

        transform.position = _targetPosition;
    }

    private void ApplyZoom()
    {
        _mainCamera.orthographicSize = Mathf.SmoothDamp(
            _mainCamera.orthographicSize,
            _zoomTarget,
            ref _zoomVelocity,
            zoomSmoothTime
        );
    }

    private void UpdateCameraBounds()
    {
        float height = _mainCamera.orthographicSize;
        float width = height * _mainCamera.aspect;

        float minX = _worldBounds.min.x + width;
        float maxX = _worldBounds.max.x - width;
        float minY = _worldBounds.min.y + height;
        float maxY = _worldBounds.max.y - height;

        _cameraBounds.SetMinMax(new Vector3(minX, minY, 0f), new Vector3(maxX, maxY, 0f));
    }

    private Vector3 ClampToBounds(Vector3 position)
    {
        return new Vector3(
            Mathf.Clamp(position.x, _cameraBounds.min.x, _cameraBounds.max.x),
            Mathf.Clamp(position.y, _cameraBounds.min.y, _cameraBounds.max.y),
            transform.position.z
        );
    }

    private Vector3 GetMouseWorldPosition => _mainCamera.ScreenToWorldPoint(
        new Vector3(Mouse.current.position.ReadValue().x,
                    Mouse.current.position.ReadValue().y,
                    -_mainCamera.transform.position.z));

    public void OnDrag(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _origin = GetMouseWorldPosition;
            _isDragging = true;
        }

        if (ctx.canceled)
        {
            _isDragging = false;
        }
    }

    public void OnZoomIn(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _zoomTarget -= zoomSpeed * Time.deltaTime;
            _zoomTarget = Mathf.Clamp(_zoomTarget, minZoom, maxZoom);
        }
    }

    public void OnZoomOut(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _zoomTarget += zoomSpeed * Time.deltaTime;
            _zoomTarget = Mathf.Clamp(_zoomTarget, minZoom, maxZoom);
        }
    }
}
