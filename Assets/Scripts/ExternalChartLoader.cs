using System;
using System.IO;
using UnityEngine;

public class ExternalChartLoader : MonoBehaviour
{
    private string externalFolderPath;

    public ChartReader chartReader; // Riferimento da assegnare nell'Inspector

    void Start()
    {
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        externalFolderPath = Path.Combine(documentsPath, "TaikoCharts");

        if (Directory.Exists(externalFolderPath))
        {
            string[] osuFiles = Directory.GetFiles(externalFolderPath, "*.osu");
            foreach (string file in osuFiles)
            {
                Debug.Log("Trovato file chart: " + file);
                chartReader.ReadChart(file);
                chartReader.SpawnAllNotes();
                break;
            }
        }
        else
        {
            Debug.LogWarning("Cartella esterna non trovata: " + externalFolderPath);
        }
    }
}
