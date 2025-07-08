using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed = 5f;
    public float hitPositionX = 0f;

    [HideInInspector]
    public int noteTime; // tempo della nota in millisecondi

    public enum NoteType { Don, Kan }
    public NoteType noteType;

    public Sprite donSprite;
    public Sprite kanSprite;

    private SpriteRenderer spriteRenderer;

     void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Imposta lo sprite in base al tipo di nota
        if (noteType == NoteType.Don)
            spriteRenderer.sprite = donSprite;
        else
            spriteRenderer.sprite = kanSprite;
    }

    void Update()
{
    // Movimento indipendente dal frame rate
    transform.position += Vector3.left * speed * Time.deltaTime;

    if (transform.position.x <= hitPositionX)
    {
        Destroy(gameObject);
    }
}

}
