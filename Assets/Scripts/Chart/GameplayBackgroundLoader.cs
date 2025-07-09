using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameplayBackgroundLoader : MonoBehaviour
{
    public Image backgroundImage;

    void Start()
    {
        string bgPath = PlayerPrefs.GetString("SelectedBgPath", "");
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
                Debug.LogWarning("Impossibile caricare l'immagine di sfondo nella scena Gameplay.");
            }
        }
        else
        {
            Debug.LogWarning("Percorso immagine background non valido o file non trovato.");
        }
    }
}
