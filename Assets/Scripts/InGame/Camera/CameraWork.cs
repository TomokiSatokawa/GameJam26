using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
    [Header("スライド設定")]
    [SerializeField] private float _slideAmount = 10f;
    [SerializeField] private float _slideDuration = 0.5f;
    [SerializeField] private Ease _animationEase = Ease.OutQuad;
    [SerializeField] private Transform _cameraTransform;

    [Header("シェイク設定")]
    [SerializeField] private float _shakeDuration = 0.3f;
    [SerializeField] private float _shakeStrength = 0.5f;
    [SerializeField] private int _shakeVibrato = 20;

    public float SlideAmount => Mathf.Abs(_slideAmount);

    public bool IsInputLocked { get; private set; }

    private CancellationTokenSource _lockCts;

    public async UniTask Slide()
    {
        float targetX = _cameraTransform.position.x + SlideAmount;
        await _cameraTransform.DOMoveX(targetX, _slideDuration).SetEase(_animationEase);
    }

    public void Shake()
    {
        _cameraTransform.DOComplete();
        _cameraTransform.DOShakePosition(_shakeDuration, _shakeStrength, _shakeVibrato);

        _lockCts?.Cancel();
        _lockCts?.Dispose();
        _lockCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

        LockInputAsync(_shakeDuration, _lockCts.Token).Forget();
    }

    private async UniTaskVoid LockInputAsync(float duration, CancellationToken token)
    {
        IsInputLocked = true;
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: token);
        }
        finally
        {
            if (!token.IsCancellationRequested)
            {
                IsInputLocked = false;
            }
        }
    }

    private void OnDestroy()
    {
        _lockCts?.Cancel();
        _lockCts?.Dispose();
    }
}
