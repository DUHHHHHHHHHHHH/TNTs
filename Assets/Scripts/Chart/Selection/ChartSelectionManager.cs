using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ChartSelectionManager : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform folderListContent; // Content della ScrollView dei folder (destra)
    public RectTransform chartListContent;  // Content della ScrollView dei chart (sinistra)
    public GameObject folderItemPrefab;     // Prefab FolderItem (solo bottone con nome folder)
    public GameObject chartItemPrefab;      // Prefab ChartItem (bottone con nome chart)

    [Header("Background & Info UI")]
    public Image backgroundImage;
    public Text songTitleText;
    public Text songTitleUnicodeText;
    public Text songArtistText;
    public Text songArtistUnicodeText;
    public Text songCreatorText;
    public Text songVersionText;
    public Text songSourceText;
    public Text songTagsText;

    private List<string> folders = new List<string>();
    private Dictionary<string, List<string>> folderCharts = new Dictionary<string, List<string>>();

    private string selectedChartPath;
    private string selectedBgPath;

    private static MetadataPicker currentSelectedMetadata;

    void Start()
    {
        LoadFoldersAndCharts();
        PopulateFolderList();
        ClearSongInfoDisplay();
    }

    private string GetChartsRootFolder()
    {
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string taikoChartsPath = Path.Combine(documentsPath, "Taiko", "TaikoCharts");
        return taikoChartsPath;
    }

    void LoadFoldersAndCharts()
    {
        folders.Clear();
        folderCharts.Clear();

        string rootPath = GetChartsRootFolder();
        if (!Directory.Exists(rootPath))
        {
            Debug.LogError($"Cartella root non trovata: {rootPath}");
            return;
        }

        string[] dirs = Directory.GetDirectories(rootPath);
        foreach (var dir in dirs)
        {
            string folderName = Path.GetFileName(dir);
            folders.Add(folderName);

            string[] osuFiles = Directory.GetFiles(dir, "*.osu");
            folderCharts[folderName] = new List<string>(osuFiles);

            // Carica background immagine folder (primo file immagine trovato)
            string[] possibleExtensions = new[] { ".png", ".jpg", ".jpeg" };
            foreach (var ext in possibleExtensions)
            {
                var imgs = Directory.GetFiles(dir, "*" + ext);
                if (imgs.Length > 0)
                {
                    selectedBgPath = imgs[0]; // Puoi gestire meglio se vuoi background per folder
                    break;
                }
            }
        }
    }

    void PopulateFolderList()
    {
        // Pulisci prima
        foreach (Transform child in folderListContent)
            Destroy(child.gameObject);

        foreach (var folderName in folders)
        {
            GameObject folderGO = Instantiate(folderItemPrefab, folderListContent);
            folderGO.name = "Folder_" + folderName;

            Button folderButton = folderGO.GetComponentInChildren<Button>();
            Text folderButtonText = folderButton.GetComponentInChildren<Text>();

            folderButtonText.text = folderName;

            // Listener per selezionare il folder e popolare la lista chart
            folderButton.onClick.AddListener(() =>
            {
                PopulateChartList(folderCharts[folderName]);
            });
        }
    }

    void PopulateChartList(List<string> chartPaths)
    {
        // Pulisci prima
        foreach (Transform child in chartListContent)
            Destroy(child.gameObject);

        foreach (var chartPath in chartPaths)
        {
            GameObject chartGO = Instantiate(chartItemPrefab, chartListContent);
            chartGO.name = "Chart_" + Path.GetFileNameWithoutExtension(chartPath);

            Button chartButton = chartGO.GetComponent<Button>();
            Text chartButtonText = chartButton.GetComponentInChildren<Text>();
            chartButtonText.text = Path.GetFileName(chartPath);

            chartButton.onClick.AddListener(() =>
            {
                OnChartSelected(chartPath);
            });
        }
    }

    void OnChartSelected(string chartPath)
{
    if (selectedChartPath == chartPath)
    {
        // Se clicchi di nuovo lo stesso chart, avvia subito il gameplay
        OnPlayButtonPressed();
        return;
    }

    selectedChartPath = chartPath;

    // Carica background come sopra
    string folderPath = Path.GetDirectoryName(chartPath);
    selectedBgPath = null;
    string[] possibleExtensions = new[] { ".png", ".jpg", ".jpeg" };
    foreach (var ext in possibleExtensions)
    {
        var imgs = Directory.GetFiles(folderPath, "*" + ext);
        if (imgs.Length > 0)
        {
            selectedBgPath = imgs[0];
            break;
        }
    }

    currentSelectedMetadata = MetadataPicker.Parse(chartPath);
    UpdateSongInfoDisplay(currentSelectedMetadata);
    LoadBackgroundFromChart();

    PlayerPrefs.SetString("SelectedChart", chartPath);
}

    void LoadBackgroundFromChart()
    {
        if (string.IsNullOrEmpty(selectedBgPath) || !File.Exists(selectedBgPath))
        {
            backgroundImage.sprite = null;
            return;
        }

        byte[] imageData = File.ReadAllBytes(selectedBgPath);
        Texture2D tex = new Texture2D(2, 2);
        if (tex.LoadImage(imageData))
        {
            backgroundImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            backgroundImage.sprite = null;
        }
    }

    private void UpdateSongInfoDisplay(MetadataPicker metadata)
    {
        if (metadata == null)
        {
            ClearSongInfoDisplay();
            return;
        }

        songTitleText.text = "Title: " + (string.IsNullOrEmpty(metadata.Title) ? "N/A" : metadata.Title);
        songTitleUnicodeText.text = "Title Unicode: " + (string.IsNullOrEmpty(metadata.TitleUnicode) ? "N/A" : metadata.TitleUnicode);
        songArtistText.text = "Artist: " + (string.IsNullOrEmpty(metadata.Artist) ? "N/A" : metadata.Artist);
        songArtistUnicodeText.text = "Artist Unicode: " + (string.IsNullOrEmpty(metadata.ArtistUnicode) ? "N/A" : metadata.ArtistUnicode);
        songCreatorText.text = "Creator: " + (string.IsNullOrEmpty(metadata.Creator) ? "N/A" : metadata.Creator);
        songVersionText.text = "Version: " + (string.IsNullOrEmpty(metadata.Version) ? "N/A" : metadata.Version);
        songSourceText.text = "Source: " + (string.IsNullOrEmpty(metadata.Source) ? "N/A" : metadata.Source);
        songTagsText.text = "Tags: " + (string.IsNullOrEmpty(metadata.Tags) ? "N/A" : metadata.Tags);
    }

    private void ClearSongInfoDisplay()
    {
        songTitleText.text = "Title: ";
        songTitleUnicodeText.text = "Title Unicode: ";
        songArtistText.text = "Artist: ";
        songArtistUnicodeText.text = "Artist Unicode: ";
        songCreatorText.text = "Creator: ";
        songVersionText.text = "Version: ";
        songSourceText.text = "Source: ";
        songTagsText.text = "Tags: ";
    }

    public void OnPlayButtonPressed()
    {
        if (string.IsNullOrEmpty(selectedChartPath))
        {
            Debug.LogError("Nessuna chart selezionata");
            return;
        }

        PlayerPrefs.SetString("SelectedChart", selectedChartPath);
        PlayerPrefs.SetString("SelectedBgPath", selectedBgPath ?? "");
        SceneManager.LoadScene("Gameplay");
    }

    public static MetadataPicker GetSelectedMetadata()
    {
        return currentSelectedMetadata;
    }
}
