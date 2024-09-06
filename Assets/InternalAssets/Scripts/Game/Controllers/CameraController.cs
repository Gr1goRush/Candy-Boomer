using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float targetWidth = 5.4f;

    void Start()
    {
        _camera.orthographicSize = targetWidth / _camera.aspect / 2f;
    }
}
