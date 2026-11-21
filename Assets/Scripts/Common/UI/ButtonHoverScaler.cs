using CoreKit.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    [RequireComponent(typeof(Button), typeof(HoverChecker))]
    public class ButtonHoverScaler : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private HoverChecker _hoverChecker;
        [SerializeField] private float _hoverScale = 1.2f;
        [SerializeField] private float _duration = 0.3f;

        private float _defaultScale;

        private void Start()
        {
            _defaultScale = _button.transform.localScale.x;

            _hoverChecker.OnEnter += () => OnMouseEvent(_defaultScale * _hoverScale);
            _hoverChecker.OnExit += () => OnMouseEvent(_defaultScale);
        }

        private void OnMouseEvent(float size)
        {
            _button.transform.DOKill();
            _button.transform.DOScale(size, _duration).SetEase(Ease.OutBack);
        }

        private void Reset()
        {
            _button = GetComponent<Button>();
            _hoverChecker = GetComponent<HoverChecker>();
        }
    }
}