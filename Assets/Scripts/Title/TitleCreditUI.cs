//AI
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleCreditUI : MonoBehaviour
{
    [Header("参照設定")]
    [SerializeField] private GameObject _creditPanel;   // クレジット画面全体のパネル
    [SerializeField] private Image _creditImage;        // クレジットを表示するImage
    [SerializeField] private Sprite[] _creditSprites;   // 1枚目・2枚目の画像を登録

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
        // Escキーで閉じられるようにする
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

    // クリック時のページ送り処理（最後の次は1枚目に戻ってループ）
    public void NextPageByClick()
    {
        if (_creditSprites == null || _creditSprites.Length == 0) return;

        _currentIndex = (_currentIndex + 1) % _creditSprites.Length;

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