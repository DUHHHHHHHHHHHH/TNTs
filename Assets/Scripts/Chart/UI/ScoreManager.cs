using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;      // Assegna in Inspector
    public Text accuracyText;   // Assegna in Inspector

    public int maxScore = 1000000; // un milione di punti massimo, come su osu in scoreV2
    private int totalNotes = 0;
    private int currentScore = 0;

    private float baseNoteScore = 0f;

    // Moltiplicatori punteggio per giudizi
    private readonly System.Collections.Generic.Dictionary<string, float> judgementMultipliers = new System.Collections.Generic.Dictionary<string, float>()
    {
        {"Marvelous", 1.0f},
        {"Great", 0.75f},
        {"Miss", 0f}
    };

    public void Initialize(int totalNotesCount)
    {
        totalNotes = totalNotesCount;
        baseNoteScore = totalNotes > 0 ? (float)maxScore / totalNotes : 0f;
        currentScore = 0;
        UpdateScoreUI();
    }

    // GameManager chiama questa funzione quando una nota viene colpita
    public void AddScore(string judgement)
    {
        if (!judgementMultipliers.ContainsKey(judgement))
            return;

        if (judgement != "Miss")
        {
            int pointsToAdd = Mathf.RoundToInt(baseNoteScore * judgementMultipliers[judgement]);
            currentScore += pointsToAdd;
        }

        UpdateScoreUI();
    }

    // GameManager chiama questa funzione quando c'è un miss
    public void OnMiss()
    {
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = $"Score: {currentScore:N0}";

        float accuracy = totalNotes > 0 ? ((float)currentScore / maxScore) * 100f : 0f;
        accuracyText.text = $"Accuracy: {accuracy:F2}%";
    }
}
