using System;
using System.Collections.Generic;
using UnityEngine;

// Prefabと生成数のセット
[Serializable]
public struct SpawnPrefab
{
    public GameObject prefab;
    [Min(1)] public int count;
}

// 入力（Enum）と、生成するアイテムのグループの紐づけ
[Serializable]
public struct ActionPrefabMapping
{
    public Ingredients ingredient;
    public List<SpawnPrefab> spawnPrefabs; // 複数のPrefabと個数を登録可能にする
}


