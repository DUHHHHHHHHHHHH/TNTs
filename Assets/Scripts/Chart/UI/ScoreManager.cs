using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text TextScorePunteggio;         // Assegna in Inspector
    public Text TextScoreAccuracy;          // Assegna in Inspector
    public Text TextScoreCombo;             // Testo per mostrare la combo

    public Text TextCountMarvelous;         // Nuovo: contatore Marvelous
    public Text TextCountGreat;             // Nuovo: contatore Great
    public Text TextCountMiss;              // Nuovo: contatore Miss

    public int maxScore = 1000000; // un milione di punti massimo
    private int totalNotes = 0;
    private int currentScore = 0;
    private int currentCombo = 0;  // Contatore combo

    private int countMarvelous = 0;
    private int countGreat = 0;
    private int countMiss = 0;

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
        currentCombo = 0;

        countMarvelous = 0;
        countGreat = 0;
        countMiss = 0;

        UpdateScoreUI();
        UpdateComboUI();
        UpdateCountUI();
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

            if (judgement == "Marvelous")
            {
                countMarvelous++;
                currentCombo++;
            }
            else if (judgement == "Great")
            {
                countGreat++;
                currentCombo++;
            }
        }
        else
        {
            countMiss++;
            currentCombo = 0;
        }

        UpdateScoreUI();
        UpdateComboUI();
        UpdateCountUI();
    }

    // GameManager chiama questa funzione quando c'è un miss
    public void OnMiss()
    {
        countMiss++;
        currentCombo = 0;
        UpdateComboUI();
        UpdateScoreUI();
        UpdateCountUI();
    }

    private void UpdateScoreUI()
    {
        TextScorePunteggio.text = $"Score: {currentScore:N0}";

        float accuracy = totalNotes > 0 ? ((float)currentScore / maxScore) * 100f : 0f;
        TextScoreAccuracy.text = $"Accuracy: {accuracy:F2}%";
    }

    private void UpdateComboUI()
    {
        if (currentCombo > 0)
        {
            TextScoreCombo.gameObject.SetActive(true);
            TextScoreCombo.text = $"Combo: {currentCombo}";
        }
        else
        {
            TextScoreCombo.text = $"Combo: 0";
            // oppure puoi nascondere il testo combo qui se preferisci
            // TextScoreCombo.gameObject.SetActive(false);
        }
    }

    private void UpdateCountUI()
    {
        if (TextCountMarvelous != null)
            TextCountMarvelous.text = $"Marvelous: {countMarvelous}";

        if (TextCountGreat != null)
            TextCountGreat.text = $"Great: {countGreat}";

        if (TextCountMiss != null)
            TextCountMiss.text = $"Miss: {countMiss}";
    }
}
