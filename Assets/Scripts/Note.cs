using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed = 5f;
    public float hitPositionX = 0f;

    [HideInInspector]
    public int noteTime;

    public enum NoteType { Don, Kan, FinisherDon, FinisherKan }
    public NoteType noteType;

    public Sprite donSprite;
    public Sprite kanSprite;
    public Sprite finisherDonSprite;  
    public Sprite finisherKanSprite;   

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        switch (noteType)
        {
            case NoteType.Don:
                spriteRenderer.sprite = donSprite;
                break;
            case NoteType.FinisherDon:
                spriteRenderer.sprite = finisherDonSprite;
                break;
            case NoteType.Kan:
                spriteRenderer.sprite = kanSprite;
                break;
            case NoteType.FinisherKan:
                spriteRenderer.sprite = finisherKanSprite;
                break;
            default:
                spriteRenderer.sprite = donSprite; // fallback
                break;
        }
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
