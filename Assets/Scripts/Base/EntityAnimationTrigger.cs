using UnityEngine;

public class EntityAnimationTrigger : MonoBehaviour
{
    public Entity entity;

    void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    protected void OnTriggerCalled()
    {
        entity.stateMachine.currentState.stateTriggerCalled = true;
    }
}