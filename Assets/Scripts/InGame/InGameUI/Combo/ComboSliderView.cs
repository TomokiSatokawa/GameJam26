using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// コンボスライダーの表示。
/// ・fillImage(Image Type=Filled)のfillAmountでゲージを動かす
/// ・「どこで何秒増えるか」をスライダー上にマーカー表示する
/// ・秒数が増えたときに「+N秒」をポップアップ表示する
/// </summary>
public class ComboSliderView : MonoBehaviour
{
    [SerializeField] private ComboSystem comboSystem;

    [Header("Slider")]
    [SerializeField] private Image fillImage;          // Image Type = Filled / Horizontal
    [SerializeField] private float fillSpeed = 3f;     // 1秒あたりのfillAmount変化量
    [SerializeField] private TMP_Text comboText;       // 「12 COMBO」など

    [Header("Markers (どこで何秒)")]
    [SerializeField] private RectTransform markerRoot; // スライダーのバーと同じ範囲のRectTransform
    [SerializeField] private TMP_Text markerPrefab;    // 「+2秒」を表示するテキスト
    [SerializeField] private Color reachedColor = Color.yellow;
    [SerializeField] private Color notReachedColor = Color.white;
    [SerializeField] private float markerY = 0f;       // マーカーのY位置（markerRoot基準、固定）

    [Header("Time Popup")]
    [SerializeField] private TMP_Text timePopup;       // 「+2秒」ポップアップ
    [SerializeField] private float popupDuration = 1f;

    private class Marker
    {
        public int combo;
        public TMP_Text label;
    }

    private readonly List<Marker> markers = new List<Marker>();
    private float targetFill;
    private Coroutine popupRoutine;

    private void Awake()
    {
        BuildMarkers();
        if (timePopup != null) timePopup.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        comboSystem.OnComboChanged += HandleComboChanged;
        comboSystem.OnTimeAdded += HandleTimeAdded;
        HandleComboChanged(comboSystem.Combo);
        fillImage.fillAmount = targetFill;
    }

    private void OnDisable()
    {
        comboSystem.OnComboChanged -= HandleComboChanged;
        comboSystem.OnTimeAdded -= HandleTimeAdded;
    }

    private void Update()
    {
        fillImage.fillAmount = Mathf.MoveTowards(fillImage.fillAmount, targetFill, fillSpeed * Time.deltaTime);
    }

    // スライダー上の「何コンボ目で何秒」マーカーを生成
    private void BuildMarkers()
    {
        var data = comboSystem.Data;
        foreach (var m in data.Milestones)
        {
            var label = Instantiate(markerPrefab, markerRoot);
            label.text = $"+{m.bonusSeconds}秒";

            var rt = label.rectTransform;
            Vector2 size = rt.rect.size; // アンカー変更前にサイズを退避

            // xは割合、yは中央に固定（縦方向のストレッチはしない）
            float ratio = Mathf.Clamp01((float)m.combo / data.MaxCombo);
            rt.anchorMin = new Vector2(ratio, 0.5f);
            rt.anchorMax = new Vector2(ratio, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = size;
            rt.anchoredPosition = new Vector2(0f, markerY);

            markers.Add(new Marker { combo = m.combo, label = label });

            label.gameObject.SetActive(true);
        }
    }

    private void HandleComboChanged(int combo)
    {
        targetFill = Mathf.Clamp01((float)combo / comboSystem.Data.MaxCombo);

        // 最大コンボ到達時は一度満タンにしてから、0に向かって減らす
        if (combo >= comboSystem.Data.MaxCombo) fillImage.fillAmount = 1f;

        if (comboText != null) comboText.text = $"{combo} COMBO";

        foreach (var marker in markers)
        {
            marker.label.color = combo >= marker.combo ? reachedColor : notReachedColor;
        }
    }

    private void HandleTimeAdded(int seconds)
    {
        if (timePopup == null) return;
        if (popupRoutine != null) StopCoroutine(popupRoutine);
        popupRoutine = StartCoroutine(PopupRoutine(seconds));
    }

    private IEnumerator PopupRoutine(int seconds)
    {
        timePopup.text = $"+{seconds}秒";
        timePopup.gameObject.SetActive(true);
        yield return new WaitForSeconds(popupDuration);
        timePopup.gameObject.SetActive(false);
        popupRoutine = null;
    }
}