using UnityEngine;
using TMPro;
public class RankingItem : MonoBehaviour
{
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI customerCountText;

    public void SetData(int rank, RankingEntry entry)
    {
        rankText.text = rank + "à ";
        playerNameText.text = entry.playerName;
        customerCountText.text = entry.customerCount + "êl";
    }
}
