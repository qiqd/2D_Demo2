using UnityEngine;

public class EntityCombat : MonoBehaviour
{
    public Collider2D[] colliders;
    public float attackRange = 1f;
    public LayerMask targetLayer;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    /// <summary>
    /// 获取攻击范围内的所有目标
    /// </summary>
    public void GetColliders()
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, targetLayer);
    }

    public void PerformAttack()
    {
        foreach (var item in colliders)
        {

        }
    }

}