using Common.Sound;
using Common.UI;
using CoreKit.SceneLoading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultSceneChanger : MonoBehaviour
{
    [Header("シーン名設定")]
    [SerializeField] private string _titleSceneName = "Title"; // タイトル画面のシーン名


    [SerializeField] private FadeImageControl _fade;

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
    public async void BackToTitle()
    {
        var fade = _fade.FadeOut();
        var scene = SceneLoader.LoadAsync(_titleSceneName, token: destroyCancellationToken);

        SoundManager.BGM.ChangeVolume(0, 1f);

        //非同期で読み込みつつフェードアウトする
        await UniTask.WhenAll(fade, scene);

        //Scene移動
        await SceneLoader.TryMoveSceneAsync();

        SoundManager.BGM.ChangeVolume(0, 1f);

        Time.timeScale = 1f;
        ScoreManager.ResetScore();

        SceneManager.LoadScene(_titleSceneName);
    }
}
