using System;
using UnityEngine;
[Serializable]
public class EntityStats : MonoBehaviour
{
    [SerializeField]
    public Stats maxHp = new Stats();
    [SerializeField]
    public StatMajorGroup majorGroup;
    [SerializeField]
    public StatOffenseGroup offenseGroup;
    [SerializeField]
    public StatDefenseGroup defenseGroup;

    public float GetMaxHP() => maxHp.GetValue() + majorGroup.vital.vitality.GetValue() * 5;


}