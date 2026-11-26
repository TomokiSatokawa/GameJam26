using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 具材を生成するクラス </summary>
public class IngredientsGenerator : MonoBehaviour
{
    [Header("入力とPrefabのマッピング設定")]
    [SerializeField] private List<ActionPrefabMapping> actionMappings;

    [Header("生成位置")]
    [SerializeField] private Transform spawnPoint;

    // 具材とprefabの辞書
    private Dictionary<Ingredients, GameObject> prefabDictionary;

    #region InputServiceイベント
    private IDisposable _yasai;
    private IDisposable _ninniku;
    private IDisposable _abura;
    #endregion

    private void Awake()
    {
        // リストの内容をDictionaryに変換してキャッシュする
        prefabDictionary = new Dictionary<Ingredients, GameObject>();

        foreach (ActionPrefabMapping mapping in actionMappings)
        {
            if (!prefabDictionary.ContainsKey(mapping.ingradient))
            {
                prefabDictionary.Add(mapping.ingradient, mapping.ingradientPrefab);
            }
            else
            {
                Debug.LogWarning($"重複したIngredientsが設定されています: {mapping.ingradient}");
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

    public void InputYasai()
    {
        SpawnPrefabByInput(Ingredients.Yasai);
    }

    public void InputNinniku()
    {
        SpawnPrefabByInput(Ingredients.Ninniku);
    }

    public void InputAbura()
    {
        SpawnPrefabByInput(Ingredients.Abura);
    }

    //指定したenumに対応するPrefabを生成する処理
    public void SpawnPrefabByInput(Ingredients ingredients)
    {
        if (prefabDictionary.TryGetValue(ingredients, out GameObject targetPrefab))
        {
            if (targetPrefab != null)
            {
                Instantiate(targetPrefab, spawnPoint.position, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogWarning($"{ingredients} に対応するPrefabが登録されていません。");
        }
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
