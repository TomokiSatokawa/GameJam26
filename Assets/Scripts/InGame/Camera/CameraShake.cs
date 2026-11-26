using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private ToppingCounter _toppingCounter;

    [Header("シェイク設定")]
    [SerializeField] private float _duration = 0.25f;   
    [SerializeField] private float _magnitude = 0.15f;

    private Vector3 _initialLocalPosition;
    private Coroutine _shakeCoroutine;

    private void Awake()
    {
        _initialLocalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        if (_toppingCounter != null)
        {
            _toppingCounter.penalty += StartShake;
        }
    }

    private void OnDisable()
    {
        if (_toppingCounter != null)
        {
            _toppingCounter.penalty -= StartShake;
        }
    }

    private void StartShake()
    {
        if (_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);
        }
        _shakeCoroutine = StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * _magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * _magnitude;

            transform.localPosition = _initialLocalPosition + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.localPosition = _initialLocalPosition;
        _shakeCoroutine = null;
    }
}
