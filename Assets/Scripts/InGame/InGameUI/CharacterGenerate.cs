using UnityEngine;
using UnityEngine.UI;

public class CharacterGenerate : MonoBehaviour
{
    [Header("参照")]
    [Tooltip("CharacterImageDataを入れる")]
    [SerializeField] private CharacterImageData _characterSprite;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        // スプライトのリストからランダムに数を取得
        int max = _characterSprite.CharacterSprites.Count;
        int count = Random.Range(0, max);

        // スプライトをランダムに変更
        _spriteRenderer.sprite = _characterSprite.CharacterSprites[count];

        /*
        Image character = gameObject.GetComponent<Image>();
        character.sprite = _characterSprite.CharacterSprites[count];
        */
    }

    /// <summary>
    /// デバッグ用にボタンから呼び出す処理
    /// </summary>
    /*
    public void Test()
    {
        // スプライトのリストからランダムに数を取得
        int max = _characterSprite.CharacterSprites.Count;
        int count = Random.Range(0, max);

        // スプライトをランダムに変更
        _spriteRenderer.sprite = _characterSprite.CharacterSprites[count];

    }
    */
    
}
