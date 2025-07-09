using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class ChartSelectionManager : MonoBehaviour
{
    [Header("UI References")]
    public Dropdown FolderDropdown;
    public Dropdown OsuDropdown;
    public Image backgroundImage;

    private List<string> folders = new List<string>();
    private List<string> osuFiles = new List<string>();
    private string selectedFolderPath;
    private string selectedBgPath; // Percorso immagine background selezionata

    void Start()
    {
        LoadFolders();
        FolderDropdown.onValueChanged.AddListener(OnFolderChanged);
        OsuDropdown.onValueChanged.AddListener(OnOsuFileChanged);

        if (folders.Count > 0)
        {
            FolderDropdown.value = 0;
            OnFolderChanged(0);
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
        if (index < 0 || index >= folders.Count) return;

        selectedFolderPath = folders[index];
        LoadOsuFiles(selectedFolderPath);
        LoadBackground(selectedFolderPath);

        if (osuFiles.Count > 0)
        {
            OsuDropdown.value = 0;
            OnOsuFileChanged(0);
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

        selectedBgPath = bgPath; // Salva percorso immagine

        if (File.Exists(bgPath))
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
            Debug.LogWarning("File non trovato in: " + folderPath);
            backgroundImage.sprite = null;
        }
    }

    void OnOsuFileChanged(int index)
    {
        if (index < 0 || index >= osuFiles.Count) return;
        string selectedOsuFile = osuFiles[index];

        Debug.Log($"File .osu selezionato: {selectedOsuFile}");
        PlayerPrefs.SetString("SelectedChart", selectedOsuFile);
    }

    public void OnPlayButtonPressed()
    {
        if (osuFiles.Count == 0) return;

        int selectedIndex = OsuDropdown.value;
        string selectedOsuPath = osuFiles[selectedIndex];

        PlayerPrefs.SetString("SelectedChart", selectedOsuPath);
        PlayerPrefs.SetString("SelectedBgPath", selectedBgPath ?? "");
        SceneManager.LoadScene("Gameplay");
    }
}
