using Common.Order;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraWork _camera;
    [SerializeField] private PrefabSpawner _spawner;

    private void Update()
    {
        if (InputService.Player.Interact.Down)
        {
           
            if (_spawner != null)
            {
                _spawner.SpawnNext();
            }

            _camera.Slider().Forget();
        }
    }
}
