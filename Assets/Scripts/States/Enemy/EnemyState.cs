using UnityEngine;

public class EnemyState : EntityState
{
    public EnemyState(Animator animator, StateMachine stateMachine, string condition) : base(animator, stateMachine, condition)
    {
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}