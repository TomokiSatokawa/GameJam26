using System.Collections.Generic;
using Common.Order;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientFormatData", menuName = "Scriptable Objects/IngredientFormatData")]
public class IngredientFormatData : ScriptableObject
{
    [SerializeField] private CountSuffix[] _suffixes;

    public IReadOnlyList<CountSuffix> Suffixes => _suffixes;
}
