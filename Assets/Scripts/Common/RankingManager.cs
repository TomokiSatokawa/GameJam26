using System.IO;
using System.Linq;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    [SerializeField] private int _maxRankCount = 10;

    private string SavePath => Path.Combine(Application.persistentDataPath, "ranking.json");

    public void SaveCurrentScore()
    {
        RankingData data = LoadRanking();

        RankingEntry newEntry = new RankingEntry
        {
            customerCount = ScoreManager.CustomerCount,
            totalScore = ScoreManager.TotalScore
        };

        data.rankings.Add(newEntry);

        data.rankings = data.rankings
            .OrderByDescending(x => x.totalScore)
            .ThenByDescending(x => x.customerCount)
            .Take(_maxRankCount)
            .ToList();

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);

        // ★保存成功確認ログ
        Debug.Log($"[Ranking] 保存完了！ スコア:{newEntry.totalScore} / 保存先:{SavePath}");
    }

    public RankingData LoadRanking()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log($"[Ranking] 保存ファイルがまだありません: {SavePath}");
            return new RankingData();
        }

        string json = File.ReadAllText(SavePath);
        RankingData data = JsonUtility.FromJson<RankingData>(json);
        Debug.Log($"[Ranking] 読み込み成功！ 件数:{data?.rankings.Count ?? 0}件");
        return data ?? new RankingData();
    }
}
