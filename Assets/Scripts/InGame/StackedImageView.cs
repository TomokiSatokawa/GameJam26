using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    /// <summary>
    /// 個数分のImageをContentの中心から上方向へ一定間隔で積み上げ、時差をつけて順番に生成し、個数をTMPに表示する
    /// </summary>
    public class StackedImageView : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private Image _imagePrefab;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private float _interval = 20f;
        [SerializeField] private float _startDelay = 0.2f;
        [SerializeField] private float _itemDelay = 0.1f;

        private readonly List<Image> _images = new();
        private CancellationTokenSource _cts;

        private void OnDestroy()
        {
            CancelAnimation();
        }

        /// <summary>
        /// 個数を反映し、全体の時差の後に1個ずつ順番に積み上げる
        /// </summary>
        public void SetData(int count)
        {
            if (count < 0)
            {
                Debug.LogError($"[StackedImageView] countが負の値です: {count}");
                count = 0;
            }

            CancelAnimation();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);

            _countText.text = count.ToString() + "コ";
            HideAll();

            PlayAsync(count, _cts.Token).Forget();
        }

        private async UniTaskVoid PlayAsync(int count, CancellationToken token)
        {
            if (await WaitAsync(_startDelay, token))
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                if (_images.Count <= i)
                {
                    _images.Add(CreateImage());
                }

                Image image = _images[i];
                // 横ランダム・回転ランダム
                int randomX = UnityEngine.Random.Range(-60, 60);
                float randomRot = UnityEngine.Random.Range(-70f, 70f);
                image.rectTransform.anchoredPosition = new Vector2(randomX, _interval * i);
                image.rectTransform.Rotate(new Vector3(0f, 0f, randomRot));
                image.gameObject.SetActive(true);

                if (await WaitAsync(_itemDelay, token))
                {
                    return;
                }
            }
        }

        // キャンセルされたらtrueを返す
        private async UniTask<bool> WaitAsync(float seconds, CancellationToken token)
        {
            if (seconds <= 0f)
            {
                return token.IsCancellationRequested;
            }

            return await UniTask.Delay(TimeSpan.FromSeconds(seconds), cancellationToken: token)
                .SuppressCancellationThrow();
        }

        private Image CreateImage()
        {
            Image image = Instantiate(_imagePrefab, _content);
            image.sprite = _sprite;

            // Contentの中心を基準に配置するため、アンカーを中央に固定する
            RectTransform rect = image.rectTransform;
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            return image;
        }

        private void HideAll()
        {
            foreach (Image image in _images)
            {
                image.gameObject.SetActive(false);
            }
        }

        private void CancelAnimation()
        {
            if (_cts == null)
            {
                return;
            }

            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }
}