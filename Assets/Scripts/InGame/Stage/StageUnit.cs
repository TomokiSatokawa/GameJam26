using System.Collections.Generic;
using UnityEngine;

public class StageUnit : InitializableBase
{
    [SerializeField] private InitializableBase[] _initializables;
    [SerializeField] private CharacterGenerate _characterGenerate;

    // このステージ上に生成された具材を記録しておくリスト
    private readonly List<GameObject> _spawnedIngredients = new List<GameObject>();

    /// <summary>生成した具材をこのステージに紐づける</summary>
    public void RegisterIngredient(GameObject ingredient)
    {
        if (ingredient == null) return;
        ingredient.transform.SetParent(this.transform); // ステージの子オブジェクトにする
        _spawnedIngredients.Add(ingredient);
    }

    public override void Initialize()
    {
        // 1. ステージ再利用時に、前回このステージに盛られた具材をすべて削除する
        foreach (var item in _spawnedIngredients)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        _spawnedIngredients.Clear();

        // 2. 既存の初期化処理（キャラやポスターの再抽選）
        foreach (var initializable in _initializables)
        {
            initializable.Initialize();
        }
    }

    public void Fall()
    {
        _characterGenerate.ChangeExpression();
    }
}
