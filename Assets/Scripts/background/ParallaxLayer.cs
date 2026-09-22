using System;
using UnityEngine;

[Serializable]
public class ParallaxLayer
{
    // 该层的父节点。它下面可以挂多张首尾相接的 sprite（比如 Tower 下的 TowerLeft / TowerRight），
    // 整组会作为一个整体一起移动和循环，因此组内相对位置永远不变，永远不会重叠或断开。
    [SerializeField]
    public Transform background;

    [SerializeField]
    public float parallaxMultiplier = 0.1f;
    private float backgroundHalfWidth;

    public void GetBackgroundHalfWidth() => backgroundHalfWidth = background.GetComponent<SpriteRenderer>().bounds.size.x / 2;

    public void MoveLayer(float moveDistance)
    {
        if (background != null)
        {
            background.position += new Vector3(moveDistance * parallaxMultiplier, 0, 0);
        }
    }

    public void LoopLayer(float cameraLeftEdge, float cameraRightEdge)
    {
        float backgroundLeftEdge = background.transform.position.x - backgroundHalfWidth;
        float backgroundRightEdge = background.transform.position.x + backgroundHalfWidth;
        if (backgroundLeftEdge > cameraRightEdge)
        {
            background.position += Vector3.right * -backgroundHalfWidth * 2;
        }
        else if (backgroundRightEdge < cameraLeftEdge)
        {
            background.position += Vector3.right * backgroundHalfWidth * 2;
        }
    }
}

