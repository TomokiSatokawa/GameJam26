using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [Header("生成プレハブ設定")]
    [SerializeField] private StageUnit _stagePrefab;
    [SerializeField] private Vector3 _centerStartPosition;
    [SerializeField] private int _maxKeepCount = 3;
    [SerializeField] private float _interval;

    private Queue<StageUnit> _poolQueue = new Queue<StageUnit>();
    private int _spawnIndex = 0;

    public Vector3 CurrentPosition => GetPositionByIndex(CurrentIndex);
    public Vector3 NextPosition => GetPositionByIndex(CurrentIndex + 1);

    private int CurrentIndex => _spawnIndex - 1 - Mathf.Max(1, _maxKeepCount) / 2;

    /// <summary>★追加：今プレイヤーがいる中央のStageUnitを取得する</summary>
    public StageUnit CurrentStage
    {
        get
        {
            if (_poolQueue.Count == 0) return null;
            int targetIdx = Mathf.Clamp(_poolQueue.Count - 1 - Mathf.Max(1, _maxKeepCount) / 2, 0, _poolQueue.Count - 1);
            int i = 0;
            foreach (var stage in _poolQueue)
            {
                if (i == targetIdx) return stage;
                i++;
            }
            return null;
        }
    }

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
            var obj = Instantiate(_stagePrefab, GetPositionByIndex(_spawnIndex), _stagePrefab.transform.rotation);
            _poolQueue.Enqueue(obj);
            obj.Initialize();
            _spawnIndex++;
        }
    }

    public void SpawnNext()
    {
        if (_poolQueue.Count == 0) return;

        var reuseObj = _poolQueue.Dequeue();
        reuseObj.transform.position = GetPositionByIndex(_spawnIndex);

        _poolQueue.Enqueue(reuseObj);
        reuseObj.Initialize();
        _spawnIndex++;
    }
}
