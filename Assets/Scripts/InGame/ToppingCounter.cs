using System;
using R3;
using UnityEngine;

public class ToppingCounter : MonoBehaviour
{
    [SerializeField] private  ComboSystem _comboSystem;
    private readonly ReactiveProperty<int> _ninnikuCount = new(0);
    private readonly ReactiveProperty<int> _yasaiCount = new(0);
    private readonly ReactiveProperty<int> _aburaCount = new(0);

    public ReadOnlyReactiveProperty<int> NinnikuCount => _ninnikuCount;
    public ReadOnlyReactiveProperty<int> YasaiCount => _yasaiCount;
    public ReadOnlyReactiveProperty<int> AburaCount => _aburaCount;

    private OrderData _currecntData;

    public event Action penalty;
    public event Action Completed;

    public void SetData(OrderData orderData)
    {
        _currecntData = orderData;
        _ninnikuCount.Value = 0;
        _yasaiCount.Value = 0;
        _aburaCount.Value = 0;
    }

    public void AddNinniku(int amount = 1)
    {
        if (_currecntData == null) return;
        ProcessTopping(_ninnikuCount, _currecntData.NinnikuCount, amount, ScoreManager.AddNinniku);
    }

    /// <summary>
    /// 野菜のスコアを加算する
    /// </summary>
    public void AddYasai(int amount = 1)
    {
        if (_currecntData == null) return;
        ProcessTopping(_yasaiCount, _currecntData.YasaiCount, amount, ScoreManager.AddYasai);
    }

    /// <summary>
    /// アブラのスコアを加算する
    /// </summary>
    public void AddAbura(int amount = 1)
    {
        if (_currecntData == null) return;
        ProcessTopping(_aburaCount, _currecntData.AburaCount, amount, ScoreManager.AddAbura);
    }

    private void ProcessTopping(ReactiveProperty<int> targetCount, int maxCount, int amount, Action<int> addScoreAction)
    {
        int nextCount = targetCount.Value + amount;

        if (nextCount <= maxCount) 
        {
            // 上限以内ならカウントとスコアを更新し、完了チェックを行う
            targetCount.Value = nextCount;
            addScoreAction?.Invoke(amount);
            CheckCompleted();
            _comboSystem.AddCombo();
        }
        else
        {
            // ★修正ポイント：上限を超えた場合はカウントを増やさず（maxCountのまま維持）、ペナルティだけ発火する
            penalty?.Invoke();
            _comboSystem.Miss();
        }
    }

    private void CheckCompleted()
    {
        if (_ninnikuCount.Value == _currecntData.NinnikuCount &&
            _yasaiCount.Value == _currecntData.YasaiCount &&
            _aburaCount.Value == _currecntData.AburaCount)
        {
            Completed?.Invoke();
        }
    }

    private void OnDestroy()
    {
        _ninnikuCount.Dispose();
        _yasaiCount.Dispose();
        _aburaCount.Dispose();
    }
}
