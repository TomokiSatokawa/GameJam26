using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gameTimerText;
    [SerializeField] private TextMeshProUGUI _customerTimerText;

    public void UpdateGameTimer(float value)
    {
        _gameTimerText.text = value.ToString("N2");
    }

    public void UpdateCustomerTimer(float value)
    {
        _customerTimerText.text = value.ToString("N2");
    }
}