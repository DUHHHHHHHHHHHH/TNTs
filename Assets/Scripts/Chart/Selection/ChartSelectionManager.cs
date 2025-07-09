using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ChartSelectionManager : MonoBehaviour
{
    [Header("UI References")]
    public Dropdown FolderDropdown;
    public Dropdown OsuDropdown;
    public Image backgroundImage;

    [Header("Song Info Display (Chart Selection Scene)")]
    public Text songTitleText;
    public Text songTitleUnicodeText;
    public Text songArtistText;
    public Text songArtistUnicodeText;
    public Text songCreatorText;
    public Text songVersionText;
    public Text songSourceText;
    public Text songTagsText;

    private List<string> folders = new List<string>();
    private List<string> osuFiles = new List<string>();
    private string selectedFolderPath;
    private string selectedBgPath;

    private static MetadataPicker currentSelectedMetadata;

    void Start()
    {
        LoadFolders();
        FolderDropdown.onValueChanged.AddListener(OnFolderChanged);
        OsuDropdown.onValueChanged.AddListener(OnOsuFileChanged);

        if (folders.Count > 0)
        {
            FolderDropdown.value = 0;
        }
        else
        {
            ClearSongInfoDisplay();
        }
    }

    private string GetChartsRootFolder()
    {
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string taikoChartsPath = Path.Combine(documentsPath, "taiko", "taikocharts");
        return taikoChartsPath;
    }

    void LoadFolders()
    {
        folders.Clear();
        FolderDropdown.ClearOptions();

        string rootPath = GetChartsRootFolder();
        if (!Directory.Exists(rootPath))
        {
            Debug.LogError($"Cartella root non trovata: {rootPath}");
            FolderDropdown.AddOptions(new List<string> { "Nessuna cartella trovata" });
            return;
        }

        string[] dirs = Directory.GetDirectories(rootPath);
        List<string> options = new List<string>();

        foreach (var dir in dirs)
        {
            string folderName = Path.GetFileName(dir);
            folders.Add(dir);
            options.Add(folderName);
        }

        if (options.Count == 0)
            options.Add("Nessuna cartella trovata");

        FolderDropdown.AddOptions(options);
    }

    void OnFolderChanged(int index)
    {
        if (index < 0 || index >= folders.Count)
        {
            ClearSongInfoDisplay();
            return;
        }

        selectedFolderPath = folders[index];
        LoadOsuFiles(selectedFolderPath);
        LoadBackground(selectedFolderPath);

        if (osuFiles.Count > 0)
        {
            OsuDropdown.value = 0;
        }
        else
        {
            ClearSongInfoDisplay();
        }
    }

    void LoadOsuFiles(string folderPath)
    {
        osuFiles.Clear();
        OsuDropdown.ClearOptions();

        string[] files = Directory.GetFiles(folderPath, "*.osu");
        List<string> options = new List<string>();

        foreach (var file in files)
        {
            string fileName = Path.GetFileName(file);
            osuFiles.Add(file);
            options.Add(fileName);
        }

        if (options.Count == 0)
            options.Add("Nessun file .osu trovato");

        OsuDropdown.AddOptions(options);
    }

    void LoadBackground(string folderPath)
    {
        string[] possibleExtensions = new[] { ".png", ".jpg", ".jpeg" };
        string bgPath = null;
        foreach (var ext in possibleExtensions)
        {
            var files = Directory.GetFiles(folderPath, "*" + ext, SearchOption.TopDirectoryOnly);
            if (files.Length > 0)
            {
                bgPath = files[0];
                break;
            }
        }

        selectedBgPath = bgPath;

        if (!string.IsNullOrEmpty(bgPath) && File.Exists(bgPath))
        {
            byte[] imageData = File.ReadAllBytes(bgPath);
            Texture2D tex = new Texture2D(2, 2);
            if (tex.LoadImage(imageData))
            {
                backgroundImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                Debug.LogWarning("Impossibile caricare l'immagine di sfondo.");
                backgroundImage.sprite = null;
            }
        }
        else
        {
            backgroundImage.sprite = null;
        }
    }

    void OnOsuFileChanged(int index)
    {
        if (index < 0 || index >= osuFiles.Count)
        {
            ClearSongInfoDisplay();
            currentSelectedMetadata = null;
            return;
        }

        string selectedOsuFile = osuFiles[index];
        Debug.Log($"File .osu selezionato: {selectedOsuFile}");

        currentSelectedMetadata = MetadataPicker.Parse(selectedOsuFile);
        UpdateSongInfoDisplay(currentSelectedMetadata);

        PlayerPrefs.SetString("SelectedChart", selectedOsuFile);
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
        if (currentSelectedMetadata == null)
        {
            Debug.LogError("Nessuna chart selezionata o metadata non disponibili per l'avvio del gioco.");
            return;
        }

        int selectedIndex = OsuDropdown.value;
        string selectedOsuPath = osuFiles[selectedIndex];

        PlayerPrefs.SetString("SelectedChart", selectedOsuPath);
        PlayerPrefs.SetString("SelectedBgPath", selectedBgPath ?? "");
        SceneManager.LoadScene("Gameplay");
    }

    public static MetadataPicker GetSelectedMetadata()
    {
        return currentSelectedMetadata;
    }
}
