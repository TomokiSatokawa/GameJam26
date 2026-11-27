using UnityEngine;

/// <summary>
/// 指定した確率で対象オブジェクトをアクティブにする。
/// 同じ結果(アクティブ/非アクティブ)がn回連続で出ることは禁止する。
/// </summary>
public class RandomActive : InitializableBase
{
    [SerializeField] private GameObject _target;
    [SerializeField, Range(0f, 1f)] private float _activeProbability = 0.5f;
    [SerializeField, Min(2)] private int _banConsecutiveCount = 3;

    private bool _hasLastResult;
    private bool _lastResult;
    private int _consecutiveCount;

    public override void Initialize()
    {
        Activate();
    }

    /// <summary>
    /// 確率に従って抽選し、対象のアクティブ状態を切り替える。
    /// </summary>
    public void Activate()
    {
        if (_target == null)
        {
            Debug.LogError($"[RandomActive] ターゲットが未設定です。 name={name}");
            return;
        }

        bool result = Random.value < _activeProbability;

        // 同じ結果がn回連続になる場合は結果を反転する
        bool isBanned = _hasLastResult
                        && result == _lastResult
                        && _consecutiveCount >= _banConsecutiveCount - 1;
        if (isBanned)
        {
            result = !result;
        }

        _consecutiveCount = _hasLastResult && result == _lastResult ? _consecutiveCount + 1 : 1;
        _lastResult = result;
        _hasLastResult = true;

        _target.SetActive(result);
    }
}