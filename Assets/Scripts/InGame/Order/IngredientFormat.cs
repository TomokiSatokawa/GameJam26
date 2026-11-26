using System;
using UnityEngine;

namespace Common.Order
{
    /// <summary>
    /// 個数に応じた表記の切り替え条件(○個以上ならこの接尾辞)を表す
    /// </summary>
    [Serializable]
    public class CountSuffix
    {
        [SerializeField] private int _minCount;
        [SerializeField] private string _suffix;

        public int MinCount => _minCount;
        public string Suffix => _suffix;
    }

    /// <summary>
    /// 1つの具材の名前と、個数ごとの表記を管理する
    /// </summary>
    [Serializable]
    public class IngredientFormat
    {
        [SerializeField] private string _name;
        [SerializeField] private IngredientFormatData _suffixes;

        public string Name => _name;

        /// <summary>
        /// 個数に応じた表記を返す。0以下の場合は空文字を返す
        /// </summary>
        public string GetText(int count)
        {
            if (count <= 0)
            {
                return string.Empty;
            }

            var bestMinCount = 0;
            var bestSuffix = string.Empty;

            if (_suffixes != null)
            {
                // count以下で最も大きいMinCountの接尾辞を採用する
                foreach (var entry in _suffixes.Suffixes)
                {
                    if (entry.MinCount <= count && entry.MinCount > bestMinCount)
                    {
                        bestMinCount = entry.MinCount;
                        bestSuffix = entry.Suffix;
                    }
                }
            }

            return _name + bestSuffix;
        }
    }
}