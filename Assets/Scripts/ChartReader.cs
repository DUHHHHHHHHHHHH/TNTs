using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChartReader : MonoBehaviour
{
    public List<int> noteTimes = new List<int>();
    public List<Note.NoteType> noteTypes = new List<Note.NoteType>();

    public NoteSpawner noteSpawner; // Assegna nello Inspector

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
                    int typeValue = int.Parse(parts[3]);

                    noteTimes.Add(time);

                    // Determina il tipo di nota in base al valore type
                    // Qui un esempio semplice, adatta se serve
                    if ((typeValue & 1) > 0)
                        noteTypes.Add(Note.NoteType.Don);
                    else if ((typeValue & 8) > 0)
                        noteTypes.Add(Note.NoteType.Kan);
                    else
                        noteTypes.Add(Note.NoteType.Don); // default
                }
            }
        }

        /* NormalizeNoteTimes(); */
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

    public void SpawnAllNotes()
    {
        noteSpawner.SpawnAllNotes(noteTimes, noteTypes);
    }
}
