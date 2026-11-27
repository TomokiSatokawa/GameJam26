using TMPro;
using UnityEngine;

public class RankingItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rankText;

    [SerializeField] private TextMeshProUGUI _scoreText;

    public void SetData(int rank, RankingEntry entry)
    {
        if (_rankText != null) _rankText.text = $"{rank}位";
        if (_scoreText != null) _scoreText.text = $"{entry.totalScore}点";
    }
}
