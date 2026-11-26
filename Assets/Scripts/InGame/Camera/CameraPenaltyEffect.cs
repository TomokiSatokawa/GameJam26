using UnityEngine;

public class CameraPenaltyEffect : MonoBehaviour
{
    [SerializeField] private ToppingCounter _toppingCounter;
    [SerializeField] private CameraWork _cameraWork;

    private void OnEnable()
    {
        
        if (_toppingCounter != null)
        {
            _toppingCounter.penalty += OnPenalty;
        }
    }

    private void OnDisable()
    {
 
        if (_toppingCounter != null)
        {
            _toppingCounter.penalty -= OnPenalty;
        }
    }

    private void OnPenalty()
    {
        if (_cameraWork != null)
        {
            _cameraWork.Shake();
        }
    }
}

