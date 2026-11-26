using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [Header("生成プレハブ設定")]
    [SerializeField] private GameObject _stagePrefab;
    [SerializeField] private Vector3 _centerStartPosition;
    [SerializeField] private int _maxKeepCount = 3;
    [SerializeField] private float _interval;

    private Queue<GameObject> _poolQueue = new Queue<GameObject>();
    private int _spawnIndex = 0;

    /// <summary>今プレイヤーがいるステージ（保持している中央）の位置</summary>
    public Vector3 CurrentPosition => GetPositionByIndex(CurrentIndex);

    /// <summary>現在の1つ先のステージの位置</summary>
    public Vector3 NextPosition => GetPositionByIndex(CurrentIndex + 1);

    // 最後に生成したインデックスから、中央のインデックスを逆算
    private int CurrentIndex => _spawnIndex - 1 - Mathf.Max(1, _maxKeepCount) / 2;

    private Vector3 GetPositionByIndex(int index)
    {
        float moveX = (index - 1) * _interval;
        return _centerStartPosition + new Vector3(moveX, 0f, 0f);
    }

    private void Start()
    {
        if (_stagePrefab == null) return;

        for (int i = 0; i < _maxKeepCount; i++)
        {
            GameObject obj = Instantiate(_stagePrefab, GetPositionByIndex(_spawnIndex), _stagePrefab.transform.rotation);
            _poolQueue.Enqueue(obj);
            _spawnIndex++;
        }
    }

    public void SpawnNext()
    {
        if (_poolQueue.Count == 0) return;

        GameObject reuseObj = _poolQueue.Dequeue();
        reuseObj.transform.position = GetPositionByIndex(_spawnIndex);

        _poolQueue.Enqueue(reuseObj);
        _spawnIndex++;
    }
}