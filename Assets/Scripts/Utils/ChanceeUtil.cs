using UnityEngine;

public class ChanceUtil
{

    /// <summary>
    /// 获取是否发生概率事件
    /// </summary>
    /// <param name="rate">概率</param>
    /// <returns></returns>
    public static bool GetChance(float rate = 10)
    {
        return Random.Range(0, 100) < rate;
    }
}