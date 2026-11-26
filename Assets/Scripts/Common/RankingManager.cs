using System.IO;
using System.Linq;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(
            Application.persistentDataPath,
            "ranking.dat"
        );
    }

    public void AddRanking()
    {
        RankingData data = LoadRanking();
        RankingEntry entry = new RankingEntry();

        entry.playerName = "player";
        entry.customerCount = ScoreManager.CustomerCount;
        data.rankings.Add(entry);
        SaveRanking(data);
    }

    public void SaveRanking(RankingData data)
    {
        data.rankings = data.rankings
            .OrderByDescending(x => x.customerCount)
            .Take(20)
            .ToList();

        string json = JsonUtility.ToJson(data);
        string encrypatedData = AESCipher.Encrypt(json);
        File.WriteAllText(savePath, encrypatedData);
    }

    public RankingData LoadRanking()
    {
        if(!File.Exists(savePath))
        {
            return new RankingData();
        }

        string encrypatedData = File.ReadAllText(savePath);
        string json = AESCipher.Decrypt(encrypatedData);
        return JsonUtility.FromJson<RankingData>(json);
    }
}