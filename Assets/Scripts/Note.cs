using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed = 5f;
    public float hitPositionX = 0f;

    [HideInInspector]
    public int noteTime;

    public enum NoteType { Don, Kan }
    public NoteType noteType;

    public Sprite donSprite;  // Assegna nello Inspector lo sprite per Don
    public Sprite kanSprite;  // Assegna nello Inspector lo sprite per Kan

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Cambia sprite in base al tipo di nota
        if (noteType == NoteType.Don)
            spriteRenderer.sprite = donSprite;
        else if (noteType == NoteType.Kan)
            spriteRenderer.sprite = kanSprite;
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= hitPositionX)
        {
            Destroy(gameObject);
        }
    }
}
