using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gameTimerText;
    [SerializeField] private TextMeshProUGUI _customerTimerText;

    public void UpdateGameTimer(float value)
    {
        _gameTimerText.text = Mathf.CeilToInt(value).ToString();
    }

    public void UpdateCustomerTimer(float value)
    {
        _customerTimerText.text = Mathf.CeilToInt(value).ToString();
    }
}