using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterImageData", menuName = "Scriptable Objects/CharacterImageData")]
public class CharacterImageData : ScriptableObject
{
    // シリアライズフィールドでスプライトを格納
    [SerializeField] private Sprite[] _characterSprites;

    // リードオンリーでスプライトを渡す
    public IReadOnlyList<Sprite> CharacterSprites => _characterSprites;
}
