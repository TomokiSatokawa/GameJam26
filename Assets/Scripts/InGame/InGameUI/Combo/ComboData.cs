using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コンボの設定データ。「何コンボで何秒増えるか」をここで管理する。
/// </summary>
[CreateAssetMenu(fileName = "ComboData", menuName = "Game/Combo Data")]
public class ComboData : ScriptableObject
{
    [Serializable]
    public class Milestone
    {
        [Min(1)] public int combo = 10;        // 何コンボ目で
        [Min(1)] public int bonusSeconds = 2;  // 何秒増えるか
    }

    [SerializeField, Min(1)] private int maxCombo = 50; // スライダーが満タンになるコンボ数
    [SerializeField]
    private List<Milestone> milestones = new List<Milestone>
    {
        new Milestone { combo = 10, bonusSeconds = 2 },
        new Milestone { combo = 25, bonusSeconds = 3 },
        new Milestone { combo = 50, bonusSeconds = 5 },
    };

    public int MaxCombo => maxCombo;
    public IReadOnlyList<Milestone> Milestones => milestones;

    /// <summary>そのコンボ数ちょうどで増える秒数（なければ0）</summary>
    public int GetBonusSeconds(int combo)
    {
        foreach (var m in milestones)
        {
            if (m.combo == combo) return m.bonusSeconds;
        }
        return 0;
    }
}