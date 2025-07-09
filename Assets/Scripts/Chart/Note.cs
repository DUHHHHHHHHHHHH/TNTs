using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed = 5f;
    public float hitPositionX = 0f;
    public float hitWindow = 0.5f; // tolleranza per colpire la nota (in unità di posizione)

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
                spriteRenderer.sprite = donSprite;
                break;
        }
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < hitPositionX - hitWindow)
        {
            // Destroy(gameObject);
            // Qui puoi aggiungere gestione del miss (es. decremento punteggio)
        }
    }

    // Prova a colpire la nota con un input di tipo inputType
    // Ritorna true se la nota è stata colpita correttamente e distrutta
    public bool TryHit(NoteType inputType)
    {
        float distance = Mathf.Abs(transform.position.x - hitPositionX);

        if (distance <= hitWindow && inputType == noteType)
        {
            Destroy(gameObject);
            Debug.Log($"Nota colpita: Tipo={noteType}, Posizione={transform.position.x}, Tempo={noteTime} ms");
            // Qui puoi aggiungere gestione del successo (es. incremento punteggio, effetti)
            return true;
        }
        return false; // input errato o fuori tempo
    }
}
