using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Note> activeNotes = new List<Note>();
    public InputManager inputManager;

    public float songTime = 0f; // tempo in millisecondi dall’inizio della mappa
    public float hitWindowMs = 100f; // tolleranza in ms

    void Update()
    {
        songTime += Time.deltaTime * 1000f;

        if (inputManager.IsDonPressed())
        {
            TryHitNotes(new List<Note.NoteType> { Note.NoteType.Don, Note.NoteType.FinisherDon });
        }
        else if (inputManager.IsKanPressed())
        {
            TryHitNotes(new List<Note.NoteType> { Note.NoteType.Kan, Note.NoteType.FinisherKan });
        }
    }

    void TryHitNotes(List<Note.NoteType> inputTypes)
    {
        Note closestNote = null;
        float minTimeDiff = float.MaxValue;

        foreach (var note in activeNotes)
        {
            if (!inputTypes.Contains(note.noteType)) continue;

            float timeDiff = Mathf.Abs(note.noteTime - songTime);
            if (timeDiff <= hitWindowMs && timeDiff < minTimeDiff)
            {
                minTimeDiff = timeDiff;
                closestNote = note;
            }
        }

        if (closestNote != null)
        {
            bool hit = closestNote.TryHit(closestNote.noteType);
            if (hit)
            {
                activeNotes.Remove(closestNote);
                Debug.Log($"HIT: Tipo={closestNote.noteType}, Tempo nota={closestNote.noteTime} ms, Tempo attuale={songTime:F1} ms");
            }
            else
            {
                Debug.Log($"MISS: Tipo={closestNote.noteType}, Tempo nota={closestNote.noteTime} ms, Tempo attuale={songTime:F1} ms");
            }
        }
        else
        {
            Debug.Log("MISS: nessuna nota colpita.");
        }
    }

    public void RegisterNote(Note note) { activeNotes.Add(note); }
    public void UnregisterNote(Note note) { activeNotes.Remove(note); }
}
