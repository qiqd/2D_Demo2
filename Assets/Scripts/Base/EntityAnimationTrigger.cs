using UnityEngine;

public class EntityAnimationTrigger : MonoBehaviour
{
    public Entity entity;
    public EntityCombat entityCombat;

    void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    protected void OnTriggerCalled()
    {
        entity.stateMachine.currentState.stateTriggerCalled = true;
    }

    protected void OnAttackOver()
    {
        entityCombat.GetColliders();
    }
}