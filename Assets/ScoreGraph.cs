using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGraph : MonoBehaviour
{
    public GameObject barPrefab;
    public Transform graphContainer;
    public float barWidth = 50f;
    public float maxHeight = 200f;
    public GameObject backButton;

    private List<int> pastScores;
    private Image barSprite;
    public TMP_FontAsset glutenFont;

    public void ShowGraph(List<UIManager.ScoreEntry> scores)
    {
        graphContainer.gameObject.SetActive(true);
        backButton.SetActive(true);
        pastScores = scores.Select(entry => entry.score).ToList();

        foreach (Transform child in graphContainer)
        {
            Destroy(child.gameObject);
        }

        if (pastScores.Count == 0)
        {
            Debug.LogWarning("No scores to display in graph.");
            return;
        }

        float maxScore = pastScores.Max();
        for (int i = 0; i < pastScores.Count; i++)
        {
            // Create Bar
            GameObject bar = Instantiate(barPrefab, graphContainer);
            GameObject dateLabelObj = new GameObject("DateLabel");
            dateLabelObj.transform.SetParent(bar.transform);
           
            RectTransform barRect = bar.GetComponent<RectTransform>();
            float barHeight = (float)pastScores[i] / maxScore * maxHeight;
            barRect.sizeDelta = new Vector2(barWidth, barHeight);
            float xPos = i * (barWidth + 10f);
            barRect.anchoredPosition = new Vector2(xPos + 10, 100f);

            barSprite = bar.GetComponent<Image>();
            if (barSprite != null)
            {
                barSprite.color = pastScores[i] > 8 ? Color.red :
                                  pastScores[i] > 4 ? Color.yellow : Color.green;
            }

            TextMeshProUGUI dateLabel = dateLabelObj.AddComponent<TextMeshProUGUI>();
            RectTransform labelRect = dateLabel.GetComponent<RectTransform>();

            labelRect.sizeDelta = new Vector2(barWidth, 70);
            labelRect.anchoredPosition = new Vector2(0 , 200f);

            dateLabel.text = scores[i].date;
            dateLabel.color = Color.black;
            dateLabel.fontSize = 14;
            dateLabel.alignment = TextAlignmentOptions.Left;
            dateLabel.font = glutenFont;
        }
    }
}
