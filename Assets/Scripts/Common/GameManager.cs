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
    public int customerCount; //’ñ‹Ÿ‚µ‚½‚¨‹q‚Ìl”
    public TimerManager timerManager;
    //public BubbleSpawner bubbleSpawner;
    //public MistakeChecker mistakeChecker;
    //public SpawnManager spawnManager;
    public TitleManager titleManager;


    void Start()
    {
        currentState = GameState.Playing;
        customerCount = 0;
        timerManager.StartTimer();
        //bubbleSpawner.ShowBubble();
        ScoreManager.ResetScore();
    }

    public void EndGame() //ƒQ[ƒ€‚ğI—¹‚µ‚ÄƒŠƒUƒ‹ƒg‚É‚·‚é
    {
        currentState = GameState.Result;
        timerManager.StopTimer();
    }

    public void AddCustomer() //’ñ‹Ÿ‚µ‚½‚¨‹q‚Ìl”‚ğ‘‚â‚·
    {
        customerCount++;
        timerManager.ResetCustomerTimer(); //‚¨‹q‚Ì§ŒÀŠÔ‚ğ10•b‚É–ß‚·
        //bubbleSpawner.ShowBubble();
        //mistakeChecker.ResetInput();
        //spawnManager.StartSpawn();
        ScoreManager.AddCustomer();
    }
}
