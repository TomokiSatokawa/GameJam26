using R3;
using UnityEngine;

public class InGameUIPresenter : MonoBehaviour
{
    [SerializeField] private TimerManager _timerManager;
    [SerializeField] private TimerUI _timerUI;

    private void Start()
    {
        _timerManager.GameTimer.Subscribe(x => _timerUI.UpdateGameTimer(x));
        _timerManager.CustomerTimer.Subscribe(x => _timerUI.UpdateCustomerTimer(x));
    }
}
