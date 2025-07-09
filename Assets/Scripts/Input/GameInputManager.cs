using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Note> activeNotes = new List<Note>();
    public InputManager inputManager;
    public JudgementDisplay judgementDisplay;
    public SoundManager soundManager;

    public float songTime = 0f; // tempo in millisecondi dall’inizio della mappa
    public float hitWindowMs = 50f; // tolleranza in ms

    private string lastJudgement = "";

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

    public void RegisterNote(Note note) { activeNotes.Add(note); }
    public void UnregisterNote(Note note) { activeNotes.Remove(note); }
}
