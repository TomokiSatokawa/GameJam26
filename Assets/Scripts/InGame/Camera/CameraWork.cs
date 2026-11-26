using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
    [Header("スライド設定")]
    [SerializeField] private float _slideAmount;
    [SerializeField] private float _slideDuration;
    [SerializeField] private Ease _animationEase;
    [SerializeField] private Transform _cameraTransform;

    [Header("シェイク設定")]
    [SerializeField] private float _shakeDuration = 0.3f; 
    [SerializeField] private float _shakeStrength = 0.5f; 
    [SerializeField] private int _shakeVibrato = 20;    
    public float SlideAmount => Mathf.Abs(_slideAmount);
    public async UniTask Slide()
    {
        float targetX = _cameraTransform.position.x + SlideAmount;
        await _cameraTransform.DOMoveX(this.transform.position.x + _slideAmount, _slideDuration).SetEase(_animationEase);
    }
    public void Shake()
    {

        _cameraTransform.DOComplete();
        _cameraTransform.DOShakePosition(_shakeDuration, _shakeStrength, _shakeVibrato);
    }
}
