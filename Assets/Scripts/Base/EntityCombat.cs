using System.Linq;
using UnityEngine;

public class EntityCombat : MonoBehaviour
{
    public Collider2D[] colliders;
    public float attackRange = 1f;
    public LayerMask targetLayer;
    public int damage = 10;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    /// <summary>
    /// 获取攻击范围内的所有目标
    /// </summary>
    private Collider2D[] GetColliders()
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, targetLayer);
        return colliders;
    }

    public void PerformAttack()
    {
        foreach (var item in GetColliders().Where(x => x != null && x.transform != transform))
        {
            if (item.GetComponent<EntityHealth>() != null)
            {
                item.GetComponent<EntityHealth>().TakeDamage(damage, item.transform);
            }
        }
    }

}