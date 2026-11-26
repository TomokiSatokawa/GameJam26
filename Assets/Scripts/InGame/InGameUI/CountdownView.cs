using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// 「3, 2, 1, スタート！」のカウントダウン演出を再生するView
/// </summary>
public class CountdownView : MonoBehaviour
{
    private const float PopDuration = 0.35f;
    private const float FadeOutDuration = 0.2f;

    [SerializeField] private TMP_Text _text;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private int _countFrom = 3;
    [SerializeField] private string _startMessage = "スタート！";
    [SerializeField] private float _numberDuration = 1f;
    [SerializeField] private float _startMessageDuration = 0.8f;
    [SerializeField] private float _popStartScale = 2f;

    private void Awake()
    {
        _canvasGroup.alpha = 0f;
    }

    /// <summary>
    /// カウントダウンを再生する。スタート！の表示が終わると完了する
    /// </summary>
    public async UniTask PlayAsync(CancellationToken token)
    {
        for (int count = _countFrom; count >= 1; count--)
        {
            await PlayStepAsync(count.ToString(), _numberDuration, token);
        }

        await PlayStepAsync(_startMessage, _startMessageDuration, token);
    }

    private async UniTask PlayStepAsync(string message, float duration, CancellationToken token)
    {
        _text.text = message;
        _text.transform.localScale = Vector3.one * _popStartScale;
        _canvasGroup.alpha = 0f;

        float holdDuration = Mathf.Max(0f, duration - PopDuration - FadeOutDuration);

        Sequence sequence = DOTween.Sequence()
            .Append(_text.transform.DOScale(1f, PopDuration).SetEase(Ease.OutBack))
            .Join(_canvasGroup.DOFade(1f, PopDuration * 0.6f))
            .AppendInterval(holdDuration)
            .Append(_canvasGroup.DOFade(0f, FadeOutDuration))
            .SetLink(gameObject);

        await sequence.ToUniTask(cancellationToken: token);
    }
}
