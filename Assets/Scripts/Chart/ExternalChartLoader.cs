using System.IO;
using UnityEngine;

public class ExternalChartLoader : MonoBehaviour
{
    public ChartReader chartReader; // Assegna in Inspector

    public void LoadChart(string chartFilePath)
    {
        if (File.Exists(chartFilePath))
        {
            Debug.Log("Carico file chart: " + chartFilePath);
            chartReader.ReadChart(chartFilePath);
            chartReader.SpawnAllNotes();
        }
        else
        {
            Debug.LogWarning("File chart non trovato: " + chartFilePath);
        }
    }

    void Start()
{
    string selectedChart = PlayerPrefs.GetString("SelectedChart", "");
    if (!string.IsNullOrEmpty(selectedChart))
    {
        LoadChart(selectedChart);
    }
    else
    {
        Debug.LogWarning("Nessun chart selezionato.");
    }
}

}
