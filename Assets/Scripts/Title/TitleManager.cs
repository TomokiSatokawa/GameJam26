using Common.Sound;
using Common.UI;
using CoreKit.DynamicEnum;
using CoreKit.SceneLoading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Title
{
    public class TitleManager : MonoBehaviour
    {
        [SerializeField] private FadeImageControl _fade;
        [SerializeField] private string _inGameSceneName;
        [SerializeField] private DynamicEnumValue _bgm;

        private void Start()
        {
            SoundManager.BGM.Play(_bgm);
        }

        public async void StartInGame()
        {
            var fade = _fade.FadeOut();
            var scene = SceneLoader.LoadAsync(_inGameSceneName, token: destroyCancellationToken);

            //非同期で読み込みつつフェードアウトする
            await UniTask.WhenAll(fade, scene);

            //Scene移動
            await SceneLoader.TryMoveSceneAsync();
        }
    }
}
