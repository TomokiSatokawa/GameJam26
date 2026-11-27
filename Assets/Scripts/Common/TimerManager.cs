using System;
using R3;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private float _maxGameTime = 90f; //  ゲーム全体の制限時間
    [SerializeField] private float _maxCustomerTime = 10f; //お客1人当たりの制限時間
    [SerializeField] private ComboSystem _comboSystem;

    private readonly ReactiveProperty<float> _gameTimer = new();
    private readonly ReactiveProperty<float> _customerTimer = new();
    public ReadOnlyReactiveProperty<float> GameTimer => _gameTimer;
    public ReadOnlyReactiveProperty<float> CustomerTimer => _customerTimer;

    private bool _isRunning; //タイマーが動いているかを確認

    public event Action OnGameTimerEnd;
    
    public event Action OnCustomerTimerEnd;

    private void Start()
    {
        _comboSystem.OnTimeAdded += AddGameTime;
    }

    private void Update()
    {
        if (!_isRunning)
        {
            return;
        }

        _gameTimer.Value -= Time.deltaTime; //ゲーム全体の制限時間を減らす
        _customerTimer.Value -= Time.deltaTime; //お客の制限時間を減らす
        //Debug.Log("全体時間：" + _gameTimer.Value);
        //Debug.Log("お客時間：" + _customerTimer.Value);

        if (_gameTimer.Value <= 0)
        {
            _gameTimer.Value = 0;
            _isRunning = false;
            OnGameTimerEnd?.Invoke();
        }

        if (_customerTimer.Value <= 0)
        {
            OnCustomerTimerEnd?.Invoke();
        }
    }

    public void StartTimer()
    {
        _gameTimer.Value = _maxGameTime;
        ResetCustomerTimer();
        _isRunning = true; //タイマーを動作状態にする
    }
    
    private void AddGameTime(int amount)
    {
        _gameTimer.Value += amount;
    }

    public void ResetCustomerTimer()
    {
        _customerTimer.Value = _maxCustomerTime; ;
    }

    public void PauseTimer(bool isRunning)
    {
        _isRunning = isRunning; //タイマーを停止状態にする
    }
}