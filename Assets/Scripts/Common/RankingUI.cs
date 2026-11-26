using UnityEngine;

public class RankingUI : MonoBehaviour
{
    public RankingManager rankingManager;
    public RankingItem rankingItemPrefab;
    public Transform content;

    private void Start()
    {
        RankingData data = rankingManager.LoadRanking();

        for(int i = 0; i < data.rankings.Count; i++)
        {
            RankingItem item = Instantiate(rankingItemPrefab, content);
            item.SetData(i + 1, data.rankings[i]);
        }
    }
}
