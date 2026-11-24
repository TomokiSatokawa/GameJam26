using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    public class FadeImageControl : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private bool _isStartFadeIn = true;

        private void Start()
        {
            _image.raycastTarget = false;
            SetAlpha(0f);

            if (_isStartFadeIn)
            {
                FadeIn().Forget();
            }
        }

        /// <summary>
        /// alpha 1 -> 0
        /// </summary>
        public async UniTask FadeIn()
        {
            _image.raycastTarget = true;
            _image.transform.SetAsLastSibling();
            _image.DOKill(true);
            SetAlpha(1f);
            await _image.DOFade(0, _duration);
            _image.raycastTarget = false;
        }

        /// <summary>
        /// alpha 0 -> 1
        /// </summary>
        public async UniTask FadeOut()
        {
            _image.raycastTarget = true;
            _image.transform.SetAsLastSibling();
            _image.DOKill(true);
            SetAlpha(0f);
            await _image.DOFade(1, _duration);
        }

        private void SetAlpha(float value)
        {
            Color color = _image.color;
            color.a = value;
            _image.color = color;

        }

        private void Reset()
        {
            _image = GetComponent<Image>();

            if (_image == null)
            {
                _image = gameObject.AddComponent<Image>();
            }

            _image.color = Color.black;
            SetAlpha(0f);
        }
    }
}
