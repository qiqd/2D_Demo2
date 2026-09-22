using UnityEngine;

public abstract class EntityState
{
    public Animator animator;
    public StateMachine stateMachine;
    public string condition;
    public bool stateTriggerCalled = false;

    public EntityState(Animator animator, StateMachine stateMachine, string condition)
    {
        this.animator = animator;
        this.stateMachine = stateMachine;
        this.condition = condition;
    }

    public virtual void Enter()
    {
        animator.SetBool(condition, true);
    }

    public virtual void Exit()
    {
        animator.SetBool(condition, false);
    }

    public abstract void Update();
}