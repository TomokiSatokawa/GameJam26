//AI
using System.Collections.Generic;
using Common.Order;
using UnityEngine;

/// <summary>
/// 注文データを管理する
/// </summary>
public class OrderData
{
    private int _yasaiCount;
    private int _ninnikuCount;
    private int _aburaCount;

    private readonly List<IngredientCount> _entries = new();

    public int YasaiCount => _yasaiCount;
    public int NinnikuCount => _ninnikuCount;
    public int AburaCount => _aburaCount;
    public int TotalCount => _yasaiCount + _ninnikuCount + _aburaCount;

    /// <summary>
    /// 注文を言う順番。同じ具材が複数回に分かれて出ることがあり、
    /// その具材の各回の個数を足すと総個数(YasaiCountなど)になる
    /// </summary>
    public IReadOnlyList<IngredientCount> Entries => _entries;

    /// <summary>
    /// 合計個数をminTotal～maxTotalから決め、3種類にランダムに振り分けて生成する。
    /// その後、注文を言う順番(Entries)を適度にランダムにして保存する
    /// </summary>
    /// <param name="swapChance">隣り合う具材の順番を入れ替える確率</param>
    /// <param name="splitChance">各具材を2回に分けて言う確率</param>
    public OrderData(int minTotal, int maxTotal, float swapChance = 0.3f, float splitChance = 0.5f)
    {
        if (minTotal > maxTotal)
        {
            Debug.LogError($"[OrderData] 最小値が最大値を超えています。 min={minTotal} max={maxTotal}");
            maxTotal = minTotal;
        }

        int total = Random.Range(minTotal, maxTotal + 1);
        for (int i = 0; i < total; i++)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    _yasaiCount++;
                    break;
                case 1:
                    _ninnikuCount++;
                    break;
                default:
                    _aburaCount++;
                    break;
            }
        }

        CreateEntries(swapChance, splitChance);
    }

    public override string ToString()
    {
        return $"yasai {_yasaiCount} ninniku {_ninnikuCount} abura {_aburaCount} entries [{string.Join(", ", _entries)}]";
    }

    /// <summary>
    /// やさい→にんにく→あぶらを基本順として、入れ替えと分割で若干順不同にしたEntriesを作る
    /// </summary>
    private void CreateEntries(float swapChance, float splitChance)
    {
        AddEntry(Ingredients.Yasai, _yasaiCount);
        AddEntry(Ingredients.Ninniku, _ninnikuCount);
        AddEntry(Ingredients.Abura, _aburaCount);

        SwapNeighbors(swapChance);
        SplitEntries(splitChance);
    }

    private void AddEntry(Ingredients ingredient, int count)
    {
        // 0個の具材は注文に含めない
        if (count <= 0)
        {
            return;
        }

        _entries.Add(new IngredientCount(ingredient, count));
    }

    /// <summary>
    /// 隣り合う要素を確率で入れ替える。1回入れ替えた要素は続けて動かさない
    /// </summary>
    private void SwapNeighbors(float swapChance)
    {
        for (int i = 0; i < _entries.Count - 1; i++)
        {
            if (Random.value >= swapChance)
            {
                continue;
            }

            (_entries[i], _entries[i + 1]) = (_entries[i + 1], _entries[i]);
            i++;
        }
    }

    /// <summary>
    /// 各具材を確率で2回に分ける
    /// </summary>
    private void SplitEntries(float splitChance)
    {
        // 処理中に要素が増えるため、元の要素を控えてから回す
        var targets = new List<IngredientCount>(_entries);
        foreach (var target in targets)
        {
            SplitEntry(target, splitChance);
        }
    }

    /// <summary>
    /// 個数を2つに分け、前半は元の位置に残し、後半をそれより後ろのランダムな位置に挿入する。
    /// 同じ具材が連続してしまう場合は分割せず、まとめて1回で言う
    /// </summary>
    private void SplitEntry(IngredientCount target, float splitChance)
    {
        // 2個未満は分けられない
        if (target.Count < 2 || Random.value >= splitChance)
        {
            return;
        }

        // この時点では各具材は未分割なので、同じ具材の要素は1つだけ
        int index = _entries.FindIndex(x => x.Ingredient == target.Ingredient);

        // index+1 ～ 末尾 のどこかに後半を入れる
        int insertIndex = Random.Range(index + 1, _entries.Count + 1);

        // すぐ後ろだと同じ具材が連続するため、分割をやめてまとめて言う
        if (insertIndex == index + 1)
        {
            return;
        }

        // 前半を1～(総個数-1)から決め、残りを後半にする(合計は総個数のまま)
        int firstCount = Random.Range(1, target.Count);
        _entries[index] = new IngredientCount(target.Ingredient, firstCount);
        _entries.Insert(insertIndex, new IngredientCount(target.Ingredient, target.Count - firstCount));
    }
}