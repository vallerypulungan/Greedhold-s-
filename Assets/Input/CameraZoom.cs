using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Camera camera;
    private float zoomTarget;

    [SerializeField]
    private float multiplier = 2f, minZoom = 1f, maxZoom = 10f, smoothTime = 0.1f;
    private float velocity = 0f;

    void Start()
    {
        camera = GetComponent<Camera>();
        zoomTarget = camera.orthographicSize;
    }

    void Update()
    {
        zoomTarget -= Input.GetAxisRaw("Mouse ScrollWheel") * multiplier;
        zoomTarget = Mathf.Clamp(zoomTarget, minZoom, maxZoom);
        camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, zoomTarget, ref velocity, smoothTime);
    }
}
