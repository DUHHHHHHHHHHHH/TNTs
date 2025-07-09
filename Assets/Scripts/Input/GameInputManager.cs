using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Note> activeNotes = new List<Note>();
    public InputManager inputManager;
    public JudgementDisplay judgementDisplay;
    public SoundManager soundManager;
    public ScoreManager scoreManager;

    public float songTime = 0f; // tempo in millisecondi dall’inizio della mappa
    public float hitWindowMs = 50f; // tolleranza in ms

    private string lastJudgement = "";
    private int totalNotes = 0;

    void Start()
    {
        totalNotes = GetTotalNotesFromMap();
        if (scoreManager != null)
            scoreManager.Initialize(totalNotes);
    }

    void Update()
    {
        songTime += Time.deltaTime * 1000f;

        if (inputManager.IsDonPressed())
        {
            
            TryHitNotes(new List<Note.NoteType> { Note.NoteType.Don, Note.NoteType.FinisherDon });
        }
        else if (inputManager.IsKanPressed())
        {
            Debug.Log("Kan pressed detected");
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
            float timeDiff = Mathf.Abs(closestNote.noteTime - songTime);
            string judgement;
            if (timeDiff <= hitWindowMs * 0.5f)
                judgement = "Marvelous";
            else if (timeDiff <= hitWindowMs)
                judgement = "Great";
            else
                judgement = "Miss";

            bool hit = closestNote.TryHit(closestNote.noteType);
            if (hit)
            {
                activeNotes.Remove(closestNote);

                // Suoni finisher gestiti qui
                if (soundManager != null)
                {
                    switch (closestNote.noteType)
                    {
                        case Note.NoteType.FinisherDon:
                            soundManager.PlayFinisherDon();
                            break;
                        case Note.NoteType.FinisherKan:
                            soundManager.PlayFinisherKan();
                            break;
                    }
                }

                if (scoreManager != null)
                {
                    scoreManager.AddScore(judgement);
                }

                if (judgement != lastJudgement)
                {
                    judgementDisplay.ShowJudgement(judgement);
                    lastJudgement = judgement;
                }
                Debug.Log($"HIT: {judgement}");
            }
            else
            {
                PlayMissSound();
                if (scoreManager != null)
                    scoreManager.OnMiss();
            }
        }
        else
        {
            // Ghosttapping ignorato
            Debug.Log("GhostTapping");
        }
    }

    public void NoteMissed(Note note)
    {
        if (activeNotes.Contains(note))
        {
            activeNotes.Remove(note);
            PlayMissSound();
            if (scoreManager != null)
                scoreManager.OnMiss();
            Debug.Log("MISS: nota persa oltre la finestra di hit");
        }
    }

    private void PlayMissSound()
    {
        if (lastJudgement != "Miss")
        {
            judgementDisplay.ShowJudgement("Miss");
            lastJudgement = "Miss";
        }
        if (soundManager != null)
        {
            soundManager.PlayMiss();
        }
    }

    private int GetTotalNotesFromMap()
    {
        // Calcola o assegna il numero totale di note nella mappa
        // Puoi contare la lista completa delle note o impostarlo manualmente
        return 934; // esempio statico, sostituisci con il valore corretto
    }

    public void RegisterNote(Note note) { activeNotes.Add(note); }
    public void UnregisterNote(Note note) { activeNotes.Remove(note); }
}
