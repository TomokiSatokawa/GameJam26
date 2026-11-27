using UnityEngine;

public class TitleRankingUI : MonoBehaviour
{
    [Header("参照設定")]
    [SerializeField] private RankingManager _rankingManager;
    [SerializeField] private GameObject _rankingPanel;
    [SerializeField] private Transform _listParent;
    [SerializeField] private RankingItem _rankingItemPrefab;

    private void Start()
    {
        if (_rankingPanel != null)
        {
            _rankingPanel.SetActive(false);
        }
    }

    public void OpenRanking()
    {
        Debug.Log("① OpenRanking が呼ばれました");
        if (_rankingPanel != null) _rankingPanel.SetActive(true);
        CreateRankingList();
    }

    public void CloseRanking()
    {
        if (_rankingPanel != null) _rankingPanel.SetActive(false);
    }

    private void CreateRankingList()
    {
        if (_rankingManager == null || _listParent == null || _rankingItemPrefab == null)
        {
            Debug.LogError($"② 枠が空欄です！ Manager:{_rankingManager} / Parent:{_listParent} / Prefab:{_rankingItemPrefab}");
            return;
        }

        foreach (Transform child in _listParent)
        {
            Destroy(child.gameObject);
        }   

        RankingData data = _rankingManager.LoadRanking();
        Debug.Log($"③ 読み込んだデータ件数: {data.rankings.Count} 件");

        for (int i = 0; i < data.rankings.Count; i++)
        {
            RankingItem item = Instantiate(_rankingItemPrefab, _listParent);
            item.SetData(i + 1, data.rankings[i]);
            Debug.Log($"④ {i + 1}位を生成しました: {item.name}");
        }
    }
}
