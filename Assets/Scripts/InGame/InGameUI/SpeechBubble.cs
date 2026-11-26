using DG.Tweening;
using TMPro;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    [Header("参照")]
    [Tooltip("注文を表示する吹き出し")]
    [SerializeField] private RectTransform _speechBubble;
    [Tooltip("注文の文字列")]
    [SerializeField] private TextMeshProUGUI _orderText;
    [Tooltip("吹き出しと注文をまとめる")]
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("吹き出しを動かす設定")]
    [Tooltip("初期位置")]
    [SerializeField] private Vector2 _startOffset = new Vector2(0f, 200f);
    [Tooltip("フェード・移動速度")]
    [SerializeField] private float _duration = 0.5f;

    // エディタ上で配置した「本来の表示位置」を覚えておく変数
    private Vector2 _defaultPos;
    // 再生中のアニメーションを管理する変数
    private Sequence _sequence;

    private void Awake()
    {
        // ゲーム開始時に、エディタ上で配置した吹き出しの位置を記憶する
        _defaultPos = _speechBubble.anchoredPosition;
        // 最初は見えない状態にしておく
        _canvasGroup.alpha = 0f;
    }


    /// <summary>
    /// ゲームマネージャーからの呼び出しで吹き出しと文章を表示する
    /// </summary>
    /// <param name="input"></param>
    public void ShowSpeechBubble(string input)
    {
        // もしアニメーション再生中なら一旦止める
        _sequence?.Kill();

        // 吹き出しの中に文章を表示する
        _orderText.text = input;

        _canvasGroup.alpha = 0f;
        _speechBubble.anchoredPosition = _defaultPos + _startOffset;

        // 吹き出しを表示する
        _sequence = DOTween.Sequence()
           .Join(_canvasGroup.DOFade(1f, _duration).SetEase(Ease.OutQuad))
           .Join(_speechBubble.DOAnchorPos(_defaultPos, _duration).SetEase(Ease.OutBack));
    }

    /// <summary>
    /// ゲームマネージャーからの呼び出しで吹き出しを画面外に出しつつ透明にする処理
    /// </summary>
    public void HideSpeechBubble()
    {
        // もしアニメーション再生中なら一旦止める
        _sequence?.Kill();

        // 吹き出しを透明にしつつ非表示に
        _sequence = DOTween.Sequence()
           .Join(_canvasGroup.DOFade(0f, _duration).SetEase(Ease.InQuad))
           .Join(_speechBubble.DOAnchorPos(_defaultPos + _startOffset, _duration).SetEase(Ease.InBack));
    }

    //// テスト用アップデート
    //private void Update()
    //{
    //    // Spaceキーを押した瞬間にテスト呼び出し
    //    if (Keyboard.current.spaceKey.wasPressedThisFrame)
    //    {
    //        ShowSpeechBubble(_testOrderText);
    //    }
    //    if(Keyboard.current.enterKey.wasPressedThisFrame)
    //    {
    //        HideSpeechBubble();
    //    }
    //}

}
