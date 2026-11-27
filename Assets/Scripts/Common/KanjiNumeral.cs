using System.Text;

namespace Common.Utility
{
    /// <summary>
    /// intを漢数字の文字列に変換する静的ユーティリティ
    /// </summary>
    public static class KanjiNumeral
    {
        private const string Minus = "マイナス";
        private const string Zero = "零";
        private const string PositionalZero = "〇";

        private static readonly string[] Digits = { "", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
        private static readonly string[] SmallUnits = { "", "十", "百", "千" };
        private static readonly string[] BigUnits = { "", "万", "億" };

        /// <summary>
        /// 位取り形式(十・百・千・万・億)に変換する。例: 12345 → 一万二千三百四十五
        /// </summary>
        public static string ToKanji(int value)
        {
            if (value == 0)
            {
                return Zero;
            }

            long abs = value;
            var sb = new StringBuilder();
            if (abs < 0)
            {
                sb.Append(Minus);
                abs = -abs; // int.MinValue対策でlong
            }

            // 4桁ずつ(億・万・一の位)に分けて上位から変換
            for (int group = BigUnits.Length - 1; group >= 0; group--)
            {
                long divisor = Pow10000(group);
                int part = (int)(abs / divisor % 10000);
                if (part == 0)
                {
                    continue;
                }

                sb.Append(ConvertUnderTenThousand(part));
                sb.Append(BigUnits[group]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 桁をそのまま漢数字にする形式に変換する。例: 2024 → 二〇二四
        /// </summary>
        public static string ToKanjiPositional(int value)
        {
            long abs = value;
            var sb = new StringBuilder();
            if (abs < 0)
            {
                sb.Append(Minus);
                abs = -abs;
            }

            foreach (char c in abs.ToString())
            {
                int digit = c - '0';
                sb.Append(digit == 0 ? PositionalZero : Digits[digit]);
            }

            return sb.ToString();
        }

        // 1〜9999を変換。十・百・千の係数「一」は省略する(一十 → 十)
        private static string ConvertUnderTenThousand(int value)
        {
            var sb = new StringBuilder();
            for (int pos = SmallUnits.Length - 1; pos >= 0; pos--)
            {
                int digit = value / (int)System.Math.Pow(10, pos) % 10;
                if (digit == 0)
                {
                    continue;
                }

                bool omitOne = digit == 1 && pos > 0;
                if (!omitOne)
                {
                    sb.Append(Digits[digit]);
                }

                sb.Append(SmallUnits[pos]);
            }

            return sb.ToString();
        }

        private static long Pow10000(int exponent)
        {
            long result = 1;
            for (int i = 0; i < exponent; i++)
            {
                result *= 10000;
            }

            return result;
        }
    }
}