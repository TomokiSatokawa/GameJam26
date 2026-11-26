using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [Header("インゲーム用UI（プレイ中に表示）")]
    public GameObject inGamePanel;                 // インゲームHUDの親（リザルト時に隠すならセット）
    public TextMeshProUGUI inGameTotalScoreText;   // インゲーム中の総合スコア

    [Header("リザルト用UI（終了時に表示）")]
    public GameObject resultPanel;
    public TextMeshProUGUI resultTotalScoreText;   // リザルト：総合スコア
    public TextMeshProUGUI customerScoreText;      // リザルト：捌いた客の数
    public TextMeshProUGUI yasaiScoreText;         // リザルト：乗せた野菜の数
    public TextMeshProUGUI ninnikuScoreText;       // （任意）リザルト：ニンニクの数
    public TextMeshProUGUI aburaScoreText;         // （任意）リザルト：アブラの数

    [Header("参照")]
    public GameManager gameManager;

    private bool resultGenerated;

    private void Start()
    {
        if (resultPanel != null) resultPanel.SetActive(false);
        if (inGamePanel != null) inGamePanel.SetActive(true);
    }

    private void Update()
    {
        if (gameManager == null) return;

        // 1. リザルト状態になったら1回だけリザルトUIを表示する
        if (gameManager.currentState == GameManager.GameState.Result)
        {
            if (!resultGenerated)
            {
                ShowResult();
                resultGenerated = true;
            }
            return;
        }

        // 2. インゲーム中は総合スコアのテキストだけを更新する
        if (inGameTotalScoreText != null)
        {
            inGameTotalScoreText.text = ScoreManager.TotalScore.ToString();
        }
    }

    private void ShowResult()
    {
        // インゲームUIを隠してリザルトパネルを表示
        if (inGamePanel != null) inGamePanel.SetActive(false);
        if (resultPanel != null) resultPanel.SetActive(true);

        // リザルトの各数値を反映
        if (resultTotalScoreText != null) resultTotalScoreText.text = ScoreManager.TotalScore.ToString();
        if (customerScoreText != null) customerScoreText.text = ScoreManager.CustomerCount.ToString();
        if (yasaiScoreText != null) yasaiScoreText.text = ScoreManager.YasaiScore.ToString();
        if (ninnikuScoreText != null) ninnikuScoreText.text = ScoreManager.NinnikuScore.ToString();
        if (aburaScoreText != null) aburaScoreText.text = ScoreManager.AburaScore.ToString();
    }
}
