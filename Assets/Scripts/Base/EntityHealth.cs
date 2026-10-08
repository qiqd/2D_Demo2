using System;
using UnityEngine;
using UnityEngine.Rendering;

public class EntityHealth : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    public EntityVFX entityVFX;

    void Awake()
    {
        entityVFX = GetComponent<EntityVFX>();
        var stats = GetComponent<EntityStats>();
        if (stats != null)
        {
            maxHP = Convert.ToInt32(stats.GetMaxHP());
            currentHP = maxHP;
        }

    }

    public virtual void ReduceHP(int damage)
    {
        if (currentHP <= 0)
        {
            Die();
            return;
        }
        currentHP -= damage;
        if (entityVFX != null)
        {
            entityVFX.SetDamageMaterial();
        }
    }

    public virtual void TakeDamage(int damage, Transform ts)
    {
        if (ts != null && ts.GetComponent<EntityHealth>() != null)
        {
            ts.GetComponent<EntityHealth>().ReduceHP(damage);
        }
    }
    public virtual void Die()
    {
        Debug.Log(transform.tag + " Die");
    }

}