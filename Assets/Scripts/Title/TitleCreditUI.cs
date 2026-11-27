using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleCreditUI : MonoBehaviour, IPointerClickHandler
{
    [Header("参照設定")]
    [SerializeField] private GameObject _creditPanel;   // クレジット画面全体のパネル
    [SerializeField] private Image _creditImage;        // クレジットを表示するImage
    [SerializeField] private Sprite[] _creditSprites;   // 1枚目・2枚目の画像を登録

    [Header("最後のページをクリックした時の動作")]
    [SerializeField] private bool _closeOnLastClick = true; // true: 2枚目の次で閉じる / false: 1枚目に戻る

    private int _currentIndex = 0;

    private void Start()
    {
        if (_creditPanel != null)
        {
            _creditPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // Escキーでも閉じられるようにする
        if (_creditPanel != null && _creditPanel.activeSelf)
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                CloseCredit();
            }
        }
    }

    // タイトルの「クレジット」ボタンに設定
    public void OpenCredit()
    {
        _currentIndex = 0;
        UpdateImage();
        if (_creditPanel != null)
        {
            _creditPanel.SetActive(true);
        }
    }

    // 閉じる処理
    public void CloseCredit()
    {
        if (_creditPanel != null)
        {
            _creditPanel.SetActive(false);
        }
    }

    // パネル（または画像）がクリックされた瞬間に自動で呼ばれる
    public void OnPointerClick(PointerEventData eventData)
    {
        NextPageByClick();
    }

    // クリック時のページ送り処理
    public void NextPageByClick()
    {
        if (_creditSprites == null || _creditSprites.Length == 0) return;

        _currentIndex++;

        if (_currentIndex >= _creditSprites.Length)
        {
            if (_closeOnLastClick)
            {
                // 2枚目を見終わった後のクリックでパネルを閉じる
                CloseCredit();
                return;
            }
            else
            {
                // 1枚目に戻る（ループ）
                _currentIndex = 0;
            }
        }

        UpdateImage();
    }

    private void UpdateImage()
    {
        if (_creditImage != null && _creditSprites != null && _creditSprites.Length > 0)
        {
            _creditImage.sprite = _creditSprites[_currentIndex];
        }
    }
}
