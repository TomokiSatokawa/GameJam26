using UnityEngine;

public class PosterGenerate : InitializableBase
{
    [Header("参照")]
    [Tooltip("PosterImageDataを入れる")]
    [SerializeField] private PosterImageData _posterData;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public override void Initialize()
    {
        if (_posterData == null || _posterData.PosterSprites == null || _posterData.PosterSprites.Count == 0)
        {
            Debug.LogWarning("PosterImageDataが設定されていないか、画像が空です。");
            return;
        }

        // スプライトのリストからランダムにインデックスを取得
        int max = _posterData.PosterSprites.Count;
        int randomIndex = Random.Range(0, max);

        // ポスターの画像をランダムに変更
        _spriteRenderer.sprite = _posterData.PosterSprites[randomIndex];
    }
}
