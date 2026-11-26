using System;
using System.Collections.Generic;
using Common.Sound;
using CoreKit;
using Generated.DynamicEnums;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary> 具材を生成するクラス </summary>
public class IngredientsGenerator : MonoBehaviour
{
    [Header("入力とPrefabグループのマッピング設定")]
    [SerializeField] private List<ActionPrefabMapping> actionMappings;

    [Header("生成位置")]
    [SerializeField] private Transform spawnPoint;

    [Header("参照")]
    [SerializeField] private float _groupRange;
    [SerializeField] private ToppingCounter _toppingCounter;
    [SerializeField] private CameraWork _cameraWork; // ★追加：カメラワークをセット

    public readonly FlagRegistry IgnoreInput = new();

    // 具材と「生成アイテムリスト」の辞書に変更
    private Dictionary<Ingredients, List<SpawnPrefab>> prefabGroupDictionary;

    private void Awake()
    {
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

    // ★追加：ペナルティイベントの登録
    private void OnEnable()
    {
        if (_toppingCounter != null)
        {
            _toppingCounter.penalty += OnPenalty;
        }
    }

    // ★追加：ペナルティイベントの解除
    private void OnDisable()
    {
        if (_toppingCounter != null)
        {
            _toppingCounter.penalty -= OnPenalty;
        }
    }

    // ★追加：ペナルティ発生時にカメラを揺らす（＝同時にIsInputLockedがtrueになる）
    private void OnPenalty()
    {
        if (_cameraWork != null)
        {
            _cameraWork.Shake();
        }
    }

    private void Update()
    {
        // ★追加：カメラがシェイク中（ペナルティ硬直中）はキー入力を無視する
        if (_cameraWork != null && _cameraWork.IsInputLocked)
        {
            return;
        }

        if (IgnoreInput.Flag)
            return;
            
        if (InputService.Player.Yasai.Down)
            InputYasai();

        if (InputService.Player.Ninniku.Down)
            InputNinniku();

        if (InputService.Player.Abura.Down)
            InputAbura();
    }

    public void InputYasai()
    {
        SpawnPrefabGroupByInput(Ingredients.Yasai);
        _toppingCounter.AddYasai();
    }

    public void InputNinniku()
    {
        SpawnPrefabGroupByInput(Ingredients.Ninniku);
        _toppingCounter.AddNinniku();
    }

    public void InputAbura()
    {
        SpawnPrefabGroupByInput(Ingredients.Abura);
        _toppingCounter.AddAbura();
    }

    public void SpawnPrefabGroupByInput(Ingredients ingredients)
    {
        if (prefabGroupDictionary.TryGetValue(ingredients, out List<SpawnPrefab> items))
        {
            foreach (SpawnPrefab item in items)
            {
                if (item.prefab == null) continue;

                for (int i = 0; i < item.count; i++)
                {
                    Vector3 clonePosition = spawnPoint.position + Random.insideUnitSphere * _groupRange;
                    Instantiate(item.prefab, clonePosition, Random.rotation);
                }
            }

            SoundManager.SE.PlayOneShot(SEType.Moru);
        }
        else
        {
            Debug.LogWarning($"{ingredients} に対応するPrefabグループが登録されていません。");
        }
    }
}