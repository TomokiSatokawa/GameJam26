//AI
using System.Collections.Generic;
using UnityEngine;

namespace Common.Order
{
    /// <summary>
    /// 注文データをDatabaseの表記ルールに従って文字列にする
    /// </summary>
    public class OrderFormatter : MonoBehaviour
    {
        [SerializeField] private OrderFormatterDatabase _database;

        /// <summary>
        /// 注文データを文字列にする。例: "やさいマシ ニンニク あぶら特盛"
        /// </summary>
        public string Format(OrderData orderData)
        {
            if (_database == null || orderData == null)
            {
                Debug.LogError($"[OrderFormatter] Database または OrderData が null です。database:{_database}, orderData:{orderData}");
                return string.Empty;
            }

            var parts = new List<string>();
            AddPart(parts, _database.Yasai, orderData.YasaiCount);
            AddPart(parts, _database.Ninniku, orderData.NinnikuCount);
            AddPart(parts, _database.Abura, orderData.AburaCount);

            return string.Join(" ", parts);
        }

        private void AddPart(List<string> parts, IngredientFormat format, int count)
        {
            var text = format.GetText(count);

            // 0個の具材は無視する
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            parts.Add(text);
        }
    }
}