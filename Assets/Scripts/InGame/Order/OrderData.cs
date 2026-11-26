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

    /// <summary>
    /// 注文データを生成する
    /// </summary>
    public OrderData(int maxCount)
    {
        _yasaiCount = Random.Range(0, maxCount + 1);
        _ninnikuCount = Random.Range(0, maxCount + 1);
        _aburaCount = Random.Range(0, maxCount + 1);
    }

    public override string ToString()
    {
        return $"yasai {_yasaiCount} ninniku {_ninnikuCount} abura {_aburaCount}";
    }
}