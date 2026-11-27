//AI
using System;

namespace Common.Order
{
    /// <summary>
    /// ’•¶‚Ì1‰ñ•ª‚ÌŒ¾‹y(‚Ç‚Ì‹ïŞ‚ğ‰½ŒÂ‚Æ‚µ‚ÄŒ¾‚¤‚©)‚ğ•\‚·
    /// </summary>
    public readonly struct IngredientCount
    {
        public Ingredients Ingredient { get; }
        public int Count { get; }

        public IngredientCount(Ingredients ingredient, int count)
        {
            Ingredient = ingredient;
            Count = count;
        }

        public override string ToString()
        {
            return $"{Ingredient}:{Count}";
        }
    }
}