using Common.UI;
using Common.Utility;
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
    public StackedImageView yasaiScore;         // リザルト：乗せた野菜の数
    public StackedImageView ninnikuScore;       // （任意）リザルト：ニンニクの数
    public StackedImageView aburaScore;         // （任意）リザルト：アブラの数

    private bool resultGenerated;

    private void Start()
    {
        if (resultPanel != null) resultPanel.SetActive(false);
        if (inGamePanel != null) inGamePanel.SetActive(true);
    }

    private void Update()
    {
        // 2. インゲーム中は総合スコアのテキストだけを更新する
        if (inGameTotalScoreText != null)
        {
            inGameTotalScoreText.text = ScoreManager.TotalScore.ToString();
        }
    }

    public void ShowResult()
    {
        if (rankingManager != null)
        {
            rankingManager.SaveCurrentScore();
        }
        // インゲームUIを隠してリザルトパネルを表示
        if (inGamePanel != null) inGamePanel.SetActive(false);
        if (resultPanel != null) resultPanel.SetActive(true);

        // リザルトの各数値を反映
        if (resultTotalScoreText != null) resultTotalScoreText.text = KanjiNumeral.ToKanji(ScoreManager.TotalScore) + "点";
        if (customerScoreText != null) customerScoreText.text = KanjiNumeral.ToKanji(ScoreManager.CustomerCount) + "人";
        yasaiScore.SetData(ScoreManager.YasaiScore);
        ninnikuScore.SetData(ScoreManager.NinnikuScore);
        aburaScore.SetData(ScoreManager.AburaScore);
    }
    // ScoreUI.cs のフィールドに追加
    public RankingManager rankingManager;

}
