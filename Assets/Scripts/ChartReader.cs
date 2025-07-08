using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChartReader : MonoBehaviour
{
    public List<int> noteTimes = new List<int>();

    public NoteSpawner noteSpawner; // Riferimento da assegnare nell'Inspector

    public void ReadChart(string path)
    {
        noteTimes.Clear();

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
                if (parts.Length >= 3)
                {
                    int time = int.Parse(parts[2]);
                    noteTimes.Add(time);
                }
            }
        }
    }

    public void SpawnAllNotes()
    {
        noteSpawner.SpawnAllNotes(noteTimes);
    }
}
