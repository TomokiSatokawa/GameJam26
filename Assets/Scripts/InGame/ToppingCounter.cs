using System;
using UnityEngine;

public class ToppingCounter : MonoBehaviour
{
    private int _ninnikuCount;
    private int _yasaiCount;
    private int _aburaCount;

    private OrderData _currecntData;

    public event Action penalty;

    public void SetData(OrderData orderData)
    {
        _currecntData = orderData;
    }

    public void AddNinniku(int amount = 1)
    {
        _ninnikuCount += amount;
        CheckCount(_ninnikuCount, _currecntData.NinnikuCount);
    }

    /// <summary>
    /// 野菜のスコアを加算する
    /// </summary>
    public void AddYasai(int amount = 1)
    {
        _yasaiCount += amount;
        CheckCount(_yasaiCount, _currecntData.YasaiCount);
    }

    /// <summary>
    /// アブラのスコアを加算する
    /// </summary>
    public void AddAbura(int amount = 1)
    {
        _aburaCount += amount;
        CheckCount(_aburaCount, _currecntData.AburaCount);
    }

    private void CheckCount(int currentCount, int maxCount)
    {
        if (currentCount > maxCount)
            penalty?.Invoke();
    }
}
