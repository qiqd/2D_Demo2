using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实体状态的抽象基类，所有具体状态（如 idle、move、attack 等）都应继承此类。
/// 采用状态机模式，将行为封装到独立的状态对象中，便于管理和扩展。
/// </summary>
public abstract class EntityState
{


    /// <summary>玩家控制器引用，子类可通过此访问玩家的数据和行为</summary>
    protected Player player;

    /// <summary>持有当前状态的状态机引用，用于在状态内部切换状态</summary>
    protected StateMachine stateMachine;

    /// <summary>状态的名称标识，方便调试和日志输出</summary>
    protected string condition;

    public bool stateTriggerCalled = false;

    /// <summary>
    /// 构造函数，创建状态时传入玩家、所属状态机和状态名称
    /// </summary>
    /// <param name="player">玩家控制器实例</param>
    /// <param name="stateMachine">管理该状态的状态机实例</param>
    /// <param name="condition">状态的名称</param>
    public EntityState(Player player, StateMachine stateMachine, string condition)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.condition = condition;
    }

    /// <summary>
    /// 进入该状态时调用，用于初始化状态相关的逻辑（如播放动画、重置计时器等）
    /// </summary>
    public virtual void Enter()
    {
        player.animator.SetBool(condition, true);
    }

    /// <summary>
    /// 退出该状态时调用，用于清理状态相关的资源（如停止动画、保存状态数据等）
    /// </summary>
    public virtual void Exit()
    {
        player.animator.SetBool(condition, false);
    }

    /// <summary>
    /// 每帧更新时调用，用于处理状态的持续逻辑（如移动、检测输入、判断状态转换条件等）
    /// </summary>
    public abstract void Update();
}