using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レベルごとの注文合計個数(最小・最大)を管理するデータベース
/// </summary>
[CreateAssetMenu(fileName = "OrderLevelDatabase", menuName = "InGame/Order/OrderLevelDatabase")]
public class OrderLevelDatabase : ScriptableObject
{
    /// <summary>
    /// 1レベル分の合計個数の範囲
    /// </summary>
    [Serializable]
    public class LevelEntry
    {
        [SerializeField] private int _minTotal;
        [SerializeField] private int _maxTotal;

        public int MinTotal => _minTotal;
        public int MaxTotal => _maxTotal;
    }

    [Header("レベルごとの合計個数 (Element 0 = Level 1)")]
    [SerializeField] private List<LevelEntry> _levels = new List<LevelEntry>();

    [Header("成長曲線 (横軸: レベル, 縦軸: 合計個数) ※エディタ自動入力用")]
    [SerializeField] private int _levelCount = 20;
    [SerializeField] private AnimationCurve _minCurve = AnimationCurve.Linear(1f, 1f, 20f, 10f);
    [SerializeField] private AnimationCurve _maxCurve = AnimationCurve.Linear(1f, 2f, 20f, 30f);

    public int LevelCount => _levels.Count;

    /// <summary>
    /// 指定レベルの注文データを生成する(範囲外のレベルは最初/最後に丸める)
    /// </summary>
    public OrderData CreateOrderData(int level)
    {
        LevelEntry entry = GetEntry(level);
        if (entry == null)
        {
            return new OrderData(0, 0);
        }

        return new OrderData(entry.MinTotal, entry.MaxTotal);
    }

    private LevelEntry GetEntry(int level)
    {
        if (_levels.Count == 0)
        {
            Debug.LogError($"[OrderLevelDatabase] レベルデータが空です。 level={level}");
            return null;
        }

        int index = Mathf.Clamp(level - 1, 0, _levels.Count - 1);
        return _levels[index];
    }
}