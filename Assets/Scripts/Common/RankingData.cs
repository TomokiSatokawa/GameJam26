using System;
using System.Collections.Generic;

[Serializable]
public class RankingEntry
{
    public int customerCount; // 捌いた客の数
    public int totalScore;    // 総合スコア
    public bool isBackground; // 背景を表示するかどうか
}

[Serializable]
public class RankingData
{
    public List<RankingEntry> rankings = new List<RankingEntry>();
}
