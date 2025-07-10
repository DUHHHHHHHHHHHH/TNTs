using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChartReader : MonoBehaviour
{
    public List<int> noteTimes = new List<int>();
    public List<Note.NoteType> noteTypes = new List<Note.NoteType>();
    public NoteSpawner noteSpawner;

    public void ReadChart(string path)
    {
        noteTimes.Clear();
        noteTypes.Clear();

        bool hitObjectsSection = false;
        foreach (var line in File.ReadLines(path))
        {
            if (line.StartsWith("[HitObjects]"))
            {
                hitObjectsSection = true;
                continue;
            }
            if (hitObjectsSection)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(',');
                if (parts.Length >= 4)
                {
                    int time = int.Parse(parts[2]);
                    int typeValue = int.Parse(parts[4]);

                    // Debug.Log($"Read note: Time={time} ms, TypeValue={typeValue}");

                    noteTimes.Add(time);

                    // Determina il tipo di nota in base al valore type
                    // Rileva sia Don che Kan correttamente

                    /*

                    0 = Don
                    2, 8, 10 = Kan
                    4 = FinisherDon
                    6, 12, 14 = FinisherKan

                    da gestire in futuro:

                    - SLIDER: 101,102,4637,2,0,L|265:103,1,140
                    - DRUM-ROLLS: 256,192,5195,12,0,5474,0:0:0:0:

                    */

                    if (typeValue == 0) { noteTypes.Add(Note.NoteType.Don); }
                    else if (typeValue == 2 || typeValue == 8 || typeValue == 10)
                    {
                        noteTypes.Add(Note.NoteType.Kan);
                    }
                    else if (typeValue == 4) { noteTypes.Add(Note.NoteType.FinisherDon); }
                    else if (typeValue == 6 || typeValue == 12 || typeValue == 14)
                    { noteTypes.Add(Note.NoteType.FinisherKan); }
                    else { noteTypes.Add(Note.NoteType.Don); }

                }

            }
        }

        /* NormalizeNoteTimes(); */ // se si vuole la prima nota a 0ms
        AddDelayToNoteTimes(100); // Aggiungi un ritardo di 100ms a tutte le note
    }

    void NormalizeNoteTimes()
    {
        if (noteTimes.Count == 0) return;

        int firstNoteTime = noteTimes[0];
        for (int i = 0; i < noteTimes.Count; i++)
        {
            noteTimes[i] = noteTimes[i] - firstNoteTime;
        }
    }

    void AddDelayToNoteTimes(int delayMs)
    {
        for (int i = 0; i < noteTimes.Count; i++)
        {
            noteTimes[i] += delayMs;
        }
    }

    public void SpawnAllNotes() { noteSpawner.SpawnAllNotes(noteTimes, noteTypes); }
}
