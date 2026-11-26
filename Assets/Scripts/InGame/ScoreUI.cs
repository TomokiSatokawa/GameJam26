using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI ninnikuScoreText;
    public TextMeshProUGUI yasaiScoreText;
    public TextMeshProUGUI aburaScoreText;
    public TextMeshProUGUI customerScoreText;
    public GameManager gameManager;
    public GameObject resultPanel;
    public GameObject ninnikuPrefab;
    public GameObject yasaiPrefab;
    public GameObject aburaPrefab;
    public Transform ninnikuItems;
    public Transform yasaiItems;
    public Transform aburaItems;
    private bool resultGenerated;

    private void Start()
    {
        resultPanel.SetActive(false);
    }

    private void Update()
    {
        if (gameManager.currentState == GameManager.GameState.Result
            && !resultGenerated)
        {
            resultPanel.SetActive(true);

            ninnikuScoreText.text = ScoreManager.NinnikuScore.ToString();
            yasaiScoreText.text = ScoreManager.YasaiScore.ToString();
            aburaScoreText.text = ScoreManager.AburaScore.ToString();
            customerScoreText.text = ScoreManager.CustomerCount.ToString();

            CreateItems(ninnikuPrefab, ninnikuItems, ScoreManager.NinnikuScore);
            CreateItems(yasaiPrefab, yasaiItems, ScoreManager.YasaiScore);
            CreateItems(aburaPrefab, aburaItems, ScoreManager.AburaScore);

            resultGenerated = true;
        }
    }

    private void CreateItems(GameObject prefab, Transform parent, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(prefab, parent);
        }
    }
}
