using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;
    public Transform spawnPoint;

    public float baseScrollSpeed = 5f;
    public float userScrollSpeed = 1f;

        public void SpawnAllNotes(List<int> noteTimes, List<Note.NoteType> noteTypes)
{
    float effectiveSpeed = baseScrollSpeed * userScrollSpeed;

    for (int i = 0; i < noteTimes.Count; i++)
        {
            // Calcola la distanza dalla linea di hit in base al tempo assoluto della nota
            float distance = noteTimes[i] * effectiveSpeed * 0.001f; // ms -> s

            // Posizione di spawn a destra della spawnPoint
            Vector3 pos = spawnPoint.position + Vector3.right * distance;

            // Crea la nota
            GameObject noteObj = Instantiate(notePrefab, pos, Quaternion.identity);
            Note noteScript = noteObj.GetComponent<Note>();

            if (noteScript != null)
            {
                noteScript.noteType = noteTypes[i];
                noteScript.noteTime = noteTimes[i];
                noteScript.speed = effectiveSpeed;
                noteScript.hitPositionX = spawnPoint.position.x;
                noteScript.noteType = noteTypes[i];

                Debug.Log($"Nota creata: Tempo={noteTimes[i]} ms, Tipo={noteTypes[i]}, Posizione={pos}, Velocità={effectiveSpeed}");
            }
        }
}


}
