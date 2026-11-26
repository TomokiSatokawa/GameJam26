using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
    [SerializeField] private float _slideAmount;
    [SerializeField] private float _slideDuration;
    [SerializeField] private Ease _animationEase;
    [SerializeField] private Transform _cameraTransform;

    public float SlideAmount => Mathf.Abs(_slideAmount);
    public async UniTask Slider()
    {
        float targetX = _cameraTransform.position.x + SlideAmount;
        await _cameraTransform.DOMoveX(this.transform.position.x + _slideAmount, _slideDuration).SetEase(_animationEase);
    }
}
