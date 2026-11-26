using UnityEngine;

namespace Common.Order
{
    /// <summary>
    /// 注文の表記ルール(具材名・個数ごとの接尾辞)を保持する
    /// </summary>
    [CreateAssetMenu(fileName = "OrderFormatterDatabase", menuName = "Scriptable Objects/OrderFormatterDatabase")]
    public class OrderFormatterDatabase : ScriptableObject
    {
        [SerializeField] private IngredientFormat _yasai;
        [SerializeField] private IngredientFormat _ninniku;
        [SerializeField] private IngredientFormat _abura;

        public IngredientFormat Yasai => _yasai;
        public IngredientFormat Ninniku => _ninniku;
        public IngredientFormat Abura => _abura;
    }
}