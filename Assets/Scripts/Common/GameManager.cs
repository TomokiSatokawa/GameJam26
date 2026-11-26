using Common.Order;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState //ゲームの進行状態
    {
        Playing,
        Result
    }

    public GameState currentState;
    public TimerManager timerManager;
    public CameraWork _cameraWork;
    [SerializeField] private SpeechBubble _speechBubble;
    [SerializeField] private OrderFormatter _orderFormatter;
    [SerializeField] private ToppingCounter _toppingCounter;
    [SerializeField] private PrefabSpawner _prefabSpawner;  
    [SerializeField] private int _maxCount;
    //public BubbleSpawner bubbleSpawner;
    //public MistakeChecker mistakeChecker;
    //public SpawnManager spawnManager;

    void Start()
    {
        currentState = GameState.Playing;
        ScoreManager.ResetScore();
        timerManager.StartTimer();
        InitializeCustomer();

        _toppingCounter.penalty += () => Debug.Log("Miss");
        timerManager.OnGameTimerEnd += EndGame;
        timerManager.OnCustomerTimerEnd += () => EndCustomer().Forget();
    }

    private void InitializeCustomer()
    {
        var orderData = new OrderData(_maxCount);
        _speechBubble.ShowSpeechBubble(_orderFormatter.Format(orderData));
        _toppingCounter.SetData(orderData);
    }

    public void EndGame() //ゲームを終了してリザルトにする
    {
        currentState = GameState.Result;
        timerManager.PauseTimer(false);
    }

    public async UniTaskVoid EndCustomer()
    {
        timerManager.PauseTimer(false);
        AddCustomer();
        await _cameraWork.Slide();
        timerManager.PauseTimer(true);
        InitializeCustomer();
        _prefabSpawner.SpawnNext();
    }

    public void AddCustomer() //提供したお客の人数を増やす
    {
        ScoreManager.AddCustomer();
        timerManager.ResetCustomerTimer(); //お客の制限時間を10秒に戻す
    }
}