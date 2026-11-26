using UnityEngine;

public class StageUnit : InitializableBase
{
    [SerializeField] private InitializableBase[] _initializables;

    public override void Initialize()
    {
        foreach (var initializable in _initializables)
        {
            initializable.Initialize();
        }
    }
}
