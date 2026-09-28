/// <summary>
/// 状态机管理类，负责维护当前状态并处理状态之间的切换。
/// 所有实体状态由此类统一管理，确保状态转换的顺序和完整性。
/// </summary>
public class StateMachine
{
    /// <summary>当前激活的状态，外部只读，内部通过 Initialize 或 ChangeState 修改</summary>
    public EntityState currentState { get; private set; }

    /// <summary>
    /// 初始化状态机，设置初始状态并触发其 Enter 逻辑
    /// </summary>
    /// <param name="entityState">要设置为初始状态的状态实例</param>
    public void Initialize(EntityState entityState)
    {
        currentState = entityState;
        currentState.Enter();
    }

    /// <summary>
    /// 切换到新状态，先触发当前状态的 Exit 逻辑，再切换并触发新状态的 Enter 逻辑
    /// </summary>
    /// <param name="newState">要切换到的目标状态</param>
    public void ChangeState(EntityState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }
}