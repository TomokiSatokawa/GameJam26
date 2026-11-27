using Common.Order;
using Common.Sound;
using Cysharp.Threading.Tasks;
using Generated.DynamicEnums;
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
    [SerializeField] private CountdownView _countdownView;
    [SerializeField] private OrderLevelDatabase _orderLevelDatabase;
    [SerializeField] private int _maxCount;
    [SerializeField] private float _waitTime;
    //public BubbleSpawner bubbleSpawner;
    //public MistakeChecker mistakeChecker;
    //public SpawnManager spawnManager;

    void Start()
    {
        _toppingCounter.penalty += () => SoundManager.SE.PlayOneShot(SEType.Miss);
        _toppingCounter.Completed += () => EndCustomer().Forget();
        timerManager.OnGameTimerEnd += EndGame;
        timerManager.OnCustomerTimerEnd += () => EndCustomer().Forget();

        Initialize().Forget();
    }

    private async UniTaskVoid Initialize()
    {
        var token =   _ingredientsGenerator.IgnoreInput.RegisterFlag();
        _speechBubble.HideSpeechBubbleImmediate();
        timerManager.StartTimer();
        ScoreManager.ResetScore();
        timerManager.PauseTimer(false);

        await _countdownView.PlayAsync(destroyCancellationToken);

        SoundManager.BGM.Play(BGMType.InGameBGM);

        timerManager.PauseTimer(true);
        currentState = GameState.Playing;
        ResetCustomer();

        _ingredientsGenerator.IgnoreInput.UnregisterFlag(token);
    }

    private void ResetCustomer()
    {
        var orderData = _orderLevelDatabase.CreateOrderData(ScoreManager.CustomerCount);
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

        SoundManager.SE.PlayOneShot(SEType.CustomerClear);

        currentState = GameState.Moving;
        _speechBubble.HideSpeechBubble();
        _prefabSpawner.SpawnNext();
        timerManager.PauseTimer(false);
        var token = _ingredientsGenerator.IgnoreInput.RegisterFlag();
        ScoreManager.AddCustomer(); //提供したお客の人数を増やす
        await UniTask.WaitForSeconds(_waitTime);

        async UniTask CustmerAsync()
        {
            await UniTask.WaitForSeconds(_cameraWork.SildeDuration / 2);
            ResetCustomer();
        }

        await UniTask.WhenAll(_cameraWork.Slide(), CustmerAsync());

        timerManager.PauseTimer(true);
        _ingredientsGenerator.IgnoreInput.UnregisterFlag(token);
        currentState = GameState.Playing;
    }
}