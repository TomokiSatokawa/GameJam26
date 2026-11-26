//AI
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Physics
{
    /// <summary>
    /// 有効化から一定時間が経過するか、速度判定の猶予後に速度が閾値以下になったらRigidbodyの物理挙動を止める
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyAutoStopper : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _delaySeconds = 3f;
        [SerializeField] private float _speedCheckStartSeconds = 0.5f;
        [SerializeField] private float _speedThreshold = 0.1f;

        private CancellationTokenSource _cts;

        private void Reset()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _cts = new CancellationTokenSource();
            WaitAndStopAsync(_cts.Token).Forget();
        }

        private void OnDisable()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private async UniTaskVoid WaitAndStopAsync(CancellationToken token)
        {
            float elapsed = 0f;

            while (elapsed < _delaySeconds)
            {
                bool isCanceled = await UniTask.Yield(PlayerLoopTiming.FixedUpdate, token)
                    .SuppressCancellationThrow();

                if (isCanceled)
                {
                    return;
                }

                elapsed += Time.fixedDeltaTime;

                if (elapsed >= _speedCheckStartSeconds && IsBelowSpeedThreshold())
                {
                    break;
                }
            }

            StopRigidbody();
        }

        private bool IsBelowSpeedThreshold()
        {
            return _rigidbody.linearVelocity.sqrMagnitude < _speedThreshold * _speedThreshold;
        }

        private void StopRigidbody()
        {
            if (_rigidbody == null)
            {
                Debug.LogError($"[RigidbodyAutoStopper] Rigidbodyが未設定です: {name}");
                return;
            }

            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.isKinematic = true;
        }
    }
}