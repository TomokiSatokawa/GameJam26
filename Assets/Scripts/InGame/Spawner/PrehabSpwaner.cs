using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private CameraWork _cameraWork;
    [Header("生成プレハブ設定")]
    [SerializeField] private GameObject _stagePrefab;
    [SerializeField] private Vector3 _centerStartPosition;
    [SerializeField] private int _maxKeepCount = 3;

    private Queue<GameObject> _poolQueue = new Queue<GameObject>();
    private int _spawnIndex = 0;

    private void Start()
    {
        if (_stagePrefab == null) return;

        float interval = _cameraWork.SlideAmount;


        for (int i = 0; i < _maxKeepCount; i++)
        {
            float moveX = (_spawnIndex - 1) * interval;
            Vector3 spawnPos = _centerStartPosition + new Vector3(moveX, 0f, 0f);

            GameObject obj = Instantiate(_stagePrefab, spawnPos, _stagePrefab.transform.rotation);
            _poolQueue.Enqueue(obj);
            _spawnIndex++;
        }
    }

    public void SpawnNext()
    {
        if (_poolQueue.Count == 0) return;

        float interval = _cameraWork.SlideAmount;
        float moveX = (_spawnIndex - 1) * interval;
        Vector3 nextPos = _centerStartPosition + new Vector3(moveX, 0f, 0f);

 
        GameObject reuseObj = _poolQueue.Dequeue();


        reuseObj.transform.position = nextPos;


        _poolQueue.Enqueue(reuseObj);
        _spawnIndex++;
    }
}

