using System;
using UnityEngine;
/// <summary>
///   基础属性值
/// </summary>
[Serializable]
public class Stats
{
    [SerializeField]
    private float baseValue;

    public Stats(float baseValue = 100)
    {
        this.baseValue = baseValue;
    }

    public float GetValue() => baseValue;
}