using Common.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultSceneChanger : MonoBehaviour
{
    [Header("シーン名設定")]
    [SerializeField] private string _titleSceneName = "Title"; // タイトル画面のシーン名

    /// <summary>
    /// 現在のゲームシーンを最初からやり直す（「もう一度プレイ」ボタン用）
    /// </summary>
    public void RetryGame()
    {
        SoundManager.BGM.ChangeVolume(0, 1f);

        Time.timeScale = 1f;
        ScoreManager.ResetScore();

        // 現在開いているシーンの名前を取得して再読み込みする
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    /// <summary>
    /// タイトルシーンへ戻る（「タイトルへ戻る」ボタン用）
    /// </summary>
    public void BackToTitle()
    {
        SoundManager.BGM.ChangeVolume(0, 1f);

        Time.timeScale = 1f;
        ScoreManager.ResetScore();

        SceneManager.LoadScene(_titleSceneName);
    }
}
