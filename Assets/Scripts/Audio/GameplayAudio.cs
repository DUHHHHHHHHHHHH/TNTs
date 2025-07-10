using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameplayAudio : MonoBehaviour
{
    public AudioSource audioSource;

    private string audioFilePath;

    void Start()
    {
        string selectedChartPath = PlayerPrefs.GetString("SelectedChart", "");
        if (string.IsNullOrEmpty(selectedChartPath))
        {
            Debug.LogError("Nessun chart selezionato, impossibile caricare audio.");
            return;
        }

        // La cartella del chart
        string chartFolder = Path.GetDirectoryName(selectedChartPath);

        // Cerca il file audio nella cartella (audio.mp3 o audio.ogg)
        string[] audioExtensions = { ".mp3", ".ogg", ".wav" };
        audioFilePath = null;
        foreach (var ext in audioExtensions)
        {
            string possiblePath = Path.Combine(chartFolder, "audio" + ext);
            if (File.Exists(possiblePath))
            {
                audioFilePath = possiblePath;
                break;
            }
        }

        if (string.IsNullOrEmpty(audioFilePath))
        {
            Debug.LogError("File audio non trovato nella cartella chart: " + chartFolder);
            return;
        }

        // Avvia la coroutine per caricare e riprodurre l’audio
        StartCoroutine(LoadAndPlayAudio(audioFilePath));
    }

    IEnumerator LoadAndPlayAudio(string path)
    {
        string url = "file:///" + path.Replace("\\", "/");

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Errore caricamento audio: " + www.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = clip;
                audioSource.volume = 0.1f; // Volume a 0.1f (se no spacca i timpani)

                
                double delayInSeconds = 0;
                double scheduledTime = AudioSettings.dspTime + delayInSeconds;
                audioSource.PlayScheduled(scheduledTime);
                
            }
        }
    }
}
