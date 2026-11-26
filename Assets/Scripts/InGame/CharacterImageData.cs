using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterImageData", menuName = "Scriptable Objects/CharacterImageData")]
public class CharacterImageData : ScriptableObject
{
    // シリアライズフィールドでスプライトを格納
    [SerializeField] private DifferentExpressions[] _characterSprites;

    // リードオンリーでスプライトを渡す
    public IReadOnlyList<DifferentExpressions> CharacterSprites => _characterSprites;
}

// Unityのインスペクターにこのクラスの中身を表示させる
[System.Serializable]
public class DifferentExpressions
{
    public Sprite normal;
    public Sprite miss;
}