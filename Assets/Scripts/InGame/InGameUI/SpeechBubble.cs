using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using R3;
using Common.Sound;
using Generated.DynamicEnums;

public class SpeechBubble : MonoBehaviour
{
    [Header("参照")]
    [Tooltip("注文を表示する吹き出し")]
    [SerializeField] private RectTransform _speechBubble;
    [Tooltip("注文の文字列")]
    [SerializeField] private TextMeshProUGUI _orderText;
    [Tooltip("吹き出しと注文をまとめる")]
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private ToppingCounter _toppingCounter;

    [Header("吹き出しを動かす設定")]
    [Tooltip("初期位置")]
    [SerializeField] private Vector2 _startOffset = new Vector2(0f, 200f);
    [Tooltip("フェード・移動速度")]
    [SerializeField] private float _duration = 0.5f;

    [Header("アイコン設定")]
    [Tooltip("Horizontal Layout Groupが付いている親オブジェクト")]
    [SerializeField] private Transform _iconContainer;
    [Tooltip("生成するアイコンのImageプレハブ")]
    [SerializeField] private Image _iconPrefab;
    [Tooltip("野菜のアイコン画像")]
    [SerializeField] private Sprite _yasaiSprite;
    [Tooltip("ニンニクのアイコン画像")]
    [SerializeField] private Sprite _ninnikuSprite;
    [Tooltip("アブラのアイコン画像")]
    [SerializeField] private Sprite _aburaSprite;

    [Header("アイコンの有効/無効カラー")]
    [Tooltip("有効（入れ済み）のアイコンの色")]
    [SerializeField] private Color _validColor = Color.white;
    [Tooltip("無効（まだ足りない）アイコンの色")]
    [SerializeField] private Color _invalidColor = new Color(1f, 1f, 1f, 0.3f);

    [Header("はみ出し対策")]
    [Tooltip("アイコンのスケールxを縮める下限")]
    [SerializeField, Range(0.1f, 1f)] private float _minIconScaleX = 0.3f;

    private HorizontalLayoutGroup _layoutGroup;
    private float _defaultSpacing;

    private readonly List<Image> _yasaiImages = new();
    private readonly List<Image> _ninnikuImages = new();
    private readonly List<Image> _aburaImages = new();

    private Vector2 _defaultPos;
    private Sequence _sequence;

    private void Awake()
    {
        _defaultPos = _speechBubble.anchoredPosition;
        _canvasGroup.alpha = 0f;

        _toppingCounter.YasaiCount.Subscribe(x => UpdateIconValid(x, _yasaiImages)).RegisterTo(destroyCancellationToken);
        _toppingCounter.NinnikuCount.Subscribe(x => UpdateIconValid(x, _ninnikuImages)).RegisterTo(destroyCancellationToken);
        _toppingCounter.AburaCount.Subscribe(x => UpdateIconValid(x, _aburaImages)).RegisterTo(destroyCancellationToken);

        _layoutGroup = _iconContainer.GetComponent<HorizontalLayoutGroup>();
        _defaultSpacing = _layoutGroup.spacing;
    }

    public void ShowSpeechBubble(string input, OrderData data)
    {
        _sequence?.Kill();

        _orderText.text = input;

        _canvasGroup.alpha = 0f;
        _speechBubble.anchoredPosition = _defaultPos + _startOffset;

        UpdateIcons(data);
        SoundManager.SE.PlayOneShot(SEType.Call);

        _sequence = DOTween.Sequence()
           .Join(_canvasGroup.DOFade(1f, _duration).SetEase(Ease.OutQuad))
           .Join(_speechBubble.DOAnchorPos(_defaultPos, _duration).SetEase(Ease.OutBack));
    }

    public void HideSpeechBubble()
    {
        _sequence?.Kill();

        _sequence = DOTween.Sequence()
           .Join(_canvasGroup.DOFade(0f, _duration).SetEase(Ease.InQuad))
           .Join(_speechBubble.DOAnchorPos(_defaultPos + _startOffset, _duration).SetEase(Ease.InBack));
    }
    
    /// <summary>
     /// アニメーションなしで吹き出しを即座に非表示にする
     /// </summary>
    public void HideSpeechBubbleImmediate()
    {
        _sequence?.Kill();

        _canvasGroup.alpha = 0f;
        _speechBubble.anchoredPosition = _defaultPos + _startOffset;
    }

    private void UpdateIcons(OrderData data)
    {
        foreach (Transform child in _iconContainer)
        {
            Destroy(child.gameObject);
        }

        SpawnIcons(_yasaiSprite, data.YasaiCount, _yasaiImages);
        SpawnIcons(_ninnikuSprite, data.NinnikuCount, _ninnikuImages);
        SpawnIcons(_aburaSprite, data.AburaCount, _aburaImages);

        FitIconsToContainer();

        UpdateIconValid(_toppingCounter.YasaiCount.CurrentValue, _yasaiImages);
        UpdateIconValid(_toppingCounter.NinnikuCount.CurrentValue, _ninnikuImages);
        UpdateIconValid(_toppingCounter.AburaCount.CurrentValue, _aburaImages);
    }

    private void SpawnIcons(Sprite sprite, int count, List<Image> clonedList)
    {
        clonedList.Clear();
        for (int i = 0; i < count; i++)
        {
            Image icon = Instantiate(_iconPrefab, _iconContainer);
            icon.sprite = sprite;
            icon.color = _invalidColor; // 最初は無効状態
            clonedList.Add(icon);
        }
    }

    /// <summary>
    /// 先頭からcount個を有効色、それ以降を無効色にする
    /// </summary>
    private void UpdateIconValid(int count, List<Image> clonedList)
    {
        for (int i = 0; i < clonedList.Count; i++)
        {
            clonedList[i].color = i < count ? _validColor : _invalidColor;
        }
    }
    
    /// <summary>
     /// アイコンが_iconContainerの幅を超える場合、スケールxとspacingを縮めて収める
     /// </summary>
    private void FitIconsToContainer()
    {
        int count = _yasaiImages.Count + _ninnikuImages.Count + _aburaImages.Count;
        if (count == 0) return;

        RectTransform containerRect = (RectTransform)_iconContainer;
        float iconWidth = _iconPrefab.rectTransform.rect.width;
        float availableWidth = containerRect.rect.width - _layoutGroup.padding.horizontal;
        float requiredWidth = iconWidth * count + _defaultSpacing * (count - 1);

        float scaleX = 1f;
        if (requiredWidth > availableWidth)
        {
            scaleX = Mathf.Max(availableWidth / requiredWidth, _minIconScaleX);
        }

        // 見た目の1個分の間隔が (iconWidth + spacing) * scaleX になるようにspacingを詰める
        _layoutGroup.spacing = (iconWidth + _defaultSpacing) * scaleX - iconWidth;

        Vector3 scale = new Vector3(scaleX, 1f, 1f);
        ApplyScale(_yasaiImages, scale);
        ApplyScale(_ninnikuImages, scale);
        ApplyScale(_aburaImages, scale);
    }

    private void ApplyScale(List<Image> images, Vector3 scale)
    {
        foreach (Image image in images)
        {
            image.rectTransform.localScale = scale;
        }
    }
}