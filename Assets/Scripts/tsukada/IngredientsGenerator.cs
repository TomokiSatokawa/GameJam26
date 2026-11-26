using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 具材を生成するクラス </summary>
public class IngredientsGenerator : MonoBehaviour
{
    [Header("入力とPrefabグループのマッピング設定")]
    [SerializeField] private List<ActionPrefabMapping> actionMappings;

    [Header("生成位置")]
    [SerializeField] private Transform spawnPoint;

    // 具材と「生成アイテムリスト」の辞書に変更
    private Dictionary<Ingredients, List<SpawnPrefab>> prefabGroupDictionary;

    #region InputServiceイベント
    private IDisposable _yasai;
    private IDisposable _ninniku;
    private IDisposable _abura;
    #endregion

    private void Awake()
    {
        // リストの内容をDictionaryに変換してキャッシュする
        prefabGroupDictionary = new Dictionary<Ingredients, List<SpawnPrefab>>();

        foreach (ActionPrefabMapping mapping in actionMappings)
        {
            if (!prefabGroupDictionary.ContainsKey(mapping.ingredient))
            {
                prefabGroupDictionary.Add(mapping.ingredient, mapping.spawnPrefabs);
            }
            else
            {
                Debug.LogWarning($"重複したIngredientsが設定されています: {mapping.ingredient}");
            }
        }
    }

    private void OnEnable()
    {
        _yasai = InputService.Player.Yasai.OnPressed(InputYasai);
        _ninniku = InputService.Player.Ninniku.OnPressed(InputNinniku);
        _abura = InputService.Player.Abura.OnPressed(InputAbura);
    }

    private void OnDisable()
    {
        _yasai?.Dispose();
        _ninniku?.Dispose();
        _abura?.Dispose();
    }

    public void InputYasai() => SpawnPrefabGroupByInput(Ingredients.Yasai);
    public void InputNinniku() => SpawnPrefabGroupByInput(Ingredients.Ninniku);
    public void InputAbura() => SpawnPrefabGroupByInput(Ingredients.Abura);

    // 指定したenumに対応するPrefabグループをすべて生成する処理
    public void SpawnPrefabGroupByInput(Ingredients ingredients)
    {
        if (prefabGroupDictionary.TryGetValue(ingredients, out List<SpawnPrefab> items))
        {
            // 登録されているアイテムの種類ごとにループ
            foreach (SpawnPrefab item in items)
            {
                if (item.prefab == null) continue;

                // 指定された個数(count)分だけ生成する
                for (int i = 0; i < item.count; i++)
                {
                    Instantiate(item.prefab, spawnPoint.position, UnityEngine.Random.rotation);
                }
            }
        }
        else
        {
            Debug.LogWarning($"{ingredients} に対応するPrefabグループが登録されていません。");
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
