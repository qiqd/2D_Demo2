using UnityEngine;

class PlayerAttackTrigger : MonoBehaviour
{
    private Player player;
    void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void AttackOver()
    {
        player.stateMachine.currentState.stateTriggerCalled = true;
    }
}