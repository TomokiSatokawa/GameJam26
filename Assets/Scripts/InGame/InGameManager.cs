using Common.Sound;
using Common.UI;
using CoreKit.DynamicEnum;
using CoreKit.SceneLoading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    [SerializeField] private FadeImageControl _fade;
    [SerializeField] private string _titleName;
    [SerializeField] private DynamicEnumValue _bgm;

    private void Start()
    {
    }

    /// <summary>
    /// テスト用コード
    /// Titleからコピー
    /// </summary>
    public async void StartInGame()
    {
        var fade = _fade.FadeOut();
        var scene = SceneLoader.LoadAsync(_titleName, token: destroyCancellationToken);

        //非同期で読み込みつつフェードアウトする
        await UniTask.WhenAll(fade, scene);

        //Scene移動
        await SceneLoader.TryMoveSceneAsync();
    }
}
