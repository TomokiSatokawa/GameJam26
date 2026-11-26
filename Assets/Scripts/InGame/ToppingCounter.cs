using System;
using UnityEngine;

/// <summary>
/// トッピングの数を数え、注文との超過・コンプリートを判定して通知する
/// </summary>
public class ToppingCounter : MonoBehaviour
{
    private int _ninnikuCount;
    private int _yasaiCount;
    private int _aburaCount;

    private OrderData _currentData;

    public event Action penalty;
    public event Action Completed;

    /// <summary>
    /// 注文データを設定し、カウントをリセットする
    /// </summary>
    public void SetData(OrderData orderData)
    {
        _currentData = orderData;
        _ninnikuCount = 0;
        _yasaiCount = 0;
        _aburaCount = 0;
    }

    public void AddNinniku(int amount = 1)
    {
        _ninnikuCount += amount;
        CheckCount(_ninnikuCount, _currentData.NinnikuCount);
    }

    /// <summary>
    /// 野菜のスコアを加算する
    /// </summary>
    public void AddYasai(int amount = 1)
    {
        _yasaiCount += amount;
        CheckCount(_yasaiCount, _currentData.YasaiCount);
    }

    /// <summary>
    /// アブラのスコアを加算する
    /// </summary>
    public void AddAbura(int amount = 1)
    {
        _aburaCount += amount;
        CheckCount(_aburaCount, _currentData.AburaCount);
    }

    /// <summary>
    /// 超過ならpenalty、超過していなければコンプリート確認を行う
    /// </summary>
    private void CheckCount(int currentCount, int maxCount)
    {
        if (currentCount > maxCount)
        {
            penalty?.Invoke();
            return;
        }

        CheckComplete();
    }

    private void CheckComplete()
    {
        var isComplete = _ninnikuCount == _currentData.NinnikuCount
                         && _yasaiCount == _currentData.YasaiCount
                         && _aburaCount == _currentData.AburaCount;

        if (isComplete)
            Completed?.Invoke();
    }
}
