using UnityEngine;
using UnityEngine.UI;

public class GameplaySongInfoDisplay : MonoBehaviour
{
    public Text songTitleUnicodeText;
    public Text songArtistUnicodeText;
    public Text songCreatorText;
    public Text songVersionText;

    void Start()
    {
        MetadataPicker metadata = ChartSelectionManager.GetSelectedMetadata();

        if (metadata != null)
        {
            songTitleUnicodeText.text = metadata.TitleUnicode;
            songArtistUnicodeText.text = metadata.ArtistUnicode;
            songCreatorText.text = metadata.Creator;
            songVersionText.text = metadata.Version;
        }
        else
        {
            Debug.LogWarning("Metadata della canzone non trovati nella scena Gameplay.");
            songTitleUnicodeText.text = "N/A";
            songArtistUnicodeText.text = "N/A";
            songCreatorText.text = "N/A";
            songVersionText.text = "N/A";
        }
    }
}
