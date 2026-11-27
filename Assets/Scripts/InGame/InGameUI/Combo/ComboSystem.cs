using System;
using UnityEngine;

/// <summary>
/// コンボのロジック。AddCombo()で加算、Miss()でリセット。
/// </summary>
public class ComboSystem : MonoBehaviour
{
    [SerializeField] private ComboData data;

    public ComboData Data => data;
    public int Combo { get; private set; }

    /// <summary>コンボ数が変化したとき（現在のコンボ数）</summary>
    public event Action<int> OnComboChanged;

    /// <summary>秒数が増えたとき（増えた秒数）</summary>
    public event Action<int> OnTimeAdded;

    /// <summary>ミスしたとき</summary>
    public event Action OnMissed;

    /// <summary>最大コンボに到達したとき（直後にコンボは0に戻る）</summary>
    public event Action OnMaxComboReached;

    public void AddCombo()
    {
        Combo++;
        OnComboChanged?.Invoke(Combo);

        int bonus = data.GetBonusSeconds(Combo);
        if (bonus > 0)
        {
            OnTimeAdded?.Invoke(bonus);
        }

        // 最大コンボに達したらリセット
        if (Combo >= data.MaxCombo)
        {
            OnMaxComboReached?.Invoke();
            Combo = 0;
            OnComboChanged?.Invoke(Combo);
        }
    }

    public void Miss()
    {
        Combo = 0;
        OnComboChanged?.Invoke(Combo);
        OnMissed?.Invoke();
    }
}