using System;
using UnityEngine;
using R3;

/// <summary>
/// トッピングの数を数え、注文との超過・コンプリートを判定して通知する
/// </summary>
public class ToppingCounter : MonoBehaviour
{
    private readonly ReactiveProperty<int> _ninnikuCount = new();
    private readonly ReactiveProperty<int> _yasaiCount = new();
    private readonly ReactiveProperty<int> _aburaCount = new();

    private OrderData _currentData;

    public ReadOnlyReactiveProperty<int> NinnikuCount => _ninnikuCount;
    public ReadOnlyReactiveProperty<int> YasaiCount => _yasaiCount;
    public ReadOnlyReactiveProperty<int> AburaCount => _aburaCount;

    public event Action penalty;
    public event Action Completed;

    /// <summary>
    /// 注文データを設定し、カウントをリセットする
    /// </summary>
    public void SetData(OrderData orderData)
    {
        _currentData = orderData;
        _ninnikuCount.Value = 0;
        _yasaiCount.Value = 0;
        _aburaCount.Value = 0;
    }

    public void AddNinniku(int amount = 1)
    {
        AddCount(_ninnikuCount, amount, _currentData.NinnikuCount);
    }

    public void AddYasai(int amount = 1)
    {
        AddCount(_yasaiCount, amount, _currentData.YasaiCount);
    }

    public void AddAbura(int amount = 1)
    {
        AddCount(_aburaCount, amount, _currentData.AburaCount);
    }

    /// <summary>
    /// 注文数を超える場合はpenaltyを通知し、超えない場合は加算してコンプリート確認を行う
    /// </summary>
    private void AddCount(ReactiveProperty<int> currentCount, int amount, int maxCount)
    {
        if (currentCount.Value + amount > maxCount)
        {
            penalty?.Invoke();
            return;
        }

        currentCount.Value += amount;
        CheckComplete();
    }

    private void CheckComplete()
    {
        var isComplete = _ninnikuCount.Value == _currentData.NinnikuCount
                         && _yasaiCount.Value == _currentData.YasaiCount
                         && _aburaCount.Value == _currentData.AburaCount;

        if (isComplete)
            Completed?.Invoke();
    }
}