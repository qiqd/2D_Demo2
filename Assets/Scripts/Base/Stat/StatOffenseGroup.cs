/// <summary>
/// 攻击属性组
/// </summary>
public class StatOffenseGroup
{
    /// <summary>
    /// 攻击伤害
    /// </summary>
    public Stats damage = new Stats(10);
    /// <summary>
    /// 暴击伤害
    /// </summary>
    public Stats critPower = new Stats();
    /// <summary>
    /// 暴击率
    /// </summary>
    public Stats critChance = new Stats();
    /// <summary>
    /// 火伤害
    /// </summary>
    public Stats fireDamage = new Stats();
    /// <summary>
    /// 冰伤害
    /// </summary>
    public Stats iceDamage = new Stats();
    /// <summary>
    /// 闪电伤害
    /// </summary>
    public Stats lightningDamage = new Stats();
}