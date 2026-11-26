using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public float gameTime = 90f; //  ゲーム全体の制限時間
    public float customerTime = 10f; //お客1人当たりの制限時間
    private bool isRunning; //タイマーが動いているかを確認
    private GameManager gameManager;
    private void Start()
    {
        gameManager = GetComponent<GameManager>();
    }
    private void Update()
    {
        if(!isRunning)
        {
            return;
        }

        gameTime -= Time.deltaTime; //ゲーム全体の制限時間を減らす
        customerTime -= Time.deltaTime; //お客の制限時間を減らす
        Debug.Log("全体時間：" + gameTime);
        Debug.Log("お客時間：" + customerTime);

        if (gameTime <= 0)   
        {
            gameTime = 0;
            isRunning = false;
            gameManager.EndGame();
        }

        if (customerTime <= 0)
        {
            customerTime = 0;
        }
    }
    public void StartTimer()
    {
        gameTime = 90f;
        customerTime = 10f;
        isRunning = true; //タイマーを動作状態にする
    }

    public void ResetCustomerTimer()
    {
        customerTime = 10f;
    }

    public void StopTimer()
    {
        isRunning = false; //タイマーを停止状態にする
    }
}
