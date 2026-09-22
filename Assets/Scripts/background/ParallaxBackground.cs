using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]
    public ParallaxLayer[] parallaxLayers;

    private Camera mainCamera;
    [SerializeField]
    public float lastCameraXPosition;
    private float cameraHalfWidth;

    void Start()
    {
        this.mainCamera = Camera.main;
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        foreach (var item in parallaxLayers)
        {
            item.GetBackgroundHalfWidth();
        }

    }

    void Update()
    {
        float currentXPosition = mainCamera.transform.position.x;
        float distance = currentXPosition - lastCameraXPosition;
        lastCameraXPosition = currentXPosition;
        float cameraLeftEdge = currentXPosition - cameraHalfWidth;
        float cameraRightEdge = currentXPosition + cameraHalfWidth;
        foreach (var item in parallaxLayers)
        {
            item.MoveLayer(distance);
            item.LoopLayer(cameraLeftEdge, cameraRightEdge);
        }

    }
}

