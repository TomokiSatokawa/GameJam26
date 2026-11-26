using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TimerManager manager;
    public TextMeshProUGUI gameTimerText;
    public TextMeshProUGUI customerTimerText;

    public void Update()
    {
        gameTimerText.text = Mathf.CeilToInt(manager.gameTime).ToString();
        customerTimerText.text = Mathf.CeilToInt(manager.customerTime).ToString();
    }
}
