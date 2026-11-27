using Common.Utility;
using TMPro;
using UnityEngine;

public class RankingItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rankText;

    [SerializeField] private TextMeshProUGUI _scoreText;

    [SerializeField] private GameObject _BackgroundObject;

    public void SetData(int rank, RankingEntry entry)
    {
        if (_rankText != null) _rankText.text = $"{KanjiNumeral.ToKanji(rank)}位";
        if (_scoreText != null) _scoreText.text = $"{KanjiNumeral.ToKanji(entry.customerCount)}人";
        if (_BackgroundObject != null) _BackgroundObject.SetActive(entry.isBackground);
    }
}
