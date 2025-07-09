using UnityEngine;

public class InputSquaresController : MonoBehaviour
{
    [System.Serializable]
    public struct KeySquare
    {
        public KeyCode key;
        public SpriteRenderer squareImage;
        public Color activeColor;
        public Color inactiveColor;
    }

    public KeySquare[] keySquares;

    void Start()
    {
        foreach (var ks in keySquares)
        {
            if (ks.squareImage != null)
                ks.squareImage.color = new Color(ks.inactiveColor.r, ks.inactiveColor.g, ks.inactiveColor.b, 1f);
        }
    }

    void Update()
    {
        foreach (var ks in keySquares)
        {
            if (ks.squareImage == null) continue;

            if (Input.GetKeyDown(ks.key))
            {
                ks.squareImage.color = new Color(ks.activeColor.r, ks.activeColor.g, ks.activeColor.b, 1f);
                Debug.Log($"Key {ks.key} premuta / attiva");
            }
            if (Input.GetKeyUp(ks.key))
            {
                ks.squareImage.color = new Color(ks.inactiveColor.r, ks.inactiveColor.g, ks.inactiveColor.b, 1f);
                Debug.Log($"Key {ks.key} rilasciata / inattiva");
            }
        }
    }
}
