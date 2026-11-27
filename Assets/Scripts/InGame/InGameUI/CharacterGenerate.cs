using UnityEngine;

public class CharacterGenerate : InitializableBase
{
    [Header("参照")]
    [Tooltip("CharacterImageDataを入れる")]
    [SerializeField] private CharacterImageData _characterSprite;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    /// <summary>このスクリプトでScriptableObjectから指定するインデックス番号（ランダム）</summary>
    private int count;

    public override void Initialize()
    {
        // スプライトのリストからランダムに数を取得
        int max = _characterSprite.CharacterSprites.Count;
        count = Random.Range(0, max);

        // スプライトをランダムに変更
        _spriteRenderer.sprite = _characterSprite.CharacterSprites[count].normal;
    }

    /// <summary>
    /// キャラクタースプライトを失敗時に外部から変更するメソッド
    /// </summary>
    public void ChangeExpression()
    {
        // 表情を変更する
        _spriteRenderer.sprite = _characterSprite.CharacterSprites[count].miss;
    }
}
