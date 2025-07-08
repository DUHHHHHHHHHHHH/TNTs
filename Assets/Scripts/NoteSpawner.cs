using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;
    public Transform spawnPoint;
    
    public float baseScrollSpeed = 5f;  // velocità normale base (unità al secondo)
    public float userScrollSpeed = 1f;  // moltiplicatore scelto dall’utente (es. da 0.5 a 2)


    public void SpawnAllNotes(List<int> noteTimes)


    {
        float cumulativeDistance = 0f;
        float effectiveSpeed = baseScrollSpeed * userScrollSpeed;

        for (int i = 0; i < noteTimes.Count; i++)
        {
            if (i > 0)
            {
                int deltaTime = noteTimes[i] - noteTimes[i - 1];
                cumulativeDistance += deltaTime * effectiveSpeed * 0.001f; // ms -> s
            }

            Vector3 pos = spawnPoint.position + Vector3.right * cumulativeDistance;

            GameObject noteObj = Instantiate(notePrefab, pos, Quaternion.identity);
            Note noteScript = noteObj.GetComponent<Note>();
            
            if (noteScript != null)
            {
                noteScript.noteTime = noteTimes[i];
                noteScript.speed = effectiveSpeed;
                noteScript.hitPositionX = spawnPoint.position.x;

                Debug.Log($"Nota creata: Tempo={noteTimes[i]} ms, Posizione={pos}, Velocità={effectiveSpeed}");
            }
        }
    }



}
