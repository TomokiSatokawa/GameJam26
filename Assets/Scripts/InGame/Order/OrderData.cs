using UnityEngine;

/// <summary>
/// 注文データを管理する
/// </summary>
public class OrderData
{
    private int _yasaiCount;
    private int _ninnikuCount;
    private int _aburaCount;

    public int YasaiCount => _yasaiCount;
    public int NinnikuCount => _ninnikuCount;
    public int AburaCount => _aburaCount;
    public int TotalCount => _yasaiCount + _ninnikuCount + _aburaCount;

    /// <summary>
    /// 合計個数をminTotal～maxTotalから決め、3種類にランダムに振り分けて生成する
    /// </summary>
    public OrderData(int minTotal, int maxTotal)
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
    }

    public override string ToString()
    {
        return $"yasai {_yasaiCount} ninniku {_ninnikuCount} abura {_aburaCount}";
    }
}