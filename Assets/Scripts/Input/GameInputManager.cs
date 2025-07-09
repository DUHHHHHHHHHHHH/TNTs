using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Note> activeNotes = new List<Note>();
    public InputManager inputManager;

    void Update()
    {
        if (inputManager.IsDonPressed())
        {
            TryHitNote(Note.NoteType.Don);
        }
        else if (inputManager.IsKanPressed())
        {
            TryHitNote(Note.NoteType.Kan);
        }
    }

    void TryHitNote(Note.NoteType inputType)
    {
        Note closestNote = null;
        float minDistance = float.MaxValue;

        foreach (var note in activeNotes)
        {
            float dist = Mathf.Abs(note.transform.position.x - note.hitPositionX);
            if (dist < minDistance && dist <= note.hitWindow)
            {
                minDistance = dist;
                closestNote = note;
            }
        }

        if (closestNote != null)
        {
            bool hit = closestNote.TryHit(inputType);
            if (hit)
            {
                activeNotes.Remove(closestNote);
                // Aggiorna punteggio o mostra feedback positivo
                Debug.Log($"Nota colpita correttamente: Tipo={closestNote.noteType}, Posizione={closestNote.transform.position.x}, Tempo={closestNote.noteTime} ms");
            }
            else
            {

                Debug.Log($"Nota non colpita correttamente: Tipo={closestNote.noteType}, Posizione={closestNote.transform.position.x}, Tempo={closestNote.noteTime} ms");
            }
        }
        else
        {
            
            Debug.Log("Nessuna nota da colpire in questo momento.");
        }
    }

    public void RegisterNote(Note note) { activeNotes.Add(note); }
    public void UnregisterNote(Note note) { activeNotes.Remove(note); }
}
