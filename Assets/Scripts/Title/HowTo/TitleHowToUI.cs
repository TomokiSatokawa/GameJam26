using UnityEngine;
using UnityEngine.InputSystem;

public class TitleHowToUI : MonoBehaviour
{
    [Header("参照設定")]
    [SerializeField] private GameObject _howToPanel; // 遊び方説明のパネル

    private void Start()
    {
        if (_howToPanel != null)
        {
            _howToPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // パネルが開いているときにEscキーが押されたら閉じる
        if (_howToPanel != null && _howToPanel.activeSelf)
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                CloseHowTo();
            }
        }
    }

    // タイトルの「遊び方」ボタンに設定
    public void OpenHowTo()
    {
        if (_howToPanel != null)
        {
            _howToPanel.SetActive(true);
        }
    }

    // 遊び方パネル内の「閉じる」ボタンに設定
    public void CloseHowTo()
    {
        if (_howToPanel != null)
        {
            _howToPanel.SetActive(false);
        }
    }
}
