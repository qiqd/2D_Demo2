using UnityEngine;

public class SkeletonIdleState : EntityState
{
    private Skeleton skeleton;
    private float timer = 0;
    private float maxIdleTime = 3;
    public SkeletonIdleState(Skeleton skeleton, StateMachine stateMachine, string condition) : base(skeleton.animator, stateMachine, condition)
    {
        this.skeleton = skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        timer = 0;
    }
    public override void Update()
    {
        // if (timer >= maxIdleTime)
        // {
        //     stateMachine.ChangeState(skeleton.walkState);
        // }
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F))
        {
            stateMachine.ChangeState(skeleton.attackState);
        }
    }
}