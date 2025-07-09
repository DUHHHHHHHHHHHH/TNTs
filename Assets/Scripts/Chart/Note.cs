using UnityEngine;
using System.Collections.Generic;

public class Note : MonoBehaviour
{
    public float speed = 5f; // velocità in unità al secondo (puoi regolarla)
    public float hitPositionX = 0f;

    [HideInInspector]
    public int noteTime; // tempo in ms in cui la nota deve essere colpita

    public enum NoteType { Don, Kan, FinisherDon, FinisherKan }
    public NoteType noteType;

    public Sprite donSprite;
    public Sprite kanSprite;
    public Sprite finisherDonSprite;
    public Sprite finisherKanSprite;

    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
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
        if (gameManager == null) return;

        // Calcola il tempo rimanente in secondi
        float timeRemaining = (noteTime - gameManager.songTime) / 1000f;

        // Calcola la posizione X in base al tempo rimanente e alla velocità
        // Nota: posizioniamo la hitPositionX come riferimento zero
        transform.position = new Vector3(hitPositionX + speed * timeRemaining, transform.position.y, transform.position.z);

        // Se la nota è passata oltre la hitPositionX + tolleranza, considerala miss e distruggila
        if (gameManager.songTime - noteTime > gameManager.hitWindowMs)
        {
            gameManager.NoteMissed(this);
            Destroy(gameObject);
            gameManager.UnregisterNote(this);
            Debug.Log($"Nota persa: Tipo={noteType}, Tempo nota={noteTime} ms, Tempo attuale={gameManager.songTime:F1} ms");
        }
    }

    public bool TryHit(NoteType inputType)
    {
        if (inputType != noteType) return false;

        float timeDiff = Mathf.Abs(noteTime - gameManager.songTime);
        if (timeDiff <= gameManager.hitWindowMs)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}
