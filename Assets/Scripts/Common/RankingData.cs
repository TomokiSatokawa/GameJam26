using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[Serializable]

public class RankingEntry
{
    public string playerName;
    public int customerCount;
}

[Serializable]
public class RankingData 
{
  public List<RankingEntry> rankings = new List<RankingEntry>();
}

 

