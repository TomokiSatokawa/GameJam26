//AI
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// キャラクターのスプライトをランダムに決定し、ミス時は一定時間だけ表情を変更する
/// </summary>
public class CharacterGenerate : InitializableBase
{
    [Header("参照")]
    [Tooltip("CharacterImageDataを入れる")]
    [SerializeField] private CharacterImageData _characterSprite;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("設定")]
    [Tooltip("ミス表情を表示する時間(秒)")]
    [SerializeField] private float _missDuration = 1.0f;

    /// <summary>このスクリプトでScriptableObjectから指定するインデックス番号（ランダム）</summary>
    private int _index;

    private CancellationTokenSource _missCts;

    private void OnDestroy()
    {
        CancelMiss();
    }

    public override void Initialize()
    {
        // スプライトのリストからランダムに数を取得
        int max = _characterSprite.CharacterSprites.Count;
        _index = UnityEngine.Random.Range(0, max);

        // スプライトをランダムに変更
        _spriteRenderer.sprite = _characterSprite.CharacterSprites[_index].normal;
    }

    /// <summary>
    /// キャラクタースプライトを失敗時に外部から変更するメソッド。
    /// 一定時間だけmissにして、その後normalに戻す
    /// </summary>
    public void ChangeExpression()
    {
        Debug.Log("miss");
        // 連続ミス時は前の待機をキャンセルして、時間を数え直す
        CancelMiss();
        _missCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

        ShowMissAsync(_missCts.Token).Forget();
    }

    private async UniTaskVoid ShowMissAsync(CancellationToken token)
    {
        _spriteRenderer.sprite = _characterSprite.CharacterSprites[_index].miss;

        // キャンセルされたら例外を投げずにfalseが返る
        bool canceled = await UniTask.Delay(TimeSpan.FromSeconds(_missDuration), cancellationToken: token)
            .SuppressCancellationThrow();
        if (canceled)
        {
            return;
        }

        _spriteRenderer.sprite = _characterSprite.CharacterSprites[_index].normal;
    }

    private void CancelMiss()
    {
        if (_missCts == null)
        {
            return;
        }

        _missCts.Cancel();
        _missCts.Dispose();
        _missCts = null;
    }
}