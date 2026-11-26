using Cysharp.Threading.Tasks;
using Title;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState //ƒQ[ƒ€‚Ìisó‘Ô
    {
        Playing,
        Result
    }

    public GameState currentState;
    public TimerManager timerManager;
    public CameraWork _cameraWork;
    //public BubbleSpawner bubbleSpawner;
    //public MistakeChecker mistakeChecker;
    //public SpawnManager spawnManager;

    void Start()
    {
        currentState = GameState.Playing;
        ScoreManager.ResetScore();
        timerManager.StartTimer();

        timerManager.OnGameTimerEnd += EndGame;
        timerManager.OnCustomerTimerEnd += () => EndCustomer().Forget();
    }

    public void EndGame() //ƒQ[ƒ€‚ğI—¹‚µ‚ÄƒŠƒUƒ‹ƒg‚É‚·‚é
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
    }

    public void AddCustomer() //’ñ‹Ÿ‚µ‚½‚¨‹q‚Ìl”‚ğ‘‚â‚·
    {
        ScoreManager.AddCustomer();
        timerManager.ResetCustomerTimer(); //‚¨‹q‚Ì§ŒÀŠÔ‚ğ10•b‚É–ß‚·
    }
}
