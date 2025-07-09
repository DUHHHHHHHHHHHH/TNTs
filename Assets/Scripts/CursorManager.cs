using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=ELhg7ge2rIA

public class CursorManager : MonoBehaviour
{

    [SerializeField] private Texture2D cursorTexture;
    private Vector2 cursorHotSpot;

    // Start is called before the first frame update
    void Start()
    {
        cursorHotSpot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, cursorHotSpot, CursorMode.Auto);

    }
}
