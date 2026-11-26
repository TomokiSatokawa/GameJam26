using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PosterImageData", menuName = "Scriptable Objects/PosterImageData")]
public class PosterImageData : ScriptableObject
{
    [SerializeField] private List<Sprite> _posterSprites;

    public List<Sprite> PosterSprites => _posterSprites;
}
