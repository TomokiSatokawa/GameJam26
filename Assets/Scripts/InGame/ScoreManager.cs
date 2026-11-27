/// <summary>
/// ゲームの評価スコアを管理する
/// </summary>
public static class ScoreManager
{
    public static int NinnikuScore { get; private set; }
    public static int YasaiScore { get; private set; }
    public static int AburaScore { get; private set; }
    public static int CustomerCount { get; private set; }

    public static int TotalScore => (NinnikuScore + YasaiScore + AburaScore) * 100 + (CustomerCount * 500);

    /// <summary>
    /// スコアをリセットする
    /// </summary>
    public static void ResetScore()
    {
        NinnikuScore = 0;
        YasaiScore = 0;
        AburaScore = 0;
        CustomerCount = 0;
    }

    /// <summary>
    /// ニンニクのスコアを加算する
    /// </summary>
    public static void AddNinniku(int score = 1)
    {
        NinnikuScore += score;
    }

    /// <summary>
    /// 野菜のスコアを加算する
    /// </summary>
    public static void AddYasai(int score = 1)
    {
        YasaiScore += score;
    }

    /// <summary>
    /// アブラのスコアを加算する
    /// </summary>
    public static void AddAbura(int score = 1)
    {
        AburaScore += score;
    }

    /// <summary>
    /// 客の数を加算する
    /// </summary>
    public static void AddCustomer(int count = 1)
    {
        CustomerCount += count;
    }
}