using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace InGame.UI
{
    /// <summary>
    /// ゲーム終了時の「終了！！」演出を再生する。
    /// </summary>
    public class GameEndView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _textRoot;
        [SerializeField] private float _popDuration = 0.4f;
        [SerializeField] private float _holdDuration = 1.0f;
        [SerializeField] private float _fadeDuration = 0.3f;
        [SerializeField] private float _startScale = 3f;

        private void Awake()
        {
            _canvasGroup.alpha = 0f;
        }

        private void OnDestroy()
        {
            _textRoot.DOKill();
            _canvasGroup.DOKill();
        }

        /// <summary>
        /// 「終了！！」演出を再生し、完了まで待機する。
        /// </summary>
        public async UniTask PlayAsync(CancellationToken cancellationToken)
        {
            _canvasGroup.alpha = 0f;
            _textRoot.localScale = Vector3.one * _startScale;

            var sequence = DOTween.Sequence()
                .Append(_canvasGroup.DOFade(1f, _popDuration * 0.5f))
                .Join(_textRoot.DOScale(1f, _popDuration).SetEase(Ease.OutBack))
                .AppendInterval(_holdDuration)
                .Append(_canvasGroup.DOFade(0f, _fadeDuration));

            try
            {
                await sequence.ToUniTask(cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException)
            {
                sequence.Kill();
                _canvasGroup.alpha = 0f;
                throw;
            }
        }
    }
}