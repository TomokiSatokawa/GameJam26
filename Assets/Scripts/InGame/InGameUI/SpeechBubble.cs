using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    [Header("テスト用設定")]
    [Tooltip("テストで表示する文字列（テストする場合はコード最下部のUpdateのコメントアウトを外してください。）")]
    [SerializeField] private string _testOrderText = "ヤサイマシマシアブラマシマシニンニクマシマシ";
    [SerializeField] private int _testYasai = 3;
    [SerializeField] private int _testAbura = 3;
    [SerializeField] private int _testNinniku = 3;

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
    public void ShowSpeechBubble(string input, int yasaiCount, int ninnikuCount, int aburaCount)
    {
        // もしアニメーション再生中なら一旦止める
        _sequence?.Kill();

        // 吹き出しの中に文章を表示する
        _orderText.text = input;

        _canvasGroup.alpha = 0f;
        _speechBubble.anchoredPosition = _defaultPos + _startOffset;

        // アイコンを再生成して整列させる
        UpdateIcons(yasaiCount, ninnikuCount, aburaCount);

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

    private void UpdateIcons(int yasaiCount, int aburaCount, int ninnikuCount)
    {
        // 前回の注文のアイコンが残っていればすべて削除する
        foreach (Transform child in _iconContainer)
        {
            Destroy(child.gameObject);
        }

        // ヤサイ・ニンニク・アブラの順に、指定された個数分だけ生成する
        SpawnIcons(_yasaiSprite, yasaiCount);
        SpawnIcons(_ninnikuSprite, ninnikuCount);
        SpawnIcons(_aburaSprite, aburaCount);
    }

    private void SpawnIcons(Sprite sprite, int count)
    {
        for (int i = 0; i < count; i++)
        {
            // _iconContainerの子として生成すると、Horizontal Layout Groupが自動で横並びに整列する
            Image icon = Instantiate(_iconPrefab, _iconContainer);
            icon.sprite = sprite;
        }
    }

    // テスト用アップデート
    
    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ShowSpeechBubble(_testOrderText, _testYasai, _testNinniku, _testAbura);
        }
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            HideSpeechBubble();
        }
    }
    

}
