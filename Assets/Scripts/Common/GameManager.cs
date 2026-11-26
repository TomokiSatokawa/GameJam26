using Common.Order;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState //ゲームの進行状態
    {
        Playing,
        Moving,
        Result
    }

    public GameState currentState;
    public TimerManager timerManager;
    public CameraWork _cameraWork;
    [SerializeField] private SpeechBubble _speechBubble;
    [SerializeField] private OrderFormatter _orderFormatter;
    [SerializeField] private ToppingCounter _toppingCounter;
    [SerializeField] private PrefabSpawner _prefabSpawner;
    [SerializeField] private IngredientsGenerator _ingredientsGenerator;
    [SerializeField] private int _maxCount;
    [SerializeField] private float _waitTime;
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
        _toppingCounter.Completed += () => EndCustomer().Forget();
        timerManager.OnGameTimerEnd += EndGame;
        timerManager.OnCustomerTimerEnd += () => EndCustomer().Forget();
    }

    private void InitializeCustomer()
    {
        var orderData = new OrderData(_maxCount);
        _speechBubble.ShowSpeechBubble(_orderFormatter.Format(orderData), orderData);
        _toppingCounter.SetData(orderData);
        timerManager.ResetCustomerTimer(); //お客の制限時間を10秒に戻す
    }

    public void EndGame() //ゲームを終了してリザルトにする
    {
        currentState = GameState.Result;
        timerManager.PauseTimer(false);
    }

    public async UniTaskVoid EndCustomer()
    {
        if (currentState != GameState.Playing)
            return;
        currentState = GameState.Moving;
        _speechBubble.HideSpeechBubble();
        _prefabSpawner.SpawnNext();
        timerManager.PauseTimer(false);
        var token = _ingredientsGenerator.IgnoreInput.RegisterFlag();
        ScoreManager.AddCustomer(); //提供したお客の人数を増やす
        await UniTask.WaitForSeconds(_waitTime);
        await _cameraWork.Slide();
        timerManager.PauseTimer(true);
        InitializeCustomer();
        _ingredientsGenerator.IgnoreInput.UnregisterFlag(token);
        currentState = GameState.Playing;
    }
}